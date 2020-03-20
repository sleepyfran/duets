import { ReactComponent as CalendarIcon } from '@ui/assets/icons/calendar.svg'
import { createNavButton } from './nav.button'
import { ButtonType } from '../button'

export default createNavButton(CalendarIcon, ButtonType.normal, false)
