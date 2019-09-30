import React, { Dispatch, SetStateAction } from 'react'
import { NonEmptyArray } from 'fp-ts/lib/NonEmptyArray'
import { ValidationError } from '@core/entities/error'

type Input = {
    id: string
    setError: Dispatch<SetStateAction<boolean>>
}

type FormContextConsumer = {
    inputs: Input[]
    register: (input: Input) => void
    clearErrors: () => void
    setErrorFor: (inputId: string) => void
    markAllValidationErrors: (validationErrors: NonEmptyArray<ValidationError>) => void
}

export const formContextConsumer: FormContextConsumer = {
    inputs: [],
    register: function(input: Input) {
        this.inputs.push(input)
    },
    clearErrors: function() {
        this.inputs.forEach(input => input.setError(false))
    },
    setErrorFor: function(inputId: string) {
        this.inputs
            .filter(input => input.id === inputId) //
            .forEach(input => input.setError(true))
    },
    markAllValidationErrors: function(validationErrors: NonEmptyArray<ValidationError>) {
        validationErrors.forEach(error => this.setErrorFor(error.property))
    },
}

/**
 * Context in which each input of the current Form will register themselves so the form has quick access to functions
 * like resetting all errors.
 */
export const FormContext = React.createContext<FormContextConsumer>(formContextConsumer)
