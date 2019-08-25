import { SkillsState } from './skills.state'
import { SkillActions } from './skills.actions'

const initialState: SkillsState = []

export default (state: SkillsState = initialState, action: SkillActions) => {
    switch (action.type) {
        case 'saveSkillAction':
            const skill = action.skill
            const existingSkill = state.find(s => s.name === skill.name)

            return [...(existingSkill ? state.map(s => (s.name === skill.name ? skill : s)) : [...state, skill])]
        default:
            return state
    }
}
