import React, { FunctionComponent } from 'react'
import HeaderLayout from '@ui/components/layout/header-layout'
import { TimeOfDay } from '@engine/entities/calendar'

const Home: FunctionComponent = () => {
    return (
        <HeaderLayout gameDate={new Date()} gameTime={TimeOfDay.Morning}>
            <h1>This is a test</h1>
        </HeaderLayout>
    )
}

export default Home
