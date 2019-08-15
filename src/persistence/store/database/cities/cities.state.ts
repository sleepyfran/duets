import { City } from '@engine/entities/city'
import { Loading } from '@persistence/store/common'

export type CitiesState = Loading | ReadonlyArray<City>
