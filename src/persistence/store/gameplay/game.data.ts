import { Dispatch } from 'redux'
import { InMemoryGameData } from '@core/interfaces/gameplay/in-memory-game-data'
import { createSaveGameAction, GameActions } from './game.actions'
import Store from '@persistence/store/store'

export default (dispatch: Dispatch<GameActions>): InMemoryGameData => ({
    get: () => Store.getState().gameplay,
    save: game => dispatch(createSaveGameAction(game)),
})
