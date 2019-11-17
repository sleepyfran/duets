import { ChangelogList } from '@core/entities/changelog'

export default interface ChangelogsData {
    saveChangelogs: (changelogs: ChangelogList) => void
    saveError: (error: Error) => void
}
