import { pipe } from 'fp-ts/lib/pipeable'
import { flatten, fromEither, map, tryCatch } from 'fp-ts/lib/TaskEither'
import { duetsDataPath, readFile } from '@infrastructure/electron.files'
import { tryParseJson } from '@infrastructure/json.utils'
import CachedDatabase from '@core/interfaces/database/cached.database'
import { Database } from '@core/entities/database'

const fileDatabase: CachedDatabase = {
    get: pipe(
        duetsDataPath,
        duetsDataPath => `${duetsDataPath}/cities.duets`,
        readFile,
        map(tryParseJson),
        map(fromEither),
        flatten,
        map(json => json as Database),
    ),

    // TODO: Implement.
    save: (database: Database) =>
        tryCatch(() => new Promise(resolve => resolve(database)), error => new Error(String(error))),
}

export default fileDatabase
