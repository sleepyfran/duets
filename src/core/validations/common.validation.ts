import { fold, isNone, Option } from 'fp-ts/lib/Option'
import { Either, left, right } from 'fp-ts/lib/Either'
import { ValidationError } from '@core/entities/error'
import { pipe } from 'fp-ts/lib/pipeable'

/**
 * Creates a missing property error with a simple string and no type-safety. Use `missingPropertyError` if possible.
 * @param property Property name for the description.
 */
export const missingPropertyStringError = (property: string): ValidationError => ({
    message: `Property ${property} should be present in the input`,
    property: property,
})

/**
 * Creates a missing property error with type-safety.
 * @param property Key of R for the message.
 */
export const missingPropertyError = <R>(property: keyof R): ValidationError =>
    missingPropertyStringError(property.toString())

/**
 * Checks whether a certain property of the input is None.
 * @param input Input to check.
 * @param property Property to check.
 */
export const propertyNotNone = <R>(input: R, property: keyof R): Either<ValidationError, R> =>
    isNone((input as any)[property]) ? left(missingPropertyError(property)) : right(input)

/**
 * Validates that the given input is not none.
 * @param input Input to check.
 * @param property Property that we're checking. Simply used for the missing property error.
 */
export const notNone = <R>(input: Option<R>, property: string): Either<ValidationError, R> =>
    pipe(
        input,
        fold(
            () => left(missingPropertyStringError(property)), //
            r => right(r),
        ),
    )
