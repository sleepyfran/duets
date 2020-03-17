import React, { FunctionComponent } from 'react'
import Button, { ButtonType } from '@ui/components/buttons/button'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import { Dialog } from '@core/entities/dialog'
import ConfirmationDialog from '@ui/dialogs/common/confirmation.dialog'

const DatabaseDownloadPromptDialog: FunctionComponent = () => {
    const { exit } = useCommands().window
    const { showDialog } = useDialog()

    const handleDownload = () => showDialog(Dialog.DatabaseDownloadProgress)

    return (
        <ConfirmationDialog
            title="Database download needed"
            content={
                <>
                    <p>
                        Seems like you haven't download a database yet. Databases includes things as the playable
                        cities, artists of the game and other data that is <b>needed to play</b>.
                    </p>
                    <p>Would you like to download the latest database now?</p>
                </>
            }
            choice={
                <>
                    <Button buttonType={ButtonType.Warn} onClick={exit}>
                        No, exit
                    </Button>
                    <Button onClick={handleDownload}>Download</Button>
                </>
            }
        />
    )
}

export default DatabaseDownloadPromptDialog
