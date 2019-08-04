import React from 'react'
import ReactDOM from 'react-dom'
import { HashRouter } from 'react-router-dom'
import './index.scss'
import App from './app'
import { GameInfo } from '@ui/types/game-info'
import { GameInfoContext } from '@ui/contexts/game-info.context'

/**
 * Renders the app with the game information.
 */
export default (appInfo: GameInfo) => {
    return ReactDOM.render(
        <GameInfoContext.Provider value={appInfo}>
            <HashRouter>
                <App />
            </HashRouter>
        </GameInfoContext.Provider>,
        document.getElementById('root'),
    )
}
