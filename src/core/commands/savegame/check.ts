import { Game } from '@core/entities/game'

export enum SavegameState {
    Ready,
    CharacterMissing,
    BandMissing,
}

export type CheckSavegameCommand = (game: Game) => SavegameState

/**
 * Checks the state in which the game is in. Returns whether it's ready to be played or some necessary part is
 * missing.
 */
export default (): CheckSavegameCommand => game => {
    if (!game.character) return SavegameState.CharacterMissing
    if (!game.band) return SavegameState.BandMissing
    return SavegameState.Ready
}
