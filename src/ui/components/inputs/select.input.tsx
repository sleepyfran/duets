import React, { FunctionComponent } from 'react'

type Option = {
    label: string
    value: string
}

type SelectInputProps = {
    value: any
    label: string
    options: ReadonlyArray<Option>
    error: boolean
    onChange: (value: string) => void
}

const SelectInput: FunctionComponent<SelectInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <select onChange={event => props.onChange(event.target.value)} className={props.error ? 'error' : ''}>
                {props.options.map(option => (
                    <option key={option.label} defaultValue={props.value} value={option.value}>
                        {option.label}
                    </option>
                ))}
            </select>
        </div>
    )
}

export default SelectInput
