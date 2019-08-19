import { createStore, combineReducers } from 'redux'
import ChangelogsReducer from '@persistence/store/changelogs/changelogs.reducer'
import InstrumentsReducer from '@persistence/store/database/instruments/instruments.reducer'
import CitiesReducer from '@persistence/store/database/cities/cities.reducer'
import UiReducer from '@persistence/store/ui/ui.reducer'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'
import { DatabaseState } from '@persistence/store/database/database.state'
import { UiState } from '@persistence/store/ui/ui.state'

const rootReducer = combineReducers({
    changelogs: ChangelogsReducer,
    database: combineReducers({
        cities: CitiesReducer,
        instruments: InstrumentsReducer,
    }),
    ui: UiReducer,
})

export type State = {
    changelogs: ChangelogsState
    database: DatabaseState
    ui: UiState
}

export default createStore(
    rootReducer,
    {},
    // Needed by the Redux DevTools to detect the game's store.
    (window as any).devToolsExtension ? (window as any).devToolsExtension() : (f: any) => f,
)
