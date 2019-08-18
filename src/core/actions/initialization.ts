import { chain, flatten, map, rightIO, TaskEither } from 'fp-ts/lib/TaskEither'
import { pipe } from 'fp-ts/lib/pipeable'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import CachedDatabase from '@core/interfaces/database/cached.database'
import { City } from '@engine/entities/city'

export interface InitializationActions {
    fetchCacheAndSaveCities: TaskEither<Error, ReadonlyArray<City>>
    loadFromCacheAndSaveCities: TaskEither<Error, ReadonlyArray<City>>
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
        chain(cities => rightIO(inMemoryDatabase.saveCities(cities))),
    ),

    loadFromCacheAndSaveCities: pipe(
        cachedDatabase.getCities,
        chain(cities => rightIO(inMemoryDatabase.saveCities(cities))),
    ),
})
