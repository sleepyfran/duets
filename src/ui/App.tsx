import React, { FunctionComponent } from 'react'
import './app.scss'
import Routes from '@ui/screens/screens'
import { Route } from 'react-router-dom'

const App: FunctionComponent = () => {
    return (
        <div className="game">
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
