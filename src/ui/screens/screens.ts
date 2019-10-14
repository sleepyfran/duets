import { FunctionComponent } from 'react'
import StartComponent from './start'
import CharacterCreation from '@ui/screens/character-creation'

type Screen = {
    name: string
    path: string
    exact?: boolean
    component: FunctionComponent
}

export const StartScreen: Screen = {
    name: 'Start',
    path: '/',
    exact: true,
    component: StartComponent,
}

export const CharacterCreationScreen: Screen = {
    name: 'CharacterCreation',
    path: '/character-creation',
    component: CharacterCreation,
}

/**
 * Screens available in the game. This translates to routes in the app.
 */
export default [StartScreen, CharacterCreationScreen]
