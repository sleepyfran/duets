import React, { FunctionComponent } from 'react'
import { Route } from 'react-router-dom'
import Routes from '@ui/screens/screens'
import DialogOverlay from '@ui/dialogs/dialog.overlay'
import './app.scss'

const App: FunctionComponent = () => {
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
