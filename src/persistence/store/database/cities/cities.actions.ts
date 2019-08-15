import { City } from '@engine/entities/city'

export type SaveCitiesAction = {
    type: 'saveCitiesAction'
    cities: ReadonlyArray<City>
}

export type CitiesActions = SaveCitiesAction

export const createSaveCitiesAction = (cities: ReadonlyArray<City>): SaveCitiesAction => ({
    type: 'saveCitiesAction',
    cities,
})
