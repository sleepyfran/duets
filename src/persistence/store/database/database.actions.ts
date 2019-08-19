import { CitiesActions } from '@persistence/store/database/cities/cities.actions'
import { InstrumentsActions } from '@persistence/store/database/instruments/instruments.actions'

export type DatabaseActions = CitiesActions | InstrumentsActions
