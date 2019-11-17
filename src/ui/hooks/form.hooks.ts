import { Dispatch, SetStateAction } from 'react'
import { Input, InputHookProps, useInput } from '@ui/hooks/input.hooks'
import { ValidationError } from '@core/entities/error'

type InputRef = {
    id: string
    setError: Dispatch<SetStateAction<boolean>>
}

export type Form = {
    withInput: <T>(props: InputHookProps<T>) => Input<T>
    clear: () => void
    markValidationErrors: (validationErrors: ValidationError[]) => void
}

/**
 * An utility hook to quickly create forms containing different inputs that can show an error state.
 */
export const useForm = (): Form => {
    /**
     * List of inputs that the form will handle. The reason why we use a normal const instead of a React state is
     * because we *don't* want to trigger a re-render every time a new input is added to the form, since that creates
     * an infinite loop of re-renders.
     */
    const inputs: InputRef[] = []

    /**
     * Creates a new input with `useInput` and adds it to the list of inputs of the form.
     * @param props Props of the input.
     */
    const WithInput = <T>(props: InputHookProps<T>) => {
        const input = useInput(props)
        inputs.push({ id: props.id, setError: input.setError })
        return input
    }

    /**
     * Marks error as true for the input that has the given ID if it exists.
     * @param id Unique ID of the input.
     */
    const markErrorFor = (id: string) => {
        inputs.filter(input => input.id === id).forEach(input => input.setError(true))
    }

    /**
     * Marks error as true for a given list of validation errors.
     * @param validationErrors List of validation errors.
     */
    const markValidationErrors = (validationErrors: ValidationError[]) => {
        validationErrors.forEach(validationError => markErrorFor(validationError.property))
    }

    /**
     * Clears all errors from the form.
     */
    const clear = () => {
        inputs.forEach(input => input.setError(false))
    }

    return {
        withInput: WithInput,
        clear,
        markValidationErrors,
    }
}
