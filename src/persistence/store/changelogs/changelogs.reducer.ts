import { ChangelogsState } from './changelogs.state'
import { ChangelogsAction } from './changelogs.actions'

const initialState: ChangelogsState = 'loading'

export default (state: ChangelogsState = initialState, action: ChangelogsAction) => {
    switch (action.type) {
        case 'saveChangelogsAction':
            return action.changelogs
        case 'saveErrorAction':
            return action.error
        default:
            return state
    }
}
