import { Dispatch } from 'redux'
import ChangelogsData from '@core/interfaces/changelogs/changelogs.data'
import { ChangelogList } from '@core/entities/changelog'
import { ChangelogsAction, createSaveChangelogsAction } from './changelogs.actions'
import { createSaveErrorAction } from '@persistence/store/common'

/**
 * Implementation of ChangelogsData that saves the changelogs into a Redux store.
 */
export default (dispatch: Dispatch<ChangelogsAction>): ChangelogsData => ({
    saveChangelogs: (changelogs: ChangelogList) => dispatch(createSaveChangelogsAction(changelogs)),
    saveError: (error: Error) => dispatch(createSaveErrorAction(error)),
})
