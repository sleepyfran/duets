import ElectronWindow from '@infrastructure/electron.window'
import createExitCommand from '@core/commands/window/exit'
import createOpenBrowserCommand from '@core/commands/window/open-browser-command'

const exitCommand = createExitCommand(ElectronWindow)
const openBrowserCommand = createOpenBrowserCommand(ElectronWindow)

export default {
    exit: exitCommand,
    openBrowser: openBrowserCommand,
}
