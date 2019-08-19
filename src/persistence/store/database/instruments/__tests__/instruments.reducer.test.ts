import InstrumentsReducer from '../instruments.reducer'
import { createSaveInstrumentsAction } from '../instruments.actions'
import { Instrument } from '@engine/entities/instrument'
import { Database } from '@core/entities/database'

const createDatabaseWithInstruments = (instruments: ReadonlyArray<Instrument>) =>
    (({ instruments } as unknown) as Database)

describe('InstrumentsReducer', () => {
    it('should return an empty list when the saveInstrumentsAction with an empty list is given', () => {
        const database = createDatabaseWithInstruments([])
        const result = InstrumentsReducer([], createSaveInstrumentsAction(database))

        expect(result).toHaveLength(0)
    })

    it('should return a given list when the saveInstrumentsAction with such list is given', () => {
        const database = createDatabaseWithInstruments([
            {
                name: 'Test City',
                allowsAnotherInstrument: false,
            },
        ])
        const result = InstrumentsReducer([], createSaveInstrumentsAction(database))

        expect(result).toEqual(database.instruments)
    })
})
