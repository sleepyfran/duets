import { Either, mapLeft } from 'fp-ts/lib/Either'
import { pipe } from 'fp-ts/lib/pipeable'
import { NonEmptyArray } from 'fp-ts/lib/NonEmptyArray'
import { Option } from 'fp-ts/lib/Option'

/**
 * Transforms an Either<E, R> into an Either<E[], R>. This comes in handy for validation, since it allows us to collect
 * the errors that happen on a validation pipeline, returning either the resource we are validating (indicating that everything
 * went well) or a list of errors that happened while validating the resource.
 * @param validate Function to apply when validating the resource.
 */
export const liftValidation = <E, R>(validate: (r: R) => Either<E, R>): ((r: R) => Either<NonEmptyArray<E>, R>) => r =>
    pipe(
        validate(r),
        mapLeft(e => [e]),
    )

/**
 * Same as liftValidation but with two arguments.
 */
export const liftA2Validation = <E, R, P>(
    validate: (r: R, p: P) => Either<E, R>,
): ((r: R, p: P) => Either<NonEmptyArray<E>, R>) => (r, p) =>
    pipe(
        validate(r, p),
        mapLeft(e => [e]),
    )

/**
 * Same as liftA2Validation but accepting an Option as an argument.
 */
export const liftA2ValidationOption = <E, R, P>(
    validate: (r: Option<R>, p: P) => Either<E, R>,
): ((r: Option<R>, p: P) => Either<NonEmptyArray<E>, R>) => (r, p) =>
    pipe(
        validate(r, p),
        mapLeft(e => [e]),
    )
