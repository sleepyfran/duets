/**
 * Defines a possible error that can occur while validating an input.
 */
export type ValidationError = {
    code?: number | string
    message: string
    property: string
}

/**
 * Defines a list of possible errors that can occur while validating an input.
 */
export type ValidationErrorList<T> = ValidationError[]
