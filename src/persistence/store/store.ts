import { createStore, combineReducers } from 'redux'
import ChangelogsReducer from '@persistence/store/changelogs/changelogs.reducer'
import CitiesReducer from '@persistence/store/database/cities/cities.reducer'
import UiReducer from '@persistence/store/ui/ui.reducer'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'
import { DatabaseState } from '@persistence/store/database/database.state'
import { UiState } from '@persistence/store/ui/ui.state'

const rootReducer = combineReducers({
    changelogs: ChangelogsReducer,
    cities: CitiesReducer,
    ui: UiReducer,
})

export type State = {
    changelogs: ChangelogsState
    database: DatabaseState
    ui: UiState
}

export default createStore(rootReducer)
