import { Dispatch } from 'redux'
import { pipe } from 'fp-ts/lib/pipeable'
import { of } from 'fp-ts/lib/IO'
import { GameData } from '@core/interfaces/gameplay/game.data'
import { createSaveGameAction, GameActions } from './game.actions'
import { Game } from '@core/entities/game'
import Store from '@persistence/store/store'

export default (dispatch: Dispatch<GameActions>): GameData => ({
    getGame: () => of(Store.getState().gameplay),

    saveGame: (game: Game) =>
        pipe(
            game,
            createSaveGameAction,
            action => of(dispatch(action)),
        ),
})
