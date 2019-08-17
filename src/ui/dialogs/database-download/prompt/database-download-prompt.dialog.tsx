import React, { FunctionComponent } from 'react'
import Button, { ButtonType } from '@ui/components/buttons/button'
import { useActions } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import { DialogType } from '@persistence/store/ui/ui.state'
import './database-download-prompt.dialog.scss'

const DatabaseDownloadPromptDialog: FunctionComponent = () => {
    const { exit } = useActions().window
    const { showDialog } = useDialog()

    const handleDownload = () => showDialog(DialogType.databaseDownloadProgress)

    return (
        <div className="database-download-dialog">
            <h1>Database download needed</h1>
            <p>
                Seems like you haven't download a database yet. Databases includes things as the playable cities,
                artists of the game and other data that is <b>needed to play</b>.
            </p>
            <p>Would you like to download the latest database now?</p>
            <div className="choice">
                <Button buttonType={ButtonType.warn} onClick={exit}>
                    No, exit
                </Button>
                <Button onClick={handleDownload}>Download</Button>
            </div>
        </div>
    )
}

export default DatabaseDownloadPromptDialog
