import React, { FunctionComponent } from 'react'
import CircularButton from '@ui/components/buttons/circular.button'
import { ReactComponent as MenuIcon } from '@ui/assets/icons/menu.svg'
import '@ui/styles/nav.button.scss'

type MenuButtonProps = {
    className?: string
    onClick: () => void
}

const MenuButton: FunctionComponent<MenuButtonProps> = props => {
    return (
        <CircularButton
            className={`nav-button ${props.className}`}
            circleClassName="nav-button-circle"
            size="35"
            onClick={props.onClick}
        >
            <MenuIcon />
        </CircularButton>
    )
}

export default MenuButton
