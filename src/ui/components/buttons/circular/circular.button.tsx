import React, { FunctionComponent } from 'react'
import './circular.button.scss'

type CircularButtonProps = {
    className?: string
    circleClassName?: string
    size?: string
}

const CircularButton: FunctionComponent<CircularButtonProps> = props => {
    const size = props.size || '100'
    const style = {
        height: `${size}px`,
        width: `${size}px`,
    }

    return (
        <div className={`circular-button ${props.className}`} style={style}>
            <div className={`circle ${props.circleClassName}`}>{props.children}</div>
        </div>
    )
}

export default CircularButton
