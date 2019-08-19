import { InstrumentsState } from '@persistence/store/database/instruments/instruments.state'
import { InstrumentsActions } from '@persistence/store/database/instruments/instruments.actions'

const initialState: InstrumentsState = []

export default (state: InstrumentsState = initialState, action: InstrumentsActions) =>
    action.type === 'saveInstrumentsAction' ? action.instruments : state
