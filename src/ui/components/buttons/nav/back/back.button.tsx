import React, { FunctionComponent } from 'react'
import CircularButton from '@ui/components/buttons/circular/circular.button'
import { ReactComponent as BackIcon } from '@ui/assets/icons/back.svg'
import './back.button.scss'

type BackButtonProps = {
    className?: string
    onClick: () => void
}

const BackButton: FunctionComponent<BackButtonProps> = props => {
    return (
        <CircularButton
            className={`back-button ${props.className}`}
            circleClassName="back-button-circle"
            size="35"
            onClick={props.onClick}
        >
            <BackIcon />
        </CircularButton>
    )
}

export default BackButton
