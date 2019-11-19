import Validation, { Result } from 'validum'
import Moment from 'moment'
import { Gender } from '@engine/entities/gender'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'
import { Game } from '@core/entities/game'
import { CharacterSkill } from '@engine/entities/character-skill'
import { TimeOfDay } from '@engine/entities/calendar'
import { InMemoryGameData } from '@core/interfaces/gameplay/in-memory-game-data'
import Savegame from '@core/interfaces/savegame'

export type CreateGameInput = {
    name: string
    birthday: Date
    gender: Gender
    originCity: City
    instrument: Instrument
    gameStartDate: Date
    skills: ReadonlyArray<CharacterSkill>
}

export type CreateGame = (creationFormInput: CreateGameInput) => Promise<Result<CreateGameInput>>

export default (gameData: InMemoryGameData, savegame: Savegame): CreateGame => creationFormInput => {
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

    if (result.hasErrors()) return Promise.resolve(result)

    const initialGame: Game = {
        character: {
            name: creationFormInput.name,
            birthday: creationFormInput.birthday,
            gender: creationFormInput.gender,
            skills: creationFormInput.skills,
            fame: 0,
            health: 100,
            mood: 100,
        },
        calendar: {
            date: creationFormInput.gameStartDate,
            time: TimeOfDay.Morning,
        },
        band: undefined,
    }

    gameData.save(initialGame)

    return savegame.save(initialGame).then(() => result)
}
