import { TaskEither } from 'fp-ts/lib/TaskEither'
import { Database } from '@core/entities/database'

/**
 * Defines a database that is fetched from an external source like GitHub.
 */
export default interface RemoteDatabase {
    get: TaskEither<Error, Database>
}
