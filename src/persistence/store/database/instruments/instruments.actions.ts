import { Database } from '@core/entities/database'
import { Instrument } from '@engine/entities/instrument'

export type SaveInstrumentsAction = {
    type: 'saveInstrumentsAction'
    instruments: ReadonlyArray<Instrument>
}

export type InstrumentsActions = SaveInstrumentsAction

export const createSaveInstrumentsAction = (database: Database): SaveInstrumentsAction => ({
    type: 'saveInstrumentsAction',
    instruments: database.instruments,
})
