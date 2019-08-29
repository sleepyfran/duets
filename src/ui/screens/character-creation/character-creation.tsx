import React, { FunctionComponent, useEffect, useState } from 'react'
import {
    stringToMaybeCity,
    stringToMaybeDate,
    stringToMaybeGender,
    stringToMaybeInstrument,
    stringToString,
} from '@core/utils/mappers'
import useRouter from 'use-react-router'
import Layout from '@ui/components/layout/layout'
import FullSizeSidebar from '@ui/components/sidebars/full-size-sidebar/full-size.sidebar'
import TextInput from '@ui/components/inputs/text.input'
import DateInput from '@ui/components/inputs/date.input'
import GenderInput from '@ui/components/inputs/gender/gender.input'
import SelectInput from '@ui/components/inputs/select.input'
import SkillsTable from '@ui/components/tables/skills.table'
import Info from '@ui/components/info/info'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import Button from '@ui/components/buttons/button'
import { useSelector } from 'react-redux'
import { State } from '@persistence/store/store'
import { useInput } from '@ui/hooks/input.hooks'
import './character-creation.scss'
import { some } from 'fp-ts/lib/Option'
import { Gender } from '@engine/entities/gender'
import { head } from 'fp-ts/lib/Array'

const CharacterCreation: FunctionComponent = () => {
    const { history } = useRouter()

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

    const { content: name, bind: bindName } = useInput(stringToString)
    const { content: birthday, bind: bindBirthday } = useInput(stringToMaybeDate)
    const { content: gender, bind: bindGender } = useInput(stringToMaybeGender, some(Gender.male))
    const { content: originCity, bind: bindOriginCity } = useInput(
        value => stringToMaybeCity(value, cities),
        head([...cities]),
    )
    const { content: startDate, bind: bindStartDate } = useInput(stringToMaybeDate)
    const { content: instrument, bind: bindInstrument } = useInput(
        value => stringToMaybeInstrument(value, instruments),
        head([...instruments]),
    )

    const initialPoints = 40
    const [pointsLeft, setPointsLeft] = useState(initialPoints)
    const characterSkills = useSelector((state: State) => state.gameplay.skills)
    useEffect(() => {
        const assigned = characterSkills.reduce((prev, curr) => prev + curr.level, 0)
        setPointsLeft(initialPoints - assigned)
    }, [characterSkills])

    const handleGoOn = () => {
        console.log('name', name)
        console.log('birthday', birthday)
        console.log('gender', gender)
        console.log('origin city', originCity)
        console.log('start date', startDate)
        console.log('instrument', instrument)
    }

    return (
        <Layout
            className="character-creation"
            left={
                <FullSizeSidebar
                    className="main-menu"
                    navButton={NavButton.back}
                    onNavButtonClick={history.goBack}
                    header={
                        <div>
                            <h1>Character creation</h1>
                            <TextInput label="Name" {...bindName} />
                            <DateInput label="Birthday" {...bindBirthday} />
                            <GenderInput label="Gender" {...bindGender} />
                            <SelectInput label="Origin City" options={citiesSelect} {...bindOriginCity} />

                            <hr />
                            <DateInput label="Game start date" maxDate={new Date()} {...bindStartDate} />
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
                    <SkillsTable pointsLeft={pointsLeft} />
                    <Button className="go-button" onClick={handleGoOn}>
                        Go on
                    </Button>
                </div>
            }
        />
    )
}

export default CharacterCreation
