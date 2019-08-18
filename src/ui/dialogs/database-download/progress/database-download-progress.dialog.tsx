import React, { FunctionComponent, useState } from 'react'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/TaskEither'
import { of } from 'fp-ts/lib/Task'
import Button from '@ui/components/buttons/button'
import { useActions } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import './database-download-progress.dialog.scss'
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
            return <h5>Updating...</h5>
        case StatusType.error:
            return (
                <>
                    <h5>There was an error downloading the database. Try again later.</h5>
                    <Button onClick={hideDialog}>Exit</Button>
                </>
            )
        case StatusType.done:
            return (
                <>
                    <h5>Updated successfully</h5>
                    <Button onClick={hideDialog}>Done</Button>
                </>
            )
    }
}

const DatabaseDownloadProgressDialog: FunctionComponent = () => {
    const [status, setStatus] = useState(StatusType.downloading)

    const { fetchCacheAndSaveCities } = useActions().init
    useMountEffect(() => {
        pipe(
            fetchCacheAndSaveCities,
            fold(() => of(setStatus(StatusType.error)), () => of(setStatus(StatusType.done))),
        )()
    })

    return (
        <div className="database-download-progress-dialog">
            <h1>Updating database</h1>
            <Status status={status} />
        </div>
    )
}

export default DatabaseDownloadProgressDialog
