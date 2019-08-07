import React from 'react'
import { Queries } from '@core/queries/queries'
import { Commands } from '@core/commands/commands'

/**
 * Defines the queries and commands that will be injected into the UI.
 */
export type Injections = {
    queries: Queries
    commands: Commands
}

/**
 * Context in which we'll have access to the injections provided to the UI.
 */
export const InjectionsContext = React.createContext<Partial<Injections>>({})
