import React, { FunctionComponent } from 'react'
import { useHistory } from 'react-router-dom'
import Moment from 'moment'
import MenuButton from '@ui/components/buttons/nav/menu.button'
import CalendarButton from '@ui/components/buttons/nav/calendar.button'
import { TimeOfDay } from '@engine/entities/calendar'
import { StartScreen } from '@ui/screens/screens'
import { ReactComponent as CityIcon } from '@ui/assets/icons/city.svg'
import { ReactComponent as ArtistIcon } from '@ui/assets/icons/artist.svg'
import { ReactComponent as BandIcon } from '@ui/assets/icons/band.svg'
import { ReactComponent as PhoneIcon } from '@ui/assets/icons/phone.svg'
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

            <div className="center side">
                <CityIcon />
                <ArtistIcon />
                <BandIcon />
                <PhoneIcon />
            </div>

            <div className="right side">Forward to -></div>
        </div>
    )
}

export default Header
