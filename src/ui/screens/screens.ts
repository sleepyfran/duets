import StartComponent from './start/start'
import { FunctionComponent } from 'react'

type Screen = {
    name: string
    path: string
    exact?: boolean
    component: FunctionComponent
}

export const StartScreen: Screen = {
    name: 'Start',
    path: '/',
    component: StartComponent,
}

/**
 * Screens available in the game. This translates to routes in the app.
 */
export default [StartScreen]
