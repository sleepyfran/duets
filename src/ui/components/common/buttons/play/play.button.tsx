import React, { FunctionComponent } from 'react'
import './play.button.scss'
import CircularButton from '@ui/components/common/buttons/circular/circular.button'
import { ReactComponent as PlayIcon } from '@ui/assets/icons/play.svg'

type PlayButtonProps = {
    className?: string
}

const PlayButton: FunctionComponent<PlayButtonProps> = props => {
    return (
        <CircularButton className={`play-button ${props.className}`} circleClassName="play-button-circle" size="100">
            <PlayIcon />
        </CircularButton>
    )
}

export default PlayButton
