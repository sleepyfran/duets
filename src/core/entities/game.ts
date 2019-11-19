import { Calendar } from '@engine/entities/calendar'
import { Character } from '@engine/entities/character'
import { Band } from '@engine/entities/band'

/**
 * Defines the data stored in a save game and in the current game being played.
 */
export type Game = {
    calendar: Calendar
    character: Character
    band: Band | undefined
}
