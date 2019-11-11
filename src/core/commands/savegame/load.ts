import { Game } from '@core/entities/game'
import SavegameFetcher from '@core/interfaces/savegames/savegame.fetcher'
import SavegameParser from '@core/interfaces/savegames/savegame.parser'

export type LoadSavegameCommand = () => Promise<Game>

/**
 * Creates a command that attempts to load a save game from the disk and return it.
 * @param savegameFetcher SavegameFetcher dependency.
 * @param savegameParser SavegameParser dependency.
 */
export default (savegameFetcher: SavegameFetcher, savegameParser: SavegameParser): LoadSavegameCommand => () =>
    savegameFetcher.getDefault().then(savegameParser.parse)
