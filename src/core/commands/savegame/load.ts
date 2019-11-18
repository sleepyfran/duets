import { Game } from '@core/entities/game'
import Savegame from '@core/interfaces/savegame'

export type LoadSavegameCommand = () => Promise<Game>

/**
 * Creates a command that attempts to load a save game from the disk and return it.
 * @param savegame Savegame dependency.
 */
export default (savegame: Savegame): LoadSavegameCommand => () => savegame.getDefault().then(savegame.parse)
