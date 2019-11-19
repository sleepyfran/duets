import { createStore, combineReducers } from 'redux'
import ChangelogsReducer from '@persistence/store/changelogs/changelogs.reducer'
import GameReducer from '@persistence/store/gameplay/game.reducer'
import UiReducer from '@persistence/store/ui/ui.reducer'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'
import { DatabaseState } from '@persistence/store/database/database.state'
import { UiState } from '@persistence/store/ui/ui.state'
import { GameState } from '@persistence/store/gameplay/game.state'
import { createReducerFor } from '@persistence/store/generator'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'
import { Skill } from '@engine/entities/skill'
import { Genre } from '@engine/entities/genre'
import { Role } from '@engine/entities/role'

const rootReducer = combineReducers({
    changelogs: ChangelogsReducer,
    database: combineReducers({
        cities: createReducerFor<ReadonlyArray<City>>('cities', []),
        instruments: createReducerFor<ReadonlyArray<Instrument>>('instruments', []),
        skills: createReducerFor<ReadonlyArray<Skill>>('skills', []),
        genres: createReducerFor<ReadonlyArray<Genre>>('genres', []),
        roles: createReducerFor<ReadonlyArray<Role>>('roles', []),
    }),
    gameplay: GameReducer,
    ui: UiReducer,
})

export type State = {
    changelogs: ChangelogsState
    database: DatabaseState
    gameplay: GameState
    ui: UiState
}

export default createStore(
    rootReducer,
    {},
    // Needed by the Redux DevTools to detect the game's store.
    (window as any).__REDUX_DEVTOOLS_EXTENSION__ ? (window as any).__REDUX_DEVTOOLS_EXTENSION__() : (f: any) => f,
)
