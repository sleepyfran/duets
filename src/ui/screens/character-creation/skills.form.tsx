import React, { FunctionComponent } from 'react'
import SelectInput from '@ui/components/inputs/select.input'
import { stringToInstrument } from '@core/utils/mappers'
import { Form } from '@ui/hooks/form.hooks'
import { Instrument } from '@engine/entities/instrument'
import Info from '@ui/components/info/info'
import SkillsTable from '@ui/components/tables/skills.table'
import { useInputChange } from '@ui/hooks/input.hooks'
import { Skill } from '@engine/entities/skill'
import { CharacterSkill } from '@engine/entities/character-skill'
import { createOrUpdate } from '@utils/utils'

export type SkillsFormInput = {
    instrument: Instrument
    pointsLeft: number
    characterSkills: ReadonlyArray<CharacterSkill>
}

type FormProps = {
    form: Form
    input: SkillsFormInput
    instruments: ReadonlyArray<Instrument>
    skills: ReadonlyArray<Skill>
    onUpdate: (input: SkillsFormInput) => void
}

const SkillsForm: FunctionComponent<FormProps> = (props: FormProps) => {
    const instrumentsSelect = props.instruments.map(instrument => ({
        label: instrument.name,
        value: instrument.name,
    }))

    const { instrument: onUpdateInstrument, characterSkills: onUpdateCharacterSkills } = useInputChange({
        input: props.input,
        onChange: props.onUpdate,
    })

    const { bind: bindInstrument } = props.form.withInput({
        id: 'instrument',
        map: value => stringToInstrument(value, props.instruments),
        onChange: onUpdateInstrument,
        initial: props.input.instrument,
    })

    const onUpdateCharacterSkill = (characterSkill: CharacterSkill) => {
        onUpdateCharacterSkills(createOrUpdate(props.input.characterSkills, characterSkill, 'name'))
    }

    return (
        <>
            <h1>My instrument and skills</h1>
            <div className="instrument">
                <SelectInput label="Initial instrument" options={instrumentsSelect} {...bindInstrument} />
            </div>
            <Info text={`You can assign ${props.input.pointsLeft} more points to these skills`} />
            <SkillsTable
                input={props.input.characterSkills}
                assignedPoints={0}
                skills={props.skills}
                onUpdate={onUpdateCharacterSkill}
            />
        </>
    )
}

export default SkillsForm
