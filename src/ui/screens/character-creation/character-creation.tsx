import React, { FunctionComponent, useState } from 'react'
import HorizontalLayout from '@ui/components/layout/horizontal-layout'
import FullSizeSidebar from '@ui/components/sidebars/full-size.sidebar'
import { NavButton } from '@ui/components/buttons/nav/nav.button'
import Button from '@ui/components/buttons/button'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useHistory, useLocation } from 'react-router-dom'
import { stringToDate } from '@core/utils/mappers'
import CharacterForm, { CharacterFormInput } from '@ui/screens/character-creation/character.form'
import SkillsForm, { SkillsFormInput } from '@ui/screens/character-creation/skills.form'
import { Gender } from '@engine/entities/gender'
import { useForm } from '@ui/hooks/form.hooks'
import Config from '@config'
import { BandCreationScreen } from '@ui/screens/screens'
import { useStorage } from '@ui/hooks/storage.hooks'
import '@ui/styles/screens/character-creation.scss'

const CharacterCreation: FunctionComponent = () => {
    const history = useHistory()
    const startDateParam = useLocation().state
    const gameStartDate = stringToDate(startDateParam)
    const [getStorage] = useStorage()
    const store = getStorage()

    const database = store.database
    const cities = database.cities
    const instruments = database.instruments
    const skills = database.skills

    const form = useForm()

    const [characterInput, updateCharacterInput] = useState<CharacterFormInput>({
        name: '',
        gender: Gender.Male,
        birthday: new Date(),
        originCity: cities[0],
    })
    const [skillsInput, updateSkillInput] = useState<SkillsFormInput>({
        instrument: instruments[0],
        characterSkills: [],
        pointsLeft: Config.maxAssignablePoints,
    })

    const { createGame } = useCommands().forms.creation
    const handleGoOn = () => {
        form.clear()

        createGame({
            ...characterInput,
            ...skillsInput,
            skills: skillsInput.characterSkills,
            gameStartDate,
        }).then(result => {
            result.fold(form.markValidationErrors, () => history.push(BandCreationScreen.path))
        })
    }

    return (
        <HorizontalLayout
            className="character-creation"
            left={
                <FullSizeSidebar
                    className="main-menu"
                    navButton={NavButton.back}
                    onNavButtonClick={history.goBack}
                    header={
                        <CharacterForm
                            form={form}
                            cities={cities}
                            input={characterInput}
                            onUpdate={updateCharacterInput}
                        />
                    }
                />
            }
            right={
                <div className="instruments-skills">
                    <SkillsForm
                        form={form}
                        instruments={instruments}
                        skills={skills}
                        input={skillsInput}
                        onUpdate={updateSkillInput}
                    />
                    <Button onClick={handleGoOn}>Go on</Button>
                </div>
            }
        />
    )
}

export default CharacterCreation
