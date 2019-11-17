import createValidateStartDate from '@core/commands/forms/creation/validate-start-date'
import createGameCommand from '@core/commands/forms/creation/create-game'

export default {
    creation: {
        validateStartDate: createValidateStartDate(),
        createGame: createGameCommand(),
    },
}
