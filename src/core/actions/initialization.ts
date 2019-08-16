import { flatten, map, TaskEither } from 'fp-ts/lib/TaskEither'
import { IO } from 'fp-ts/lib/IO'
import { pipe } from 'fp-ts/lib/pipeable'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import CachedDatabase from '@core/interfaces/database/cached.database'

export interface InitializationActions {
    fetchCacheAndSaveCities: TaskEither<Error, IO<void>>
    loadFromCacheAndSaveCities: TaskEither<Error, IO<void>>
}

export default (
    remoteDatabase: RemoteDatabase,
    cachedDatabase: CachedDatabase,
    inMemoryDatabase: InMemoryDatabase,
): InitializationActions => ({
    fetchCacheAndSaveCities: pipe(
        remoteDatabase.getCities,
        map(cachedDatabase.saveCities),
        flatten,
        map(inMemoryDatabase.saveCities),
    ),

    loadFromCacheAndSaveCities: pipe(
        cachedDatabase.getCities,
        map(inMemoryDatabase.saveCities),
    ),
})
