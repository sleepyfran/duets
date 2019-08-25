import { SkillsState } from './skills.state'
import { SkillsActions } from './skills.actions'

const initialState: SkillsState = []

export default (state: SkillsState = initialState, action: SkillsActions) =>
    action.type === 'saveSkillsAction' ? action.skills : state
