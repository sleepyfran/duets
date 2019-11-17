import Validation, { Result } from 'validum'
import { CreationInput } from '@core/inputs/creation.input'

export type CreateGame = (creationFormInput: CreationInput) => Result<CreationInput>

export default (): CreateGame => creationFormInput => {
    const result = Validation.of(creationFormInput)
        .property('name')
        .notEmpty()
        .andProperty('birthday')
        .truthy()
        .andProperty('gender')
        .truthy()
        .andProperty('originCity')
        .truthy()
        .andProperty('instrument')
        .truthy()
        .result()

    if (result.hasErrors()) return result

    // TODO: Create the character
    console.log('Everything is fine, continue!')

    return result
}
