import Store from '@storage/store'
import ElectronSavegame from '@infrastructure/electron.savegame'
import createValidateStartDate from '@core/commands/forms/creation/validate-start-date'
import createGameCommand from '@core/commands/forms/creation/create-game'
import createBandCommand from '@core/commands/forms/creation/create-band'
import createSkillUpdate from '@core/commands/forms/creation/skill-update'

export default {
    creation: {
        validateStartDate: createValidateStartDate(),
        createGame: createGameCommand(Store, ElectronSavegame),
        createBand: createBandCommand(Store, ElectronSavegame),
        skillUpdate: createSkillUpdate(),
    },
}
