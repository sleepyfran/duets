import FormCommands from './form-commands'
import WindowCommands from './window-commands'
import InitCommands from './init-commands'
import { Commands } from '@core/commands/commands'

const commands: Commands = {
    forms: {
        ...FormCommands,
    },
    init: {
        ...InitCommands,
    },
    window: WindowCommands,
}

export default commands
