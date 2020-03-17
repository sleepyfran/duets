import { Storage } from '@core/entities/storage'

export type Listener = () => void

/**
 * Defines a storage that is saved in-memory.
 */
export interface MemoryStorage {
    /**
     * Retrieves the full storage content from the memory storage.
     */
    get: () => Storage

    /**
     * Saves a full given storage content to the memory storage.
     */
    set: (storage: Storage) => void

    /**
     * Subscribes the given function to updates of content in the storage.
     */
    subscribe: (listener: Listener) => void

    /**
     * Unsubscribes the given function of updates of content in the storage.
     */
    unsubscribe: (listener: Listener) => void
}
