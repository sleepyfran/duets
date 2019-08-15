import React, { FunctionComponent } from 'react'
import Button, { ButtonType } from '@ui/components/buttons/button'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import './database-download.dialog.scss'

const DatabaseDownloadDialog: FunctionComponent = () => {
    const { exit } = useCommands().window
    const { hideDialog } = useDialog()

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
                <Button onClick={hideDialog}>Download</Button>
            </div>
        </div>
    )
}

export default DatabaseDownloadDialog
