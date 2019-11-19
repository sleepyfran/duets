import { Genre } from '@engine/entities/genre'
import { Character } from '@engine/entities/character'

export type BandMember = {
    character: Character
    from: Date
    until: Date | undefined
}

export type Band = {
    name: string
    genre: Genre
    members: ReadonlyArray<BandMember>
}
