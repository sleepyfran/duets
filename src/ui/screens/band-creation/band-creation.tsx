import React, { FunctionComponent } from 'react'
import HorizontalLayout, { LayoutMode } from '@ui/components/layout/horizontal-layout'
import FullSizeSidebar from '@ui/components/sidebars/full-size.sidebar'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import { useHistory } from 'react-router'
import NewBandForm from '@ui/screens/band-creation/new-band.form'
import JoinExistingForm from '@ui/screens/band-creation/join-existing.form'
import '@ui/styles/screens/band-creation.scss'

const BandCreation: FunctionComponent = () => {
    const history = useHistory()

    return (
        <HorizontalLayout
            className="band-creation"
            mode={LayoutMode.Half}
            left={
                <FullSizeSidebar
                    className="main-menu"
                    navButton={NavButton.Back}
                    onNavButtonClick={history.goBack}
                    header={<NewBandForm />}
                />
            }
            right={<JoinExistingForm />}
        />
    )
}

export default BandCreation
