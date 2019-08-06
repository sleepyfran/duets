import { IO } from 'fp-ts/lib/IO'
import { ChangelogList } from '@core/entities/changelog'

export default interface ChangelogsData {
    saveChangelogs(changelogs: ChangelogList): IO<void>
    saveError(error: Error): IO<void>
}
