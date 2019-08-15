import { pipe } from 'fp-ts/lib/pipeable'
import { IO, of } from 'fp-ts/lib/IO'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import { City } from '@engine/entities/city'
import { CitiesActions, createSaveCitiesAction } from '@persistence/store/database/cities/cities.actions'
import { Dispatch } from 'redux'

export default (dispatch: Dispatch<CitiesActions>): InMemoryDatabase => ({
    saveCities(cities: ReadonlyArray<City>): IO<ReadonlyArray<City>> {
        return () =>
            pipe(
                cities,
                createSaveCitiesAction,
                dispatch,
                of(cities),
            )
    },
})
