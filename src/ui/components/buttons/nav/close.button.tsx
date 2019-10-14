import React, { FunctionComponent } from 'react'
import '@ui/styles/close.button.scss'
import CircularButton from '@ui/components/buttons/circular.button'
import { ReactComponent as CloseIcon } from '@ui/assets/icons/close.svg'

type CloseButtonProps = {
    className?: string
    onClick: () => void
}

const CloseButton: FunctionComponent<CloseButtonProps> = props => {
    return (
        <CircularButton
            className={`close-button ${props.className}`}
            circleClassName="close-button-circle"
            size="35"
            onClick={props.onClick}
        >
            <CloseIcon />
        </CircularButton>
    )
}

export default CloseButton
