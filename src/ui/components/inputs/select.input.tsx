import React, { FunctionComponent } from 'react'

type Option = {
    label: string
    value: string
}

type SelectInputProps = {
    label: string
    options: ReadonlyArray<Option>
}

const TextInput: FunctionComponent<SelectInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <select>
                {props.options.map(option => (
                    <option value={option.value}>{option.label}</option>
                ))}
            </select>
        </div>
    )
}

export default TextInput
