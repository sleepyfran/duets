import { Dispatch } from 'redux'
import { pipe } from 'fp-ts/lib/pipeable'
import { SkillsData } from '@core/interfaces/gameplay/skills.data'
import { createSaveSkillAction, SkillActions } from '@persistence/store/gameplay/skills/skills.actions'
import { of } from 'fp-ts/lib/IO'
import { CharacterSkill } from '@engine/entities/character-skill'

export default (dispatch: Dispatch<SkillActions>): SkillsData => ({
    saveSkill: (skill: CharacterSkill) =>
        pipe(
            skill,
            createSaveSkillAction,
            action => of(dispatch(action)),
        ),
})
