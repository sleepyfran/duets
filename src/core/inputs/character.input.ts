import { Option } from 'fp-ts/lib/Option'
import { Gender } from '@engine/entities/gender'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'

export type CharacterInput = {
    name: string
    birthday: Option<Date>
    gender: Option<Gender>
    originCity: Option<City>
    instrument: Option<Instrument>
}
