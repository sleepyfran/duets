import React, { FunctionComponent } from 'react'
import '@ui/styles/button.scss'

export enum ButtonType {
    normal,
    warn,
    male,
    female,
}

export enum ButtonStyle {
    square,
    transparent,
    circular,
    circularBorderless,
}

export enum ButtonSize {
    regular,
    big,
    bigger,
    small,
    smaller,
}

export type BaseButtonProps = {
    onClick: () => void
    className?: string
    selected?: boolean
}

type ButtonProps = {
    type?: ButtonType
    style?: ButtonStyle
    size?: ButtonSize
} & BaseButtonProps

const buttonTypeMap = new Map([
    [ButtonType.normal, 'normal'],
    [ButtonType.warn, 'warn'],
    [ButtonType.male, 'male'],
    [ButtonType.female, 'female'],
])

const buttonStyleMap = new Map([
    [ButtonStyle.square, 'square'],
    [ButtonStyle.transparent, 'transparent'],
    [ButtonStyle.circular, 'circular'],
    [ButtonStyle.circularBorderless, 'circular-borderless'],
])

const buttonSizeMap = new Map([
    [ButtonSize.regular, 'regular'],
    [ButtonSize.big, 'big'],
    [ButtonSize.bigger, 'bigger'],
    [ButtonSize.small, 'small'],
    [ButtonSize.smaller, 'smaller'],
])

const button: FunctionComponent<ButtonProps> = props => {
    const buttonClass = props.className || ''
    const buttonColor = buttonTypeMap.get(props.type || ButtonType.normal)
    const buttonStyle = buttonStyleMap.get(props.style || ButtonStyle.square)
    const buttonSize = buttonSizeMap.get(props.size || ButtonSize.regular)
    const selectedClass = props.selected ? 'selected' : ''

    return props.style === ButtonStyle.circular || props.style === ButtonStyle.circularBorderless ? (
        <div className={`button ${buttonClass} ${buttonStyle} ${buttonSize}`} onClick={props.onClick}>
            <div className={`circle ${buttonColor} ${selectedClass}`}>{props.children}</div>
        </div>
    ) : (
        <div
            className={`button ${buttonClass} ${buttonColor} ${buttonStyle} ${buttonSize} ${selectedClass}`}
            onClick={props.onClick}
        >
            {props.children}
        </div>
    )
}

export default button
