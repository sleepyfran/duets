import React, { FunctionComponent, useState } from 'react'
import Layout from '@ui/components/layout/layout'
import FullSizeSidebar from '@ui/components/sidebars/full-size.sidebar'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import Button from '@ui/components/buttons/button'
import { useSelector } from 'react-redux'
import { State } from '@persistence/store/store'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useHistory, useLocation } from 'react-router-dom'
import { stringToDate } from '@core/utils/mappers'
import CharacterForm, { CharacterFormInput } from '@ui/screens/character-creation/character.form'
import SkillsForm, { SkillsFormInput } from '@ui/screens/character-creation/skills.form'
import { Gender } from '@engine/entities/gender'
import { useForm } from '@ui/hooks/form.hooks'
import '@ui/styles/screens/character-creation.scss'

const CharacterCreation: FunctionComponent = () => {
    const history = useHistory()
    const startDateParam = useLocation().state
    const gameStartDate = stringToDate(startDateParam)

    const database = useSelector((state: State) => state.database)
    const cities = database.cities
    const instruments = database.instruments
    const skills = database.skills

    const [characterInput, updateCharacterInput] = useState<CharacterFormInput>({
        name: '',
        gender: Gender.Male,
        birthday: new Date(),
        originCity: cities[0],
    })
    const [skillsInput, updateSkillInput] = useState<SkillsFormInput>({
        instrument: instruments[0],
        characterSkills: [],
    })

    const form = useForm()

    const { createGame } = useCommands().forms.creation
    const handleGoOn = () => {
        form.clear()

        const result = createGame({
            ...characterInput,
            ...skillsInput,
            gameStartDate,
        })

        form.markValidationErrors(result.errors())
    }

    return (
        <Layout
            className="character-creation"
            left={
                <FullSizeSidebar
                    className="main-menu"
                    navButton={NavButton.Back}
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
                        pointsLeft={0}
                        input={skillsInput}
                        onUpdate={updateSkillInput}
                    />
                    <Button className="go-button" onClick={handleGoOn}>
                        Go on
                    </Button>
                </div>
            }
        />
    )
}

export default CharacterCreation
