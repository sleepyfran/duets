import React, { FunctionComponent } from 'react'
import CircularButton from '@ui/components/buttons/circular.button'
import { ReactComponent as CalendarIcon } from '@ui/assets/icons/calendar.svg'
import '@ui/styles/nav.button.scss'

type CalendarButtonProps = {
    className?: string
    onClick: () => void
}

const CalendarButton: FunctionComponent<CalendarButtonProps> = props => {
    return (
        <CircularButton
            className={`nav-button ${props.className}`}
            circleClassName="nav-button-circle"
            size="35"
            onClick={props.onClick}
        >
            <CalendarIcon />
        </CircularButton>
    )
}

export default CalendarButton
