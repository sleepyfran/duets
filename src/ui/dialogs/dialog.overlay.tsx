import React, { FunctionComponent } from 'react'
import { useSelector } from 'react-redux'
import { State } from '@persistence/store/store'
import { DialogType } from '@persistence/store/ui/ui.state'
import DatabaseDownloadPromptDialog from '@ui/dialogs/database-download/database-download-prompt.dialog'
import DatabaseDownloadProgressDialog from '@ui/dialogs/database-download/database-download-progress.dialog'
import '@ui/styles/dialogs/dialog.overlay.scss'
import StartDateSelectionDialog from '@ui/dialogs/start-date-selection.dialog'

type DialogProps = {
    type: DialogType
}

const Dialog: FunctionComponent<DialogProps> = props => {
    switch (props.type) {
        case DialogType.DatabaseDownloadPrompt:
            return <DatabaseDownloadPromptDialog />
        case DialogType.DatabaseDownloadProgress:
            return <DatabaseDownloadProgressDialog />
        case DialogType.StartDateSelection:
            return <StartDateSelectionDialog />
        default:
            return <></>
    }
}

const DialogOverlay: FunctionComponent = () => {
    const type = useSelector((state: State) => state.ui.dialog)
    const hideDialog = type === DialogType.Hide

    return hideDialog ? (
        <></>
    ) : (
        <div className="overlay">
            <div className="dialog">
                <Dialog type={type} />
            </div>
        </div>
    )
}

export default DialogOverlay
