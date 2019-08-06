import { createStore, combineReducers } from 'redux'
import ChangelogsReducer from '@persistence/store/changelogs/changelogs.reducer'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'

const rootReducer = combineReducers({
    changelogs: ChangelogsReducer,
})

export type State = {
    changelogs: ChangelogsState
}

export default createStore(rootReducer)
