import React, { FunctionComponent, useEffect, useState } from 'react'
import { head } from 'fp-ts/lib/Array'
import { getOrElse, some } from 'fp-ts/lib/Option'
import {
    stringToMaybeCity,
    stringToMaybeDate,
    stringToMaybeGender,
    stringToMaybeInstrument,
    stringToString,
} from '@core/utils/mappers'
import Layout from '@ui/components/layout/layout'
import FullSizeSidebar from '@ui/components/sidebars/full-size.sidebar'
import TextInput from '@ui/components/inputs/text.input'
import DateInput from '@ui/components/inputs/date.input'
import GenderInput from '@ui/components/inputs/gender.input'
import SelectInput from '@ui/components/inputs/select.input'
import SkillsTable from '@ui/components/tables/skills.table'
import Info from '@ui/components/info/info'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import Button from '@ui/components/buttons/button'
import { useSelector } from 'react-redux'
import { State } from '@persistence/store/store'
import { Gender } from '@engine/entities/gender'
import { MAX_ASSIGNABLE_LEVEL_POINTS } from '@engine/operations/skill.operations'
import '@ui/styles/screens/character-creation.scss'
import { CharacterInput } from '@core/inputs/character.input'
import { useActions } from '@ui/hooks/injections.hooks'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/Either'
import { useHistory, useLocation } from 'react-router-dom'
import { useForm } from '@ui/hooks/form.hooks'

const CharacterCreation: FunctionComponent = () => {
    const history = useHistory()
    const startDateParam = useLocation().state
    const startDate = pipe(
        startDateParam || '',
        stringToMaybeDate,
        getOrElse(() => new Date()),
    )

    const database = useSelector((state: State) => state.database)
    const cities = database.cities
    const citiesSelect = cities.map(city => ({
        label: `${city.country.flagEmoji} ${city.name}, ${city.country.name}`,
        value: city.name,
    }))

    const instruments = database.instruments
    const instrumentsSelect = instruments.map(instrument => ({
        label: instrument.name,
        value: instrument.name,
    }))

    const [assignedPoints, setAssignedPoints] = useState(0)
    const pointsLeft = MAX_ASSIGNABLE_LEVEL_POINTS - assignedPoints
    const characterSkills = useSelector((state: State) => state.gameplay.character.skills)
    useEffect(() => {
        const assigned = characterSkills.reduce((prev, curr) => prev + curr.level, 0)
        setAssignedPoints(assigned)
    }, [characterSkills])

    const form = useForm()
    const { content: name, bind: bindName } = form.withInput('name', stringToString)
    const { content: birthday, bind: bindBirthday } = form.withInput('birthday', stringToMaybeDate)
    const { content: gender, bind: bindGender } = form.withInput('gender', stringToMaybeGender, some(Gender.Male))
    const { content: originCity, bind: bindOriginCity } = form.withInput(
        'originCity',
        value => stringToMaybeCity(value, cities),
        head([...cities]),
    )
    const { content: instrument, bind: bindInstrument } = form.withInput(
        'instrument',
        value => stringToMaybeInstrument(value, instruments),
        head([...instruments]),
    )

    const creation = useActions().creation
    const handleGoOn = () => {
        form.clear()

        const characterInput: CharacterInput = {
            name,
            birthday,
            gender,
            instrument,
            originCity,
        }

        pipe(
            creation.createCharacter({
                character: characterInput,
                startDate,
            }),
            fold(
                validationErrors => form.markValidationErrors(validationErrors),
                () => console.log('Everything is alright :)'),
            ),
        )
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
                        <div>
                            <h1>Character creation</h1>
                            <TextInput label="Name" {...bindName} />
                            <DateInput label="Birthday" {...bindBirthday} />
                            <GenderInput label="Gender" {...bindGender} />
                            <SelectInput label="Origin City" options={citiesSelect} {...bindOriginCity} />
                        </div>
                    }
                />
            }
            right={
                <div className="instruments-skills">
                    <h1>My instrument and skills</h1>
                    <div className="instrument">
                        <SelectInput label="Initial instrument" options={instrumentsSelect} {...bindInstrument} />
                    </div>
                    <Info text={`You can assign ${pointsLeft} more points to these skills`} />
                    <SkillsTable assignedPoints={assignedPoints} />
                    <Button className="go-button" onClick={handleGoOn}>
                        Go on
                    </Button>
                </div>
            }
        />
    )
}

export default CharacterCreation
