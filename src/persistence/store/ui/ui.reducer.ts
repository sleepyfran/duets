import { DialogType, UiState } from './ui.state'
import { UiActions } from './ui.actions'

const initialState: UiState = {
    dialog: DialogType.Hide,
}

export default (state: UiState = initialState, action: UiActions) => {
    switch (action.type) {
        case 'hideDialog':
            return {
                ...state,
                dialog: DialogType.Hide,
            }
        case 'showDialog':
            return {
                ...state,
                dialog: action.dialog,
            }
        default:
            return state
    }
}
