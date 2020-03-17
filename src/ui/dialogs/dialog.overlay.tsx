import React, { FunctionComponent } from 'react'
import { Dialog } from '@core/entities/dialog'
import DatabaseDownloadPromptDialog from '@ui/dialogs/database-download/database-download-prompt.dialog'
import DatabaseDownloadProgressDialog from '@ui/dialogs/database-download/database-download-progress.dialog'
import '@ui/styles/dialogs/dialog.overlay.scss'
import StartDateSelectionDialog from '@ui/dialogs/start-date-selection.dialog'
import { useStorage } from '@ui/hooks/storage.hooks'

type DialogProps = {
    type: Dialog
}

const DialogContent: FunctionComponent<DialogProps> = props => {
    switch (props.type) {
        case Dialog.DatabaseDownloadPrompt:
            return <DatabaseDownloadPromptDialog />
        case Dialog.DatabaseDownloadProgress:
            return <DatabaseDownloadProgressDialog />
        case Dialog.StartDateSelection:
            return <StartDateSelectionDialog />
        default:
            return <></>
    }
}

const DialogOverlay: FunctionComponent = () => {
    const [getStore] = useStorage()
    const type = getStore().ui.dialog
    const hideDialog = type === Dialog.Hide

    return hideDialog ? (
        <></>
    ) : (
        <div className="overlay">
            <div className="dialog">
                <DialogContent type={type} />
            </div>
        </div>
    )
}

export default DialogOverlay
