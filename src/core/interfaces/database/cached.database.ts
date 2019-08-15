import { TaskEither } from 'fp-ts/lib/TaskEither'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import { City } from '@engine/entities/city'

/**
 * Defines a locally cached version of the remote database. Extends all the methods in such interface but adding the ability
 * to save the fetched resources.
 */
export default interface CachedDatabase extends RemoteDatabase {
    saveCities(cities: ReadonlyArray<City>): TaskEither<Error, ReadonlyArray<City>>
}
