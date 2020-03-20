import { ReactComponent as CloseIcon } from '@ui/assets/icons/close.svg'
import { createNavButton } from './nav.button'
import { ButtonType } from '../button'

export default createNavButton(CloseIcon, ButtonType.warn, false)
