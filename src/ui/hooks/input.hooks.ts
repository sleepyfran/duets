import React, { useState } from 'react'
import { keysOf } from '@utils/utils'
import { lens } from 'lens.ts'

export type InputHookProps<T> = {
    id: string
    map: (content: string) => T
    initial?: T
    onChange?: (input: T) => void
}

export type InputBinding<T> = {
    value: T
    error: boolean
    onChange: (value: string) => void
}

export type Input<T> = {
    content: T
    set: (value: T) => void
    dirty: boolean
    error: boolean
    setError: React.Dispatch<React.SetStateAction<boolean>>
    bind: InputBinding<T>
}

/**
 * An utility hook to create inputs that can show an error state. Avoid using directly; consider using `useForm()`
 * when possible.
 * @param props Props of the input, this includes:
 * - id: Unique ID that will be used in the validation to indicate which field has errors.
 * - map: Mapping from a string into the input's type.
 * - initial: Initial value of the input.
 * - onChange: A change callback when the input is updated.
 */
export const useInput = <T>(props: InputHookProps<T>): Input<T> => {
    const [content, setContent] = useState<T>(props.initial || props.map(''))
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
            onChange: (value: string) => {
                const mappedValue = props.map(value)
                set(mappedValue)
                if (props.onChange) props.onChange(mappedValue)
            },
        },
    }
}

export type InputChangeProps<T> = {
    /**
     * Input to map.
     */
    input: T

    /**
     * onChange function to use in the mapping.
     * @param input Input of the onChange.
     */
    onChange: (input: T) => void
}

type InputChangeMap<T> = {
    [P in keyof T]: (updated: T[P]) => T
}

/**
 * Utility hook that maps all the properties of a type into an `onChange` method that can be passed into any `useInput`
 * to map each property of a global type without creating individual setters for each of them.
 * @param props Props of the input.
 */
export const useInputChange = <T, K extends keyof T>(props: InputChangeProps<T>): InputChangeMap<T> => {
    const typeLens = lens<T>()

    const keys = keysOf(props.input)

    return keys.reduce(
        (result, key) => {
            result[key] = (updated: T[K]) => props.onChange(typeLens.k(key).set(updated)(props.input))
            return result
        },
        {} as any,
    )
}
