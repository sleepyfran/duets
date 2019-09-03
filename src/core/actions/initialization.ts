import { chain, flatten, map, rightIO, TaskEither } from 'fp-ts/lib/TaskEither'
import { pipe } from 'fp-ts/lib/pipeable'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import CachedDatabase from '@core/interfaces/database/cached.database'
import { Database } from '@core/entities/database'

export interface InitializationActions {
    fetchCacheAndSaveDatabase: TaskEither<Error, Database>
    loadDatabaseFromCache: TaskEither<Error, Database>
}

export default (
    remoteDatabase: RemoteDatabase,
    cachedDatabase: CachedDatabase,
    inMemoryDatabase: InMemoryDatabase,
): InitializationActions => ({
    fetchCacheAndSaveDatabase: pipe(
        remoteDatabase.get,
        map(cachedDatabase.save),
        flatten,
        chain(database => rightIO(inMemoryDatabase.save(database))),
    ),

    loadDatabaseFromCache: pipe(
        cachedDatabase.get,
        chain(database => rightIO(inMemoryDatabase.save(database))),
    ),
})
