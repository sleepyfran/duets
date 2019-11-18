import { Dispatch } from 'redux'
import { GameData } from '@core/interfaces/gameplay/game.data'
import { createSaveGameAction, GameActions } from './game.actions'
import Store from '@persistence/store/store'

export default (dispatch: Dispatch<GameActions>): GameData => ({
    getGame: () => Store.getState().gameplay,
    saveGame: game => dispatch(createSaveGameAction(game)),
})
