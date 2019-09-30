import { CharacterInput } from '@core/inputs/character.input'
import { chain, Either, fromOption, getValidation, left, map, right } from 'fp-ts/lib/Either'
import { pipe } from 'fp-ts/lib/pipeable'
import { getSemigroup, NonEmptyArray } from 'fp-ts/lib/NonEmptyArray'
import { sequenceT } from 'fp-ts/lib/Apply'
import differenceInCalendarYears from 'date-fns/differenceInCalendarYears'
import { liftA2Validation, liftValidation } from '@core/utils/lifters'
import { missingPropertyError, propertyNotNone } from '@core/validations/common.validation'
import { createNameOfType } from '@core/utils/nameof'
import { ErrorType, ValidationError } from '@core/entities/error'

export type CharacterValidation = Either<ValidationError, CharacterInput>
export type CharacterValidationResult = Either<NonEmptyArray<ValidationError>, CharacterInput>

const nameOfCharacterProperty = createNameOfType<CharacterInput>()

const nameMinLength = (character: CharacterInput): CharacterValidation =>
    character.name && character.name.length > 0
        ? right(character)
        : left({
              type: ErrorType.MissingValue,
              description: 'Name cannot be empty',
              property: nameOfCharacterProperty('name'),
          })

const over18YearsOld = (character: CharacterInput, gameStartDate: Date): CharacterValidation =>
    pipe(
        character.birthday,
        fromOption(() => missingPropertyError(nameOfCharacterProperty('birthday'))),
        chain((birthday: Date) =>
            differenceInCalendarYears(gameStartDate, birthday) >= 18
                ? right(character)
                : left({
                      type: ErrorType.InvalidValue,
                      description: 'The character must be 18 years older since the game start',
                      property: nameOfCharacterProperty('birthday'),
                  }),
        ),
    )

/**
 * Validates all character properties to check whether they have values (in the case of properties that are Option) or that
 * the provided values are valid.
 * @param character Character input as taken from the user to check.
 * @param gameStartDate Date in which the game will start. This is used to check against certain properties of the character.
 */
export const validateCharacter = (character: CharacterInput, gameStartDate: Date): CharacterValidationResult => {
    return pipe(
        getSemigroup<ValidationError>(),
        getValidation,
        validation =>
            sequenceT(validation)(
                liftValidation(nameMinLength)(character),
                liftA2Validation(over18YearsOld)(character, gameStartDate),
                liftA2Validation(propertyNotNone)(character, 'gender'),
                liftA2Validation(propertyNotNone)(character, 'originCity'),
                liftA2Validation(propertyNotNone)(character, 'instrument'),
            ),
        map(() => character),
    )
}
