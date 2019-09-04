import React, { ChangeEvent, FunctionComponent } from 'react'
import { useSelector } from 'react-redux'
import { pipe } from 'fp-ts/lib/pipeable'
import { groupBy } from 'fp-ts/lib/NonEmptyArray'
import { State } from '@persistence/store/store'
import { Skill } from '@engine/entities/skill'
import { useActions } from '@ui/hooks/injections.hooks'
import './table.scss'
import { toArray } from 'fp-ts/lib/Record'

type SkillsTableProps = {
    assignedPoints: number
}

const SkillsTable: FunctionComponent<SkillsTableProps> = props => {
    const characterSkills = useSelector((state: State) => state.gameplay.character.skills)
    const skills = useSelector((state: State) => state.database.skills)
    const skillsByType = toArray(groupBy((skill: Skill) => skill.type)([...skills]))

    const getCharacterSkill = (skill: Skill) =>
        characterSkills.find(s => s.name === skill.name) || { ...skill, level: 0 }
    const getCharacterSkillLevel = (skill: Skill) => getCharacterSkill(skill).level

    const { modifySkillLevel } = useActions().gameplay.skills
    const handleSkillLevelChange = (skill: Skill, event: ChangeEvent<HTMLInputElement>) => {
        const newLevel = Number.parseInt(event.target.value) || 0
        pipe(modifySkillLevel(getCharacterSkill(skill), newLevel, props.assignedPoints))()
    }

    return (
        <table>
            <thead>
                <tr>
                    <th>Type</th>
                    <th>Skill</th>
                    <th>Points</th>
                </tr>
            </thead>
            <tbody>
                {skillsByType.map(([type, skills]) =>
                    skills.map((skill, index) => (
                        <tr key={skill.name}>
                            {index === 0 ? <th rowSpan={skills.length}>{type}</th> : <></>}
                            <td>{skill.name}</td>
                            <td>
                                <input
                                    type="number"
                                    value={getCharacterSkillLevel(skill)}
                                    onChange={event => handleSkillLevelChange(skill, event)}
                                />
                            </td>
                        </tr>
                    )),
                )}
            </tbody>
        </table>
    )
}

export default SkillsTable
