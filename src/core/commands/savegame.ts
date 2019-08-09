import { flatten, map, TaskEither, fromEither } from 'fp-ts/lib/TaskEither'
import { Game } from '@core/entities/game'
import { pipe } from 'fp-ts/lib/pipeable'
import SavegameFetcher from '@core/interfaces/savegames/savegame.fetcher'
import SavegameParser from '@core/interfaces/savegames/savegame.parser'

export interface SaveGameCommands {
    attemptLoad: TaskEither<Error, Game>
}

export default (savegameFetcher: SavegameFetcher, savegameParser: SavegameParser): SaveGameCommands => ({
    attemptLoad: pipe(
        savegameFetcher.getDefault,
        map(savegameString => savegameParser.parse(savegameString)),
        map(fromEither),
        flatten,
    ),
})
