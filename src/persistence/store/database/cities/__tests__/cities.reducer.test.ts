import CitiesReducer from '../cities.reducer'
import { createSaveCitiesAction } from '../cities.actions'
import { City } from '@engine/entities/city'
import { Database } from '@core/entities/database'

const createDatabaseWithCities = (cities: ReadonlyArray<City>) => (({ cities } as unknown) as Database)

describe('CitiesReducer', () => {
    it('should return an empty list when the saveCitiesAction with an empty list is given', () => {
        const database = createDatabaseWithCities([])
        const result = CitiesReducer([], createSaveCitiesAction(database))

        expect(result).toHaveLength(0)
    })

    it('should return a given list when the saveCitiesAction with such list is given', () => {
        const database = createDatabaseWithCities([
            {
                name: 'Test City',
                population: 100,
                country: {
                    name: 'Test Country',
                    flagEmoji: '😎',
                },
            },
        ])
        const result = CitiesReducer([], createSaveCitiesAction(database))

        expect(result).toEqual(database.cities)
    })
})
