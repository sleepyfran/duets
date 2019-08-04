import React, { FunctionComponent, useContext } from 'react'
import FullSizeSidebar from '@ui/components/common/sidebars/full-size-sidebar/full-size.sidebar'
import Layout from '@ui/components/common/layout/layout'
import CloseButton from '@ui/components/common/buttons/close/close.button'
import PlayButton from '@ui/components/common/buttons/play/play.button'
import { GameInfoContext } from '@ui/contexts/game-info.context'
import './start.scss'

const Start: FunctionComponent = () => {
    const gameInfo = useContext(GameInfoContext)

    return (
        <Layout>
            <FullSizeSidebar className="main-menu">
                <div className="top">
                    <CloseButton className="exit-button" />
                    <h1 className="logo">Duets</h1>

                    <div className="saves-buttons">
                        <PlayButton />
                    </div>
                </div>

                <footer>
                    <h3>v{gameInfo.version}</h3>
                    <div>
                        <a className="external-url" href={gameInfo.sourceCodeUrl}>
                            source code
                        </a>
                        <a className="external-url" href={gameInfo.homepageUrl}>
                            homepage
                        </a>
                    </div>
                </footer>
            </FullSizeSidebar>
        </Layout>
    )
}

export default Start
