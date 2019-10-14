import React, { FunctionComponent } from 'react'
import { Route } from 'react-router-dom'
import Routes from '@ui/screens/screens'
import DialogOverlay from '@ui/dialogs/dialog.overlay'
import './app.scss'
import { useActions } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import { useMountEffect } from '@ui/hooks/mount.hooks'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/TaskEither'
import { of } from 'fp-ts/lib/Task'
import { DialogType } from '@persistence/store/ui/ui.state'

const App: FunctionComponent = () => {
    const { loadDatabaseFromCache } = useActions().init
    const { showDialog } = useDialog()
    useMountEffect(() => {
        pipe(
            loadDatabaseFromCache,
            fold(() => of(showDialog(DialogType.DatabaseDownloadPrompt)), () => of(console.log('Database loaded'))),
        )()
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
