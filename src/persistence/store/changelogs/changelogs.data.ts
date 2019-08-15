import { IO } from 'fp-ts/lib/IO'
import { pipe } from 'fp-ts/lib/pipeable'
import { Dispatch } from 'redux'
import ChangelogsData from '@core/interfaces/changelogs/changelogs.data'
import { ChangelogList } from '@core/entities/changelog'
import { ChangelogsAction, createSaveChangelogsAction } from './changelogs.actions'
import { createSaveErrorAction } from '@persistence/store/common'

/**
 * Implementation of ChangelogsData that saves the changelogs into a Redux store.
 */
export default (dispatch: Dispatch<ChangelogsAction>): ChangelogsData => ({
    saveChangelogs: (changelogs: ChangelogList): IO<void> => {
        return () =>
            pipe(
                changelogs,
                createSaveChangelogsAction,
                dispatch,
            )
    },

    saveError: (error: Error): IO<void> => {
        return () =>
            pipe(
                error,
                createSaveErrorAction,
                dispatch,
            )
    },
})
