import React, { FunctionComponent, useState } from 'react'
import GenderButton from '@ui/components/buttons/gender/gender.button'
import { Gender } from '@engine/entities/gender'
import './gender.input.scss'

type GenderInputProps = {
    label: string
    onChange: (value: string) => void
}

const GenderInput: FunctionComponent<GenderInputProps> = props => {
    const [selection, setSelection] = useState(Gender.Male)

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
                />
                <GenderButton
                    onClick={() => handleOnClick(Gender.Female)}
                    selected={selection === Gender.Female}
                    gender={Gender.Female}
                />
            </div>
        </div>
    )
}

export default GenderInput
