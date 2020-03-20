import { ReactComponent as MenuIcon } from '@ui/assets/icons/menu.svg'
import { createNavButton } from './nav.button'
import { ButtonType } from '../button'

export default createNavButton(MenuIcon, ButtonType.normal, false)
