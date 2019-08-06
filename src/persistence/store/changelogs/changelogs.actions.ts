import { ChangelogList } from '@core/entities/changelog'

export type SaveChangelogsAction = {
    type: 'saveChangelogsAction'
    changelogs: ChangelogList
}

export type SaveErrorAction = {
    type: 'saveErrorAction'
    error: Error
}

export type ChangelogsAction = SaveChangelogsAction | SaveErrorAction

export const createSaveChangelogsAction = (changelogs: ChangelogList): SaveChangelogsAction => ({
    type: 'saveChangelogsAction',
    changelogs,
})

export const createSaveErrorAction = (error: Error): SaveErrorAction => ({
    type: 'saveErrorAction',
    error,
})
