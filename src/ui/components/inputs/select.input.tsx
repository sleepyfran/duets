import React, { FunctionComponent } from 'react'

type Option = {
    label: string
    value: string
}

type SelectInputProps = {
    label: string
    options: ReadonlyArray<Option>
    onChange: (value: string) => void
}

const SelectInput: FunctionComponent<SelectInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <select onChange={event => props.onChange(event.target.value)}>
                {props.options.map(option => (
                    <option value={option.value}>{option.label}</option>
                ))}
            </select>
        </div>
    )
}

export default SelectInput
