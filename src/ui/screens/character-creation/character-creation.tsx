import React, { FunctionComponent } from 'react'
import useRouter from 'use-react-router'
import Layout from '@ui/components/layout/layout'
import './character-creation.scss'
import FullSizeSidebar from '@ui/components/sidebars/full-size-sidebar/full-size.sidebar'
import TextInput from '@ui/components/inputs/text/text.input'
import { NavButton } from '@ui/components/buttons/nav/navButton'

const CharacterCreation: FunctionComponent = () => {
    const { history } = useRouter()

    return (
        <Layout>
            <FullSizeSidebar
                className="main-menu"
                header={
                    <div>
                        <h1>Character creation</h1>
                        <TextInput label="Name" />
                    </div>
                }
                navButton={NavButton.back}
                onNavButtonClick={history.goBack}
            />
        </Layout>
    )
}

export default CharacterCreation
