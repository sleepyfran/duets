import Validation, { Result } from 'validum'
import { InMemoryGameData } from '@core/interfaces/gameplay/in-memory-game-data'
import Savegame from '@core/interfaces/savegame'
import { Genre } from '@engine/entities/genre'
import { Role } from '@engine/entities/role'
import { GameLenses } from '@core/lenses'

export type CreateBandInput = {
    name: string
    genre: Genre
    role: Role
}

export type CreateBand = (formInput: CreateBandInput) => Promise<Result<CreateBandInput>>

export default (gameData: InMemoryGameData, savegame: Savegame): CreateBand => formInput => {
    const result = Validation.of(formInput)
        .property('name')
        .notEmpty()
        .andProperty('genre')
        .truthy()
        .andProperty('role')
        .truthy()
        .result()

    if (result.hasErrors()) return Promise.resolve(result)

    const game = gameData.get()

    const updatedGame = GameLenses.band.set({
        name: formInput.name,
        genre: formInput.genre,
        members: [
            {
                character: game.character,
                from: game.calendar.date,
                until: undefined,
            },
        ],
    })(game)

    gameData.save(updatedGame)

    return savegame.save(updatedGame).then(() => result)
}
