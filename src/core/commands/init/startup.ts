import CachedDatabase from '@core/interfaces/database/cached.database'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'

export type StartupCommand = () => Promise<boolean>

/**
 * Creates a command that will be executed at startup. Attempts to retrieve the database from the cache and save it
 * in memory.
 * @param cachedDatabase CacheDatabase dependency.
 * @param inMemoryDatabase InMemoryDatabase dependency.
 * @returns Promise that returns true if it was retrieved successfully, false otherwise.
 */
export default (cachedDatabase: CachedDatabase, inMemoryDatabase: InMemoryDatabase): StartupCommand => () =>
    cachedDatabase
        .get()
        .then(inMemoryDatabase.save)
        .then(database => database !== undefined)
