import React, { FunctionComponent, useState } from 'react'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/TaskEither'
import { of } from 'fp-ts/lib/Task'
import Button from '@ui/components/buttons/button'
import { useActions } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import '@ui/styles/dialogs/database-download-progress.dialog.scss'
import { useMountEffect } from '@ui/hooks/mount.hooks'

enum StatusType {
    done,
    error,
    downloading,
}

type StatusComponentProps = {
    status: StatusType
}

const Status: FunctionComponent<StatusComponentProps> = props => {
    const { hideDialog } = useDialog()

    switch (props.status) {
        case StatusType.downloading:
            return <h3>Updating...</h3>
        case StatusType.error:
            return (
                <div className="finished">
                    <h3>There was an error downloading the database. Try again later.</h3>
                    <Button onClick={hideDialog}>Exit</Button>
                </div>
            )
        case StatusType.done:
            return (
                <div className="finished">
                    <h3>Updated successfully</h3>
                    <Button onClick={hideDialog}>Done</Button>
                </div>
            )
    }
}

const DatabaseDownloadProgressDialog: FunctionComponent = () => {
    const [status, setStatus] = useState(StatusType.downloading)

    const { fetchCacheAndSaveDatabase } = useActions().init
    useMountEffect(() => {
        pipe(
            fetchCacheAndSaveDatabase,
            fold(() => of(setStatus(StatusType.error)), () => of(setStatus(StatusType.done))),
        )()
    })

    return (
        <div className="database-download-progress-dialog">
            <Status status={status} />
        </div>
    )
}

export default DatabaseDownloadProgressDialog
