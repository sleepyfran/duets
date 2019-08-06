import { TaskEither } from 'fp-ts/lib/TaskEither'
import { ChangelogList } from '@core/entities/changelog'

export default interface ChangelogsFetcher {
    getLatest(): TaskEither<Error, ChangelogList>
}
