import { Gender } from '@engine/entities/gender'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'

export type CreationInput = {
    name: string
    birthday: Date
    gender: Gender
    originCity: City
    instrument: Instrument
    gameStartDate: Date
}
