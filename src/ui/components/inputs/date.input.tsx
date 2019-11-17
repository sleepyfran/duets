import React, { FunctionComponent } from 'react'
import Moment from 'moment'

type DateInputProps = {
    value: Date
    label: string
    minDate?: Date
    maxDate?: Date
    error: boolean
    onChange: (value: string) => void
}

const formatDate = (date: Date) => Moment(date).format('YYYY-MM-DD')

const DateInput: FunctionComponent<DateInputProps> = props => {
    const formattedMaxDate = props.maxDate && formatDate(props.maxDate)
    const formattedMinDate = props.minDate && formatDate(props.minDate)

    return (
        <div className="input">
            <label>{props.label}</label>
            <input
                type="date"
                max={formattedMaxDate}
                min={formattedMinDate}
                onChange={event => props.onChange(event.target.value)}
                className={props.error ? 'error' : ''}
                value={formatDate(props.value)}
            />
        </div>
    )
}

export default DateInput
