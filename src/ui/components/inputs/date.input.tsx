import React, { FunctionComponent } from 'react'
import Moment from 'moment'

type DateInputProps = {
    label: string
    minDate?: Date
    maxDate?: Date
}

const formattedDate = (date: Date) => Moment(date).format('YYYY-MM-DD')

const DateInput: FunctionComponent<DateInputProps> = props => {
    const formattedMaxDate = props.maxDate && formattedDate(props.maxDate)
    const formattedMinDate = props.minDate && formattedDate(props.minDate)

    return (
        <div className="input">
            <label>{props.label}</label>
            <input type="date" max={formattedMaxDate} min={formattedMinDate} />
        </div>
    )
}

export default DateInput