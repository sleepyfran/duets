import React, { FunctionComponent, useState } from 'react'
import GenderButton from '@ui/components/buttons/gender/gender.button'
import { Gender } from '@core/types/gender'
import './gender.input.scss'

type GenderInputProps = {
    label: string
}

const GenderInput: FunctionComponent<GenderInputProps> = props => {
    const [selection, setSelection] = useState(Gender.male)

    return (
        <div className="input gender-input">
            <label>{props.label}</label>
            <div className="gender-options">
                <GenderButton
                    onClick={() => setSelection(Gender.male)}
                    selected={selection === Gender.male}
                    gender={Gender.male}
                />
                <GenderButton
                    onClick={() => setSelection(Gender.female)}
                    selected={selection === Gender.female}
                    gender={Gender.female}
                />
            </div>
        </div>
    )
}

export default GenderInput
