import React, { FunctionComponent } from 'react'

type TextInputProps = {
    label: string
    value: string
    error: boolean
    onChange: (value: string) => void
}

const TextInput: FunctionComponent<TextInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <input
                type="text"
                onChange={event => props.onChange(event.target.value)}
                className={props.error ? 'error' : ''}
            />
        </div>
    )
}

export default TextInput
