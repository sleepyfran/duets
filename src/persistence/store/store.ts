import { createStore, combineReducers } from 'redux'
import ChangelogsReducer from '@persistence/store/changelogs/changelogs.reducer'
import InstrumentsReducer from '@persistence/store/database/instruments/instruments.reducer'
import CitiesReducer from '@persistence/store/database/cities/cities.reducer'
import DatabaseSkillsReducer from '@persistence/store/database/skills/skills.reducer'
import GameplaySkillsReducer from '@persistence/store/gameplay/skills/skills.reducer'
import UiReducer from '@persistence/store/ui/ui.reducer'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'
import { DatabaseState } from '@persistence/store/database/database.state'
import { UiState } from '@persistence/store/ui/ui.state'
import { SkillsState } from '@persistence/store/gameplay/skills/skills.state'

const rootReducer = combineReducers({
    changelogs: ChangelogsReducer,
    database: combineReducers({
        cities: CitiesReducer,
        instruments: InstrumentsReducer,
        skills: DatabaseSkillsReducer,
    }),
    gameplay: combineReducers({
        skills: GameplaySkillsReducer,
    }),
    ui: UiReducer,
})

export type State = {
    changelogs: ChangelogsState
    database: DatabaseState
    gameplay: {
        skills: SkillsState
    }
    ui: UiState
}

export default createStore(
    rootReducer,
    {},
    // Needed by the Redux DevTools to detect the game's store.
    (window as any).__REDUX_DEVTOOLS_EXTENSION__ ? (window as any).__REDUX_DEVTOOLS_EXTENSION__() : (f: any) => f,
)
