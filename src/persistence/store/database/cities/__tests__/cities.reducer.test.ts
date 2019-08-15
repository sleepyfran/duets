import CitiesReducer from '../cities.reducer'
import { createSaveCitiesAction } from '../cities.actions'
import { City } from '@engine/entities/city'

describe('CitiesReducer', () => {
    it('should return an empty list when the saveCitiesAction with an empty list is given', () => {
        const result = CitiesReducer('loading', createSaveCitiesAction([]))

        expect(result).toHaveLength(0)
    })

    it('should return a given list when the saveCitiesAction with such list is given', () => {
        const list: ReadonlyArray<City> = [
            {
                name: 'Test City',
                population: 100,
                country: {
                    name: 'Test Country',
                    flagEmoji: '😎',
                },
            },
        ]
        const result = CitiesReducer('loading', createSaveCitiesAction(list))

        expect(result).toEqual(list)
    })
})
