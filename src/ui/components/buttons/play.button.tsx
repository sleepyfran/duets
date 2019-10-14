import React, { FunctionComponent } from 'react'
import CircularButton from '@ui/components/buttons/circular.button'
import { ReactComponent as PlayIcon } from '@ui/assets/icons/play.svg'
import '@ui/styles/play.button.scss'

type PlayButtonProps = {
    className?: string
    onClick: () => void
}

const PlayButton: FunctionComponent<PlayButtonProps> = props => {
    return (
        <CircularButton
            className={`play-button ${props.className}`}
            circleClassName="play-button-circle"
            size="100"
            onClick={props.onClick}
        >
            <PlayIcon />
        </CircularButton>
    )
}

export default PlayButton
