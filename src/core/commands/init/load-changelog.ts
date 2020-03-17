import ChangelogsFetcher from '@core/interfaces/changelogs/changelogs.fetcher'
import { MemoryStorage } from '@core/interfaces/memory-storage'

export type LoadChangelogCommand = () => void

export default (fetcher: ChangelogsFetcher, memoryStorage: MemoryStorage): LoadChangelogCommand => () => {
    const store = memoryStorage.get()

    return fetcher
        .getLatest()
        .then(changelogList => {
            store.ui.changelogList = changelogList
            memoryStorage.set(store)
        })
        .catch(error => {
            store.ui.changelogList = error
            memoryStorage.set(store)
        })
}
