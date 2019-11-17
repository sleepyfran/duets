import ChangelogsFetcher from '@core/interfaces/changelogs/changelogs.fetcher'
import ChangelogsData from '@core/interfaces/changelogs/changelogs.data'

export type LoadChangelogCommand = () => void

export default (fetcher: ChangelogsFetcher, data: ChangelogsData): LoadChangelogCommand => () =>
    fetcher
        .getLatest()
        .then(data.saveChangelogs)
        .catch(data.saveError)
