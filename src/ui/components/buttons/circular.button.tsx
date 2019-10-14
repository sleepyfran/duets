import React, { FunctionComponent } from 'react'
import '@ui/styles/circular.button.scss'

type CircularButtonProps = {
    className?: string
    circleClassName?: string
    size?: string
    onClick?: () => void
}

const CircularButton: FunctionComponent<CircularButtonProps> = props => {
    const size = props.size || '100'
    const style = {
        height: `${size}px`,
        width: `${size}px`,
    }

    return (
        <div className={`circular-button ${props.className}`} style={style} onClick={props.onClick}>
            <div className={`circle ${props.circleClassName}`}>{props.children}</div>
        </div>
    )
}

export default CircularButton
