import { SkillsActions } from '@core/actions/skills'
import { ChangelogsActions } from './changelogs'
import { CreationActions } from './creation'

export type Actions = {
    changelogs: ChangelogsActions
    gameplay: {
        skills: SkillsActions
    }
    creation: CreationActions
}
