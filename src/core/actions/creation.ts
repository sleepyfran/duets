import { CharacterInput } from '@core/inputs/character.input'
import { pipe } from 'fp-ts/lib/pipeable'
import { validateCharacter } from '@core/validations/character.validation'
import { Either, map } from 'fp-ts/lib/Either'
import { ValidationErrorList } from '@core/entities/error'

export type CharacterCreationInput = {
    character: CharacterInput
    startDate: Date
}

export type GameCreationResult = Either<ValidationErrorList<CharacterCreationInput>, CharacterCreationInput>

export interface CreationActions {
    createCharacter: (input: CharacterCreationInput) => GameCreationResult
}

export default (): CreationActions => ({
    createCharacter: input =>
        pipe(
            validateCharacter(input.character, input.startDate),
            map(() => input),
        ),
})
