import { TaskEither } from 'fp-ts/lib/TaskEither'
import { City } from '@engine/entities/city'

/**
 * Defines a database that is fetched from an external source like GitHub.
 */
export default interface RemoteDatabase {
    getCities: TaskEither<Error, ReadonlyArray<City>>
}
