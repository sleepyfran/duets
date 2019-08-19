import { pipe } from 'fp-ts/lib/pipeable'
import { flatten, fromEither, map } from 'fp-ts/lib/TaskEither'
import { duetsCachedDatabasePath, readFile, writeFile } from '@infrastructure/electron.files'
import { tryParseJson } from '@infrastructure/json.utils'
import CachedDatabase from '@core/interfaces/database/cached.database'
import { Database } from '@core/entities/database'

const fileDatabase: CachedDatabase = {
    get: pipe(
        duetsCachedDatabasePath(),
        readFile,
        map(tryParseJson),
        map(fromEither),
        flatten,
        map(json => json as Database),
    ),

    save: (database: Database) =>
        pipe(
            JSON.stringify(database),
            content => writeFile(duetsCachedDatabasePath(), content),
            map(() => database),
        ),
}

export default fileDatabase
