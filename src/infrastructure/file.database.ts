import { pipe } from 'fp-ts/lib/pipeable'
import { flatten, fromEither, map, tryCatch } from 'fp-ts/lib/TaskEither'
import { duetsDataPath, readFile } from '@infrastructure/electron.files'
import { tryParseJson } from '@infrastructure/json.utils'
import { City } from '@engine/entities/city'
import CachedDatabase from '@core/interfaces/database/cached.database'

const fileDatabase: CachedDatabase = {
    getCities: pipe(
        duetsDataPath,
        duetsDataPath => `${duetsDataPath}/cities.duets`,
        readFile,
        map(tryParseJson),
        map(fromEither),
        flatten,
        map(json => json as ReadonlyArray<City>),
    ),

    // TODO: Implement.
    saveCities: (cities: ReadonlyArray<City>) =>
        tryCatch(() => new Promise(resolve => resolve(cities)), error => new Error(String(error))),
}

export default fileDatabase
