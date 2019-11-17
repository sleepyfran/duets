import React, { FunctionComponent, useState } from 'react'
import Button from '@ui/components/buttons/button'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import '@ui/styles/dialogs/database-download-progress.dialog.scss'
import { useMountEffect } from '@ui/hooks/mount.hooks'
import { RequestStatus } from '@ui/types/request-status'

type StatusComponentProps = {
    status: RequestStatus
}

const Status: FunctionComponent<StatusComponentProps> = props => {
    const { hideDialog } = useDialog()

    switch (props.status) {
        case RequestStatus.downloading:
            return <h3>Updating...</h3>
        case RequestStatus.error:
            return (
                <div className="finished">
                    <h3>There was an error downloading the database. Try again later.</h3>
                    <Button onClick={hideDialog}>Exit</Button>
                </div>
            )
        case RequestStatus.done:
            return (
                <div className="finished">
                    <h3>Updated successfully</h3>
                    <Button onClick={hideDialog}>Done</Button>
                </div>
            )
    }
}

const DatabaseDownloadProgressDialog: FunctionComponent = () => {
    const [status, setStatus] = useState(RequestStatus.downloading)

    const { downloadDatabase } = useCommands().init
    useMountEffect(() => {
        downloadDatabase()
            .then(() => setStatus(RequestStatus.done))
            .catch(() => setStatus(RequestStatus.error))
    })

    return (
        <div className="database-download-progress-dialog">
            <Status status={status} />
        </div>
    )
}

export default DatabaseDownloadProgressDialog
