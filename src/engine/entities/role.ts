import { Instrument } from '@engine/entities/instrument'

export type Role = {
    name: string
    forInstruments: Instrument[]
}
