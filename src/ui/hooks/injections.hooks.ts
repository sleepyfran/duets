/**
 * This file defines custom hooks to make the injection easier in every component by checking that the requested
 * injection is not undefined.
 */
import { useContext } from 'react'
import { InjectionsContext } from '@ui/contexts/injections.context'
import { ChangelogsQuery } from '@core/queries/changelogs'
import { WindowCommands } from '@core/commands/window'

const throwNoProviderError = (hookName: string) => {
    throw new Error(`${hookName} must be used within an InjectionContext.Provider`)
}

const useQueries = () => useContext(InjectionsContext).queries!!
const useCommands = () => useContext(InjectionsContext).commands!!

export const useChangelogs = (): ChangelogsQuery => {
    const { changelogs } = useQueries()

    if (changelogs === undefined) {
        throwNoProviderError('useChangelogs')
    }

    return changelogs!!
}

export const useWindow = (): WindowCommands => {
    const { window } = useCommands()

    if (window === undefined) {
        throwNoProviderError('useWindow')
    }

    return window!!
}
