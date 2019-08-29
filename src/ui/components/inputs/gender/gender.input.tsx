import React, { FunctionComponent, useState } from 'react'
import GenderButton from '@ui/components/buttons/gender/gender.button'
import { Gender } from '@engine/entities/gender'
import './gender.input.scss'

type GenderInputProps = {
    label: string
    onChange: (value: string) => void
}

const GenderInput: FunctionComponent<GenderInputProps> = props => {
    const [selection, setSelection] = useState(Gender.male)

    const handleOnClick = (gender: Gender) => {
        setSelection(gender)
        props.onChange(gender)
    }

    return (
        <div className="input gender-input">
            <label>{props.label}</label>
            <div className="gender-options">
                <GenderButton
                    onClick={() => handleOnClick(Gender.male)}
                    selected={selection === Gender.male}
                    gender={Gender.male}
                />
                <GenderButton
                    onClick={() => handleOnClick(Gender.female)}
                    selected={selection === Gender.female}
                    gender={Gender.female}
                />
            </div>
        </div>
    )
}

export default GenderInput
