import { CharacterInput } from '@core/inputs/character.input'
import { pipe } from 'fp-ts/lib/pipeable'
import { validateCharacter } from '@core/validations/character.validation'
import { Either, flatten, map } from 'fp-ts/lib/Either'
import { ValidationErrorList } from '@core/entities/error'
import { notNone } from '@core/validations/common.validation'
import { Option } from 'fp-ts/lib/Option'
import { liftA2ValidationOption } from '@core/utils/lifters'

export type CharacterCreationInput = {
    character: CharacterInput
    startDate: Option<Date>
}

export type GameCreationResult = Either<ValidationErrorList<CharacterCreationInput>, CharacterCreationInput>

export interface CreationActions {
    createCharacter: (input: CharacterCreationInput) => GameCreationResult
}

export default (): CreationActions => ({
    createCharacter: input =>
        pipe(
            liftA2ValidationOption(notNone)(input.startDate, 'startDate'),
            map((startDate: Date) => validateCharacter(input.character, startDate)),
            flatten,
            map((characterInput: CharacterInput) => ({ character: characterInput, startDate: input.startDate })),
        ),
})
