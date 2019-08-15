import { fromIO, Task } from 'fp-ts/lib/Task'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/TaskEither'
import ChangelogsFetcher from '@core/interfaces/changelogs/changelogs.fetcher'
import ChangelogsData from '@core/interfaces/changelogs/changelogs.data'

export interface ChangelogsQuery {
    fetchAndSave: Task<void>
}

export default (fetcher: ChangelogsFetcher, data: ChangelogsData): ChangelogsQuery => ({
    fetchAndSave: pipe(
        fetcher.getLatest(),
        fold(error => fromIO(data.saveError(error)), changelogs => fromIO(data.saveChangelogs(changelogs))),
    ),
})
