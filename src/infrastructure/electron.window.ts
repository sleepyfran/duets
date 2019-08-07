import WindowInteractor from '@core/interfaces/window/window'
import { remote } from 'electron'

const window: WindowInteractor = {
    exit: () => remote.getCurrentWindow().close(),
}

export default window
