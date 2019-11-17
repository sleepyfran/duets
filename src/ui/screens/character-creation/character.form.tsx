import React, { FunctionComponent } from 'react'
import TextInput from '@ui/components/inputs/text.input'
import DateInput from '@ui/components/inputs/date.input'
import GenderInput from '@ui/components/inputs/gender.input'
import SelectInput from '@ui/components/inputs/select.input'
import { stringToCity, stringToDate, stringToGender, stringToString } from '@core/utils/mappers'
import { Gender } from '@engine/entities/gender'
import { Form } from '@ui/hooks/form.hooks'
import { City } from '@engine/entities/city'
import { useInputChange } from '@ui/hooks/input.hooks'

export type CharacterFormInput = {
    name: string
    birthday: Date
    gender: Gender
    originCity: City
}

type FormProps = {
    form: Form
    input: CharacterFormInput
    cities: ReadonlyArray<City>
    onUpdate: (character: CharacterFormInput) => void
}

const CharacterForm: FunctionComponent<FormProps> = (props: FormProps) => {
    const citiesSelect = props.cities.map(city => ({
        label: `${city.country.flagEmoji} ${city.name}, ${city.country.name}`,
        value: city.name,
    }))

    const {
        name: onUpdateName,
        birthday: onUpdateBirthday,
        gender: onUpdateGender,
        originCity: onUpdateOriginCity,
    } = useInputChange({
        input: props.input,
        onChange: props.onUpdate,
    })

    const { bind: bindName } = props.form.withInput({
        id: 'name',
        map: stringToString,
        onChange: onUpdateName,
        initial: props.input.name,
    })
    const { bind: bindBirthday } = props.form.withInput({
        id: 'birthday',
        map: stringToDate,
        onChange: onUpdateBirthday,
        initial: props.input.birthday,
    })
    const { bind: bindGender } = props.form.withInput({
        id: 'gender',
        map: stringToGender,
        onChange: onUpdateGender,
        initial: props.input.gender,
    })
    const { bind: bindOriginCity } = props.form.withInput({
        id: 'originCity',
        map: value => stringToCity(value, props.cities),
        onChange: onUpdateOriginCity,
        initial: props.input.originCity,
    })

    return (
        <div>
            <h1>Character creation</h1>
            <TextInput label="Name" {...bindName} />
            <DateInput label="Birthday" {...bindBirthday} />
            <GenderInput label="Gender" {...bindGender} />
            <SelectInput label="Origin City" options={citiesSelect} {...bindOriginCity} />
        </div>
    )
}

export default CharacterForm
