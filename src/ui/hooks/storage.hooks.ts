import { useState, useEffect } from 'react'
import Store from '@storage/store'
import { Storage } from '@core/entities/storage'

/**
 * Creates a hook that allows the retrieval and update of the storage keeping the components in sync.
 */
export const useStorage = (): [() => Storage, (storage: Storage) => void] => {
    const [, update] = useState({})
    const subscription = () => update({})

    useEffect(() => {
        Store.subscribe(subscription)

        return () => {
            Store.unsubscribe(subscription)
        }
    }, [update])

    return [Store.get, Store.set]
}
