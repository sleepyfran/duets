import { Dispatch } from 'redux'
import { pipe } from 'fp-ts/lib/pipeable'
import { Skill } from '@engine/entities/skill'
import { SkillsData } from '@core/interfaces/gameplay/skills.data'
import { createSaveSkillAction, SkillActions } from '@persistence/store/gameplay/skills/skills.actions'
import { of } from 'fp-ts/lib/IO'

export default (dispatch: Dispatch<SkillActions>): SkillsData => ({
    saveSkill: (skill: Skill) =>
        pipe(
            skill,
            createSaveSkillAction,
            action => of(dispatch(action)),
        ),
})
