import { FunctionComponent } from 'react'
import StartComponent from './start'
import CharacterCreation from '@ui/screens/character-creation/character-creation'
import BandCreation from '@ui/screens/band-creation/band-creation'

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

export const BandCreationScreen: Screen = {
    name: 'BandCreation',
    path: '/band-creation',
    component: BandCreation,
}

/**
 * Screens available in the game. This translates to routes in the app.
 */
export default [StartScreen, CharacterCreationScreen, BandCreationScreen]
