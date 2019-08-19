import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'

/**
 * Defines the information saved in the static database that is downloaded from the game.
 */
export type Database = {
    instruments: ReadonlyArray<Instrument>
    cities: ReadonlyArray<City>
}
