import React, { FunctionComponent } from 'react'
import { useHistory } from 'react-router-dom'
import Moment from 'moment'
import MenuButton from '@ui/components/buttons/nav/menu.button'
import CalendarButton from '@ui/components/buttons/nav/calendar.button'
import { TimeOfDay } from '@engine/entities/calendar'
import { StartScreen } from '@ui/screens/screens'
import '@ui/styles/header.scss'

type HeaderProps = {
    gameDate: Date
    gameTime: TimeOfDay
}

const Header: FunctionComponent<HeaderProps> = props => {
    const history = useHistory()

    return (
        <div className="header">
            <div className="left side">
                <MenuButton onClick={() => history.replace(StartScreen.path)} />
                <CalendarButton className="calendar" onClick={() => {}} />
                <h3 className="date">{Moment(props.gameDate).format('DD/MM/YYYY')}</h3>
                <h3 className="time">{TimeOfDay[props.gameTime]}</h3>
            </div>

            <div className="center side">Main menu icons</div>

            <div className="right side">Forward to -></div>
        </div>
    )
}

export default Header
