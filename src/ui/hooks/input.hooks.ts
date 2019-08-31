import { useContext, useState } from 'react'
import { FormContext } from '@ui/contexts/form.context'

export const useInput = <T>(map: (content: string) => T, initial: T | undefined = undefined) => {
    const [content, setContent] = useState<T>(initial || map(''))
    const [error, setError] = useState(false)
    const [dirty, setDirty] = useState(false)

    const formContext = useContext(FormContext)

    if (!formContext) {
        throw new Error('useInput cannot be used outside of a FormContext.')
    }

    // Register the current input in the FormContext.
    formContext.register({ setError })

    const set = (content: T) => {
        setContent(content)
        setDirty(true)
    }

    return {
        content,
        set,
        dirty,
        error,
        setError,
        bind: {
            value: content,
            error,
            onChange: (value: string) => set(map(value)),
        },
    }
}
