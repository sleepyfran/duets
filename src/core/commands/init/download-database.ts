import CachedDatabase from '@core/interfaces/database/cached.database'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import { Database } from '@core/entities/database'

export type DownloadDatabaseCommand = () => Promise<Database>

/**
 * Creates a command that will be executed at startup. Attempts to retrieve the database from the cache and save it
 * in memory.
 * @param remoteDatabase RemoteDatabase dependency.
 * @param cachedDatabase CacheDatabase dependency.
 * @param inMemoryDatabase InMemoryDatabase dependency.
 * @returns Promise that returns true if it was retrieved successfully, false otherwise.
 */
export default (
    remoteDatabase: RemoteDatabase,
    cachedDatabase: CachedDatabase,
    inMemoryDatabase: InMemoryDatabase,
): DownloadDatabaseCommand => () =>
    remoteDatabase
        .get()
        .then(cachedDatabase.save)
        .then(inMemoryDatabase.save)
