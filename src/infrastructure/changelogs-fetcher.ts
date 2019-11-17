import Changelogs from '@core/interfaces/changelogs/changelogs.fetcher'

const changelogsFetcher: Changelogs = {
    // Mocked for now. TODO: Implement it to fetch from GitHub once we release something.
    getLatest: () => new Promise(resolve => setTimeout(() => resolve([]), 3000)),
}

export default changelogsFetcher
