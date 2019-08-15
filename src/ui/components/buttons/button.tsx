import React, { FunctionComponent } from 'react'
import './button.scss'

export enum ButtonType {
    normal,
    warn,
}

type ButtonProps = {
    buttonType?: ButtonType
    onClick: () => void
}

const button: FunctionComponent<ButtonProps> = props => {
    const buttonClass = props.buttonType === ButtonType.warn ? 'warn' : 'normal'

    return (
        <div className={`button ${buttonClass}`} onClick={props.onClick}>
            {props.children}
        </div>
    )
}

export default button
