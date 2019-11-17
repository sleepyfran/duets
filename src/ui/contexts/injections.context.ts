import React from 'react'
import { Commands } from '@core/commands/commands'

/**
 * Context in which we'll have access to the injections provided to the UI.
 */
export const CommandsContext = React.createContext<Commands>({} as Commands)
