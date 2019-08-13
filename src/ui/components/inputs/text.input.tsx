import React, { FunctionComponent } from 'react'

type TextInputProps = {
    label: string
}

const TextInput: FunctionComponent<TextInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <input type="text" />
        </div>
    )
}

export default TextInput
