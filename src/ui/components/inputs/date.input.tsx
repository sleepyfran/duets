import React, { FunctionComponent } from 'react'

type DateInputProps = {
    label: string
}

const DateInput: FunctionComponent<DateInputProps> = props => {
    return (
        <div className="input">
            <label>{props.label}</label>
            <input type="date" />
        </div>
    )
}

export default DateInput
