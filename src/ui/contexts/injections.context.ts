import React from 'react'
import { Queries } from '@core/queries/queries'

/**
 * Defines the queries and commands that will be injected into the UI.
 */
export type Injections = Queries

/**
 * Context in which we'll have access to the injections provided to the UI.
 */
export const InjectionsContext = React.createContext<Partial<Injections>>({})
