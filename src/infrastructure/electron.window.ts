import { of } from 'fp-ts/lib/IO'
import { remote, shell } from 'electron'
import WindowInteractor from '@core/interfaces/window/window'

const window: WindowInteractor = {
    exit: () => remote.getCurrentWindow().close(),
    openInBrowser: (url: string) => of(shell.openExternalSync(url)),
}

export default window
