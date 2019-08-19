import { pipe } from 'fp-ts/lib/pipeable'
import { IO, of } from 'fp-ts/lib/IO'
import { Dispatch } from 'redux'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import { createSaveCitiesAction } from '@persistence/store/database/cities/cities.actions'
import { Database } from '@core/entities/database'
import { createSaveInstrumentsAction } from '@persistence/store/database/instruments/instruments.actions'
import { DatabaseActions } from '@persistence/store/database/database.actions'

export default (dispatch: Dispatch<DatabaseActions>): InMemoryDatabase => ({
    save(database: Database): IO<Database> {
        return () =>
            pipe(
                database,
                createSaveCitiesAction,
                dispatch,
                of(database),
                createSaveInstrumentsAction,
                dispatch,
                of(database),
            )
    },
})
