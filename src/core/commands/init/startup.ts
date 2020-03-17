import CachedDatabase from '@core/interfaces/database/cached.database'
import { MemoryStorage } from '@core/interfaces/memory-storage'

export type StartupCommand = () => Promise<boolean>

/**
 * Creates a command that will be executed at startup. Attempts to retrieve the database from the cache and save it
 * in memory.
 * @param cachedDatabase CacheDatabase dependency.
 * @param memoryStorage MemoryStorage dependency.
 * @returns Promise that returns true if it was retrieved successfully, false otherwise.
 */
export default (cachedDatabase: CachedDatabase, memoryStorage: MemoryStorage): StartupCommand => () =>
    cachedDatabase
        .get()
        .then(database => {
            const storage = memoryStorage.get()
            storage.database = database
            return storage
        })
        .then(storage => {
            memoryStorage.set(storage)
            return storage.database
        })
        .then(database => database !== undefined)
