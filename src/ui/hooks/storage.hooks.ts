import { useState, useEffect } from 'react'
import Store from '@storage/store'
import { Storage } from '@core/entities/storage'

/**
 * Creates a hook that allows the retrieval and update of the storage keeping the components in sync.
 */
export const useStorage = (): [() => Storage, (storage: Storage) => void] => {
    const [, update] = useState({})

    useEffect(() => {
        Store.subscribe(() => update({}))

        return () => {
            Store.unsubscribe(() => update({}))
        }
    }, [update])

    return [Store.get, Store.set]
}
