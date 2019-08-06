import { ChangelogList } from '@core/entities/changelog'
import { Loading } from '@persistence/store/common'

/**
 * Defines the possible states of the changelogs.
 */
export type ChangelogsState = Loading | ChangelogList | Error
