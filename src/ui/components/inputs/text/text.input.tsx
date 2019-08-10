import React, { FunctionComponent } from 'react'
import './text.input.scss'

type TextInputProps = {
    label: string
}

const TextInput: FunctionComponent<TextInputProps> = props => {
    return (
        <div className="text-input">
            <label>{props.label}</label>
            <input type="text" />
        </div>
    )
}

export default TextInput
