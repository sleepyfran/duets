import React, { ChangeEvent, FunctionComponent } from 'react'
import { Skill } from '@engine/entities/skill'
import { CharacterSkill } from '@engine/entities/character-skill'
import { groupBy } from '@utils/utils'
import '@ui/styles/table.scss'

type SkillsTableProps = {
    input: ReadonlyArray<CharacterSkill>
    assignedPoints: number
    skills: ReadonlyArray<Skill>
    onUpdate: (skills: CharacterSkill, level: number) => void
}

const SkillsTable: FunctionComponent<SkillsTableProps> = props => {
    const skillsByType = groupBy(props.skills, 'type')

    const getCharacterSkill = (skill: Skill) => props.input.find(s => s.name === skill.name) || { ...skill, level: 0 }
    const getCharacterSkillLevel = (skill: Skill) => getCharacterSkill(skill).level

    const handleSkillLevelChange = (skill: Skill, event: ChangeEvent<HTMLInputElement>) => {
        props.onUpdate(getCharacterSkill(skill), Number(event.target.value))
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
