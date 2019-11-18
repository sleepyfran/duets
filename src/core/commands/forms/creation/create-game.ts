import Validation, { Result } from 'validum'
import Moment from 'moment'
import { Gender } from '@engine/entities/gender'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'

export type CreateGameInput = {
    name: string
    birthday: Date
    gender: Gender
    originCity: City
    instrument: Instrument
    gameStartDate: Date
}

export type CreateGame = (creationFormInput: CreateGameInput) => Result<CreateGameInput>

export default (): CreateGame => creationFormInput => {
    const result = Validation.of(creationFormInput)
        .property('name')
        .notEmpty()
        .andProperty('birthday')
        .truthy()
        .fulfills(formInput => Moment(formInput.gameStartDate).diff(formInput.birthday, 'years') >= 18)
        .withPropertyName('birthday')
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
