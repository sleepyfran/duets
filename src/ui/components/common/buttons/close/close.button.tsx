import React, { FunctionComponent } from 'react'
import './close.button.scss'
import CircularButton from '@ui/components/common/buttons/circular/circular.button'
import { ReactComponent as CloseIcon } from '@ui/assets/icons/close.svg'

type CloseButtonProps = {
    className?: string
}

const CloseButton: FunctionComponent<CloseButtonProps> = props => {
    return (
        <CircularButton className={`close-button ${props.className}`} circleClassName="close-button-circle" size="35">
            <CloseIcon />
        </CircularButton>
    )
}

export default CloseButton
