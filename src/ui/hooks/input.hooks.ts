import { useState } from 'react'

/**
 * An utility hook to create inputs that can show an error state. Avoid using directly; consider using `useForm()`
 * when possible.
 * @param id Unique ID that will be used in the validation to indicate which field has errors.
 * @param map Mapping from a string into the input's type.
 * @param initial Initial value of the input.
 */
export const useInput = <T>(id: string, map: (content: string) => T, initial: T | undefined = undefined) => {
    const [content, setContent] = useState<T>(initial || map(''))
    const [error, setError] = useState(false)
    const [dirty, setDirty] = useState(false)

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
