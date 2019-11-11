import RemoteDatabase from '@core/interfaces/database/remote.database'
import { Database } from '@core/entities/database'

/**
 * Defines a locally cached version of the remote database. Extends all the methods in such interface but adding the ability
 * to save the fetched resources.
 */
export default interface CachedDatabase extends RemoteDatabase {
    save: (database: Database) => Promise<Database>
}
