import React from 'react'
import { Actions } from '@core/actions/actions'
import { Commands } from '@core/commands/commands'

/**
 * Defines the actions that will be injected into the UI.
 */
export type Injections = Actions

/**
 * Context in which we'll have access to the injections provided to the UI.
 */
export const InjectionsContext = React.createContext<Injections>({} as Injections)
export const CommandsContext = React.createContext<Commands>({} as Commands)
