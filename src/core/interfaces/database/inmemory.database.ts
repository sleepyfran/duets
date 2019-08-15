import { IO } from 'fp-ts/lib/IO'
import { City } from '@engine/entities/city'

/**
 * Defines operations for the in-memory version of the data fetched from a external source.
 */
export default interface InMemoryDatabase {
    saveCities(cities: ReadonlyArray<City>): IO<ReadonlyArray<City>>
}
