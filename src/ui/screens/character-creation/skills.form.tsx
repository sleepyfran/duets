import React, { FunctionComponent } from 'react'
import SelectInput from '@ui/components/inputs/select.input'
import { stringToInstrument } from '@core/utils/mappers'
import { Form } from '@ui/hooks/form.hooks'
import { Instrument } from '@engine/entities/instrument'
import Info from '@ui/components/info/info'
import SkillsTable from '@ui/components/tables/skills.table'
import { useInputChange } from '@ui/hooks/input.hooks'

export type SkillsFormInput = {
    instrument: Instrument
}

type FormProps = {
    form: Form
    input: SkillsFormInput
    instruments: ReadonlyArray<Instrument>
    pointsLeft: number
    onUpdate: (input: SkillsFormInput) => void
}

const SkillsForm: FunctionComponent<FormProps> = (props: FormProps) => {
    const instrumentsSelect = props.instruments.map(instrument => ({
        label: instrument.name,
        value: instrument.name,
    }))

    const { instrument: onUpdateInstrument } = useInputChange({
        input: props.input,
        onChange: props.onUpdate,
    })

    const { bind: bindInstrument } = props.form.withInput({
        id: 'instrument',
        map: value => stringToInstrument(value, props.instruments),
        onChange: onUpdateInstrument,
        initial: props.input.instrument,
    })

    return (
        <>
            <h1>My instrument and skills</h1>
            <div className="instrument">
                <SelectInput label="Initial instrument" options={instrumentsSelect} {...bindInstrument} />
            </div>
            <Info text={`You can assign ${props.pointsLeft} more points to these skills`} />
            <SkillsTable assignedPoints={0} />
        </>
    )
}

export default SkillsForm
