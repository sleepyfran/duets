import React, { Dispatch, SetStateAction } from 'react'

type Input = {
    setError: Dispatch<SetStateAction<boolean>>
}

type FormContextConsumer = {
    inputs: Input[]
    register: (input: Input) => void
    clearErrors: () => void
}

export const formContextConsumer: FormContextConsumer = {
    inputs: [],
    register: function(input: Input) {
        this.inputs.push(input)
    },
    clearErrors: function() {
        this.inputs.forEach(input => input.setError(false))
    },
}

/**
 * Context in which each input of the current Form will register themselves so the form has quick access to functions
 * like resetting all errors.
 */
export const FormContext = React.createContext<FormContextConsumer>(formContextConsumer)
