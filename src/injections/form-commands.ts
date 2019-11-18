import Store from '@persistence/store/store'
import StoreGameData from '@persistence/store/gameplay/game.data'
import ElectronSavegame from '@infrastructure/electron.savegame'
import createValidateStartDate from '@core/commands/forms/creation/validate-start-date'
import createGameCommand from '@core/commands/forms/creation/create-game'
import createSkillUpdate from '@core/commands/forms/creation/skill-update'

export default {
    creation: {
        validateStartDate: createValidateStartDate(),
        createGame: createGameCommand(StoreGameData(Store.dispatch), ElectronSavegame),
        skillUpdate: createSkillUpdate(),
    },
}
