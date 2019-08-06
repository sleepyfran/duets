import { tryCatch } from 'fp-ts/lib/TaskEither'
import Changelogs from '@core/interfaces/changelogs/changelogs.fetcher'

const changelogs: Changelogs = {
    // Mocked for now. TODO: Implement it to fetch from GitHub once we release something.
    getLatest: () =>
        tryCatch(() => new Promise(resolve => setTimeout(() => resolve([]), 3000)), error => new Error(String(error))),
}

export default changelogs
