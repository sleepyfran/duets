import React, { FunctionComponent } from 'react'
import { Route } from 'react-router-dom'
import Routes from '@ui/screens/screens'
import DialogOverlay from '@ui/dialogs/dialog.overlay'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import { useMountEffect } from '@ui/hooks/mount.hooks'
import { DialogType } from '@persistence/store/ui/ui.state'
import './app.scss'

const App: FunctionComponent = () => {
    const { showDialog } = useDialog()
    const { startup: startupCommand } = useCommands()
    useMountEffect(() => {
        startupCommand()
            .then(() => console.log('Database downloaded'))
            .catch(() => showDialog(DialogType.DatabaseDownloadPrompt))
    })

    return (
        <div className="game">
            <DialogOverlay />
            {Routes.map((route, index) => (
                <Route
                    className="content"
                    key={index}
                    path={route.path}
                    exact={route.exact}
                    component={route.component}
                />
            ))}
        </div>
    )
}

export default App
