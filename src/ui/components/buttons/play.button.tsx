import React, { FunctionComponent } from 'react'
import Button, { ButtonSize, ButtonStyle, BaseButtonProps } from './button'
import { ReactComponent as PlayIcon } from '@ui/assets/icons/play.svg'

const PlayButton: FunctionComponent<BaseButtonProps> = props => {
    return (
        <Button {...props} size={ButtonSize.bigger} style={ButtonStyle.circular}>
            <PlayIcon />
        </Button>
    )
}

export default PlayButton
