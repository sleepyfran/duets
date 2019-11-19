import FormCommands from './form-commands'
import WindowCommands from './window-commands'
import InitCommands from './init-commands'
import SavegameCommands from './savegame-commands'
import { Commands } from '@core/commands/commands'

const commands: Commands = {
    forms: FormCommands,
    init: InitCommands,
    savegame: SavegameCommands,
    window: WindowCommands,
}

export default commands
