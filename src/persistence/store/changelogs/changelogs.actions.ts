import { ChangelogList } from '@core/entities/changelog'
import { SaveErrorAction } from '@persistence/store/common'

export type SaveChangelogsAction = {
    type: 'saveChangelogsAction'
    changelogs: ChangelogList
}

export type ChangelogsAction = SaveChangelogsAction | SaveErrorAction

export const createSaveChangelogsAction = (changelogs: ChangelogList): SaveChangelogsAction => ({
    type: 'saveChangelogsAction',
    changelogs,
})
