import React, { FunctionComponent } from 'react'
import './button.scss'

export enum ButtonType {
    Normal,
    Warn,
}

type ButtonProps = {
    className?: string
    buttonType?: ButtonType
    onClick: () => void
}

const button: FunctionComponent<ButtonProps> = props => {
    const buttonClass = props.buttonType === ButtonType.Warn ? 'warn' : 'normal'

    return (
        <div className={`button ${buttonClass} ${props.className}`} onClick={props.onClick}>
            {props.children}
        </div>
    )
}

export default button
