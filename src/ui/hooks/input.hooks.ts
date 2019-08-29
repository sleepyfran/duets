import { useState } from 'react'

export const useInput = <T>(map: (content: string) => T, initial: T | undefined = undefined) => {
    const [content, setContent] = useState<T>(initial || map(''))
    const [dirty, setDirty] = useState(false)

    const set = (content: T) => {
        setContent(content)
        setDirty(true)
    }

    return {
        content,
        set,
        dirty,
        bind: {
            value: content,
            onChange: (value: string) => set(map(value)),
        },
    }
}
