import { ReactComponent as BackIcon } from '@ui/assets/icons/back.svg'
import { ButtonType } from '@ui/components/buttons/button'
import { createNavButton } from './nav.button'

export default createNavButton(BackIcon, ButtonType.normal, false)
