import { Database } from '@core/entities/database'

/**
 * Defines operations for the in-memory version of the data fetched from a external source.
 */
export default interface InMemoryDatabase {
    save(database: Database): Database
}
