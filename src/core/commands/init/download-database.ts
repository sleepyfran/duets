import CachedDatabase from '@core/interfaces/database/cached.database'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import { Database } from '@core/entities/database'
import { MemoryStorage } from '@core/interfaces/memory-storage'

export type DownloadDatabaseCommand = () => Promise<Database>

/**
 * Creates a command that will be executed at startup. Attempts to retrieve the database from the cache and save it
 * in memory.
 * @param remoteDatabase RemoteDatabase dependency.
 * @param cachedDatabase CacheDatabase dependency.
 * @param memoryStorage MemoryStorage dependency.
 * @returns Promise that returns true if it was retrieved successfully, false otherwise.
 */
export default (
    remoteDatabase: RemoteDatabase,
    cachedDatabase: CachedDatabase,
    memoryStorage: MemoryStorage,
): DownloadDatabaseCommand => () =>
    remoteDatabase
        .get()
        .then(cachedDatabase.save)
        .then(database => {
            const storage = memoryStorage.get()
            storage.database = database
            return storage
        })
        .then(storage => {
            memoryStorage.set(storage)
            return storage.database
        })
