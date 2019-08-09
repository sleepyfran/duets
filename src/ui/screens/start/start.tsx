import React, { FunctionComponent, useContext, useEffect } from 'react'
import useReactRouter from 'use-react-router'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/TaskEither'
import { of } from 'fp-ts/lib/Task'
import { useSelector } from 'react-redux'
import FullSizeSidebar from '@ui/components/sidebars/full-size-sidebar/full-size.sidebar'
import Layout from '@ui/components/layout/layout'
import CloseButton from '@ui/components/buttons/nav/close/close.button'
import PlayButton from '@ui/components/buttons/play/play.button'
import Changelog from '@ui/components/changelog/changelog'
import { GameInfoContext } from '@ui/contexts/game-info.context'
import { useCommands, useQueries } from '@ui/hooks/injections.hooks'
import { State } from '@persistence/store/store'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'
import { CharacterCreationScreen } from '@ui/screens/screens'
import './start.scss'

const Start: FunctionComponent = () => {
    const { history } = useReactRouter()

    const gameInfo = useContext(GameInfoContext)

    const { fetchAndSave: fetchAndSaveChangelogs } = useQueries().changelogs
    const changelogs = useSelector<State, ChangelogsState>(state => state.changelogs)
    useEffect(() => {
        fetchAndSaveChangelogs()
    })

    const { attemptLoad } = useCommands().savegames
    const { exit, openInBrowser } = useCommands().window

    const attemptLoadPreviousSavegame = () => {
        pipe(
            attemptLoad,
            fold(error => of(history.push(CharacterCreationScreen.path)), content => of(alert('Coming soon.'))),
        )()
    }

    return (
        <Layout>
            <FullSizeSidebar className="main-menu">
                <header>
                    <CloseButton className="exit-button" onClick={exit} />
                    <h1 className="logo">Duets</h1>

                    <div className="saves-buttons">
                        <PlayButton onClick={attemptLoadPreviousSavegame} />
                    </div>
                </header>

                <footer>
                    <h3>v{gameInfo.version}</h3>
                    <div>
                        <a className="external-url" onClick={() => openInBrowser(gameInfo.sourceCodeUrl)}>
                            source code
                        </a>
                        <a className="external-url" onClick={() => openInBrowser(gameInfo.homepageUrl)}>
                            homepage
                        </a>
                    </div>
                </footer>
            </FullSizeSidebar>

            <div className="changelog">
                {changelogs === 'loading' ? (
                    'Loading...'
                ) : changelogs instanceof Error ? (
                    'There was an error loading the changelog'
                ) : (
                    <Changelog changelogList={changelogs} />
                )}
            </div>
        </Layout>
    )
}

export default Start
