import { Dispatch, SetStateAction } from 'react'
import { useInput } from '@ui/hooks/input.hooks'
import { ValidationError } from '@core/entities/error'

type Input = {
    id: string
    setError: Dispatch<SetStateAction<boolean>>
}

/**
 * An utility hook to quickly create forms containing different inputs that can show an error state.
 */
export const useForm = () => {
    /**
     * List of inputs that the form will handle. The reason why we use a normal const instead of a React state is
     * because we *don't* want to trigger a re-render every time a new input is added to the form, since that creates
     * an infinite loop of re-renders.
     */
    const inputs: Input[] = []

    /**
     * Creates a new input with `useInput` and adds it to the list of inputs of the form.
     * @param id Unique ID that will be used in the validation to indicate which field has errors.
     * @param map Mapping from a string into the input's type.
     * @param initial Initial value of the input.
     */
    const WithInput = <T>(id: string, map: (content: string) => T, initial: T | undefined = undefined) => {
        const input = useInput(id, map, initial)
        inputs.push({ id, setError: input.setError })
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
