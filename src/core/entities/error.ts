/**
 * Defines the different types of error that can happen at validation.
 */
import { NonEmptyArray } from 'fp-ts/lib/NonEmptyArray'

export enum ErrorType {
    MissingValue,
    InvalidValue,
}

/**
 * Defines a possible error that can occur while validating an input.
 */
export type ValidationError = {
    type: ErrorType
    property: string
    description: string
}

/**
 * Defines a list of possible errors that can occur while validating an input.
 */
export type ValidationErrorList<T> = NonEmptyArray<ValidationError>
