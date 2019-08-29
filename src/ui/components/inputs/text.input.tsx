import React, { FunctionComponent } from 'react'

type TextInputProps = {
    label: string
    value: string
    onChange: (value: string) => void
}

const TextInput: FunctionComponent<TextInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <input type="text" onChange={event => props.onChange(event.target.value)} />
        </div>
    )
}

export default TextInput
