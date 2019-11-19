import createLoadSavegameCommand from '@core/commands/savegame/load'
import createCheckSavegameCommand from '@core/commands/savegame/check'
import ElectronSavegame from '@infrastructure/electron.savegame'

const loadSavegameCommand = createLoadSavegameCommand(ElectronSavegame)
const checkSavegameCommand = createCheckSavegameCommand()

export default {
    load: loadSavegameCommand,
    check: checkSavegameCommand,
}
