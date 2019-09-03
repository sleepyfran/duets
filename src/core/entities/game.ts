import { Calendar } from '@engine/entities/calendar'
import { Character } from '@engine/entities/character'

/**
 * Defines the data stored in a save game and in the current game being played.
 */
export type Game = {
    calendar: Calendar
    character: Character
}
