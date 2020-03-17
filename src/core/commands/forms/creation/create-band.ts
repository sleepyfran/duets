import Validation, { Result } from 'validum'
import Savegame from '@core/interfaces/savegame'
import { Genre } from '@engine/entities/genre'
import { Role } from '@engine/entities/role'
import { MemoryStorage } from '@core/interfaces/memory-storage'

export type CreateBandInput = {
    name: string
    genre: Genre
    role: Role
}

export type CreateBand = (formInput: CreateBandInput) => Promise<Result<CreateBandInput>>

export default (memoryStorage: MemoryStorage, savegame: Savegame): CreateBand => formInput => {
    const result = Validation.of(formInput)
        .property('name')
        .notEmpty()
        .andProperty('genre')
        .truthy()
        .andProperty('role')
        .truthy()
        .result()

    if (result.hasErrors()) return Promise.resolve(result)

    const storage = memoryStorage.get()

    storage.game.band = {
        name: formInput.name,
        genre: formInput.genre,
        members: [
            {
                character: storage.game.character,
                from: storage.game.calendar.date,
                until: undefined,
            },
        ],
    }

    memoryStorage.set(storage)

    return savegame.save(storage.game).then(() => result)
}
