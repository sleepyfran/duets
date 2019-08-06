/**
 * This file defines custom hooks to make the injection easier in every component by checking that the requested
 * injection is not undefined.
 */
import { useContext } from 'react'
import { InjectionsContext } from '@ui/contexts/injections.context'
import { ChangelogsQuery } from '@core/queries/changelogs'

const throwNoProviderError = (hookName: string) => {
    throw new Error(`${hookName} must be used within an InjectionContext.Provider`)
}

export const useChangelogs = (): ChangelogsQuery => {
    const { changelogs } = useContext(InjectionsContext)

    if (changelogs === undefined) {
        throwNoProviderError('useChangelogs')
    }

    return changelogs!!
}
