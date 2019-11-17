import { SkillsActions } from '@core/actions/skills'
import { CreationActions } from './creation'

export type Actions = {
    gameplay: {
        skills: SkillsActions
    }
    creation: CreationActions
}
