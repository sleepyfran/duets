import React, { FunctionComponent } from 'react'
import useRouter from 'use-react-router'
import Layout from '@ui/components/layout/layout'
import './character-creation.scss'
import FullSizeSidebar from '@ui/components/sidebars/full-size-sidebar/full-size.sidebar'
import TextInput from '@ui/components/inputs/text.input'
import DateInput from '@ui/components/inputs/date.input'
import GenderInput from '@ui/components/inputs/gender/gender.input'
import SelectInput from '@ui/components/inputs/select.input'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import { useSelector } from 'react-redux'
import { State } from '@persistence/store/store'

const CharacterCreation: FunctionComponent = () => {
    const { history } = useRouter()

    const database = useSelector((state: State) => state.database)
    const cities = database.cities.map(city => ({
        label: `${city.country.flagEmoji} ${city.name}, ${city.country.name}`,
        value: city.name,
    }))

    const instruments = database.instruments.map(instrument => ({
        label: instrument.name,
        value: instrument.name.toLowerCase(),
    }))

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
                            <TextInput label="Name" />
                            <DateInput label="Birthday" />
                            <GenderInput label="Gender" />
                            <SelectInput label="Origin City" options={cities} />

                            <hr />
                            <DateInput label="Game start" maxDate={new Date()} />
                        </div>
                    }
                />
            }
            right={
                <div className="instruments-skills">
                    <h1>My instrument and skills</h1>
                    <div className="instrument">
                        <SelectInput label="First instrument" options={instruments} />
                    </div>
                </div>
            }
        />
    )
}

export default CharacterCreation
