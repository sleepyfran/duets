import { pipe } from 'fp-ts/lib/pipeable'
import { IO, of } from 'fp-ts/lib/IO'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import { CitiesActions, createSaveCitiesAction } from '@persistence/store/database/cities/cities.actions'
import { Dispatch } from 'redux'
import { Database } from '@core/entities/database'

export default (dispatch: Dispatch<CitiesActions>): InMemoryDatabase => ({
    save(database: Database): IO<Database> {
        return () =>
            pipe(
                database,
                createSaveCitiesAction,
                dispatch,
                of(database),
            )
    },
})
