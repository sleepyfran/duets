import React, { FunctionComponent } from 'react'
import '@ui/styles/layout.scss'
import Header from '@ui/components/header'
import { TimeOfDay } from '@engine/entities/calendar'

type LayoutProps = {
    gameDate: Date
    gameTime: TimeOfDay
}

const HeaderLayout: FunctionComponent<LayoutProps> = props => {
    return (
        <div className="header-layout">
            <Header {...props} />
            {props.children}
        </div>
    )
}

export default HeaderLayout
