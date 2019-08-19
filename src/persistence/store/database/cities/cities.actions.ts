import { City } from '@engine/entities/city'
import { Database } from '@core/entities/database'

export type SaveCitiesAction = {
    type: 'saveCitiesAction'
    cities: ReadonlyArray<City>
}

export type CitiesActions = SaveCitiesAction

export const createSaveCitiesAction = (database: Database): SaveCitiesAction => ({
    type: 'saveCitiesAction',
    cities: database.cities,
})
