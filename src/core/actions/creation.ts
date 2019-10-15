import { CharacterInput } from '@core/inputs/character.input'
import { pipe } from 'fp-ts/lib/pipeable'
import { validateCharacter } from '@core/validations/character.validation'
import { Either, map } from 'fp-ts/lib/Either'
import { ValidationErrorList } from '@core/entities/error'
import { Option } from 'fp-ts/lib/Option'
import { liftA2ValidationOption } from '@core/utils/lifters'
import { notNone } from '@core/validations/common.validation'

export type StartDateInput = Option<Date>

export type CharacterCreationInput = {
    character: CharacterInput
    startDate: Date
}

export type StartDateResult = Either<ValidationErrorList<StartDateInput>, Date>
export type GameCreationResult = Either<ValidationErrorList<CharacterCreationInput>, CharacterCreationInput>

export interface CreationActions {
    setStartDate: (input: StartDateInput) => StartDateResult
    createCharacter: (input: CharacterCreationInput) => GameCreationResult
}

export default (): CreationActions => ({
    setStartDate: input => liftA2ValidationOption(notNone)(input, 'startDate'),

    createCharacter: input =>
        pipe(
            validateCharacter(input.character, input.startDate),
            map(() => input),
        ),
})
