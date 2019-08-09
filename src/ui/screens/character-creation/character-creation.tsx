import React, { FunctionComponent } from 'react'
import useRouter from 'use-react-router'
import Layout from '@ui/components/layout/layout'
import './character-creation.scss'
import BackButton from '@ui/components/buttons/nav/back/back.button'
import FullSizeSidebar from '@ui/components/sidebars/full-size-sidebar/full-size.sidebar'

const CharacterCreation: FunctionComponent = () => {
    const { history } = useRouter()

    return (
        <Layout>
            <FullSizeSidebar className="main-menu">
                <header>
                    <BackButton className="exit-button" onClick={history.goBack} />
                    <h2>Coming soon...</h2>
                </header>
            </FullSizeSidebar>
        </Layout>
    )
}

export default CharacterCreation
