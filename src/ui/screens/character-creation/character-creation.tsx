import React, { FunctionComponent, useContext, useEffect, useState } from 'react'
import { head } from 'fp-ts/lib/Array'
import { isNone, some } from 'fp-ts/lib/Option'
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
import { Gender } from '@engine/entities/gender'
import { FormContext, formContextConsumer } from '@ui/contexts/form.context'
import './character-creation.scss'

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

    const { content: name, bind: bindName, setError: setNameError } = useInput(stringToString)
    const { content: birthday, bind: bindBirthday, setError: setBirthdayError } = useInput(stringToMaybeDate)
    const { content: gender, bind: bindGender } = useInput(stringToMaybeGender, some(Gender.Male))
    const { content: originCity, bind: bindOriginCity, setError: setOriginCityError } = useInput(
        value => stringToMaybeCity(value, cities),
        head([...cities]),
    )
    const { content: startDate, bind: bindStartDate, setError: setStartDateError } = useInput(stringToMaybeDate)
    const { content: instrument, bind: bindInstrument, setError: setInstrumentError } = useInput(
        value => stringToMaybeInstrument(value, instruments),
        head([...instruments]),
    )

    const initialPoints = 40
    const [pointsLeft, setPointsLeft] = useState(initialPoints)
    const characterSkills = useSelector((state: State) => state.gameplay.character.skills)
    useEffect(() => {
        const assigned = characterSkills.reduce((prev, curr) => prev + curr.level, 0)
        setPointsLeft(initialPoints - assigned)
    }, [characterSkills])

    const form = useContext(FormContext)
    const handleGoOn = () => {
        form.clearErrors()

        if (!name) setNameError(true)
        if (isNone(birthday)) setBirthdayError(true)
        if (isNone(originCity)) setOriginCityError(true)
        if (isNone(startDate)) setStartDateError(true)
        if (isNone(instrument)) setInstrumentError(true)
    }

    return (
        <FormContext.Provider value={formContextConsumer}>
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
        </FormContext.Provider>
    )
}

export default CharacterCreation
