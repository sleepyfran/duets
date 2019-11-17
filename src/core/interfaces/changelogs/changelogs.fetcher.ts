import { ChangelogList } from '@core/entities/changelog'

export default interface ChangelogsFetcher {
    getLatest: () => Promise<ChangelogList>
}
