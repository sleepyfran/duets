import React, { FunctionComponent } from 'react'
import Button, { BaseButtonProps, ButtonStyle, ButtonSize, ButtonType } from '@ui/components/buttons/button'

export enum NavButton {
    hide,
    back,
    close,
}

export const createNavButton = (
    Icon: FunctionComponent,
    type: ButtonType = ButtonType.normal,
    showBorder: boolean = true,
): FunctionComponent<BaseButtonProps> => props => {
    const style = showBorder ? ButtonStyle.circular : ButtonStyle.circularBorderless

    return (
        <Button {...props} size={ButtonSize.small} style={style} type={type}>
            <Icon />
        </Button>
    )
}
