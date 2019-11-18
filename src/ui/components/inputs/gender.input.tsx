import React, { FunctionComponent, useState } from 'react'
import GenderButton from '@ui/components/buttons/gender.button'
import { Gender } from '@engine/entities/gender'
import '@ui/styles/gender.input.scss'

type GenderInputProps = {
    value: Gender
    label: string
    error: boolean
    onChange: (value: string) => void
}

const GenderInput: FunctionComponent<GenderInputProps> = props => {
    const [selection, setSelection] = useState(props.value)

    const handleOnClick = (gender: Gender) => {
        setSelection(gender)
        props.onChange(gender)
    }

    return (
        <div className="input gender-input">
            <label>{props.label}</label>
            <div className="gender-options">
                <GenderButton
                    onClick={() => handleOnClick(Gender.Male)}
                    selected={selection === Gender.Male}
                    gender={Gender.Male}
                    error={props.error}
                />
                <GenderButton
                    onClick={() => handleOnClick(Gender.Female)}
                    selected={selection === Gender.Female}
                    gender={Gender.Female}
                    error={props.error}
                />
            </div>
        </div>
    )
}

export default GenderInput