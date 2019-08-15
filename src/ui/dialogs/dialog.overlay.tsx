import React, { FunctionComponent } from 'react'
import { useSelector } from 'react-redux'
import DatabaseDownloadDialog from '@ui/dialogs/database-download/database-download.dialog'
import { State } from '@persistence/store/store'
import { DialogType } from '@persistence/store/ui/ui.state'
import './dialog.overlay.scss'

const Dialog: FunctionComponent = () => {
    const type = useSelector((state: State) => state.ui.dialog)

    const hideDialog = type === DialogType.hide
    const dialogContent = type === DialogType.databaseDownload ? <DatabaseDownloadDialog /> : <></>

    return hideDialog ? (
        <></>
    ) : (
        <div className="overlay">
            <div className="dialog">{dialogContent}</div>
        </div>
    )
}

export default Dialog
