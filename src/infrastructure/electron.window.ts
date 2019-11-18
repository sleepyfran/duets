import { remote, shell } from 'electron'
import WindowInteractor from '@core/interfaces/window'

const window: WindowInteractor = {
    exit: () => remote.getCurrentWindow().close(),
    openInBrowser: (url: string) => shell.openExternal(url),
}

export default window
