import { Gender } from '@engine/entities/gender'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'
import { parseDateOrDefault } from '@utils/utils'
import { Role } from '@engine/entities/role'
import { Genre } from '@engine/entities/genre'

/**
 * Identity mapper for a string.
 * @param value Value to return.
 */
export const stringToString = (value: string) => value

/**
 * Attempts to parse a date from a given string. Returns a default date if the input is not valid.
 * @param value Value to parse.
 */
export const stringToDate = (value: string): Date => parseDateOrDefault(value)

/**
 * Attempts to parse a Gender from a given string. Defaults to Male if the gender is not recognized.
 * @param value Value to parse.
 */
export const stringToGender = (value: string): Gender => {
    switch (value) {
        case 'male':
            return Gender.Male
        case 'female':
            return Gender.Female
        default:
            return Gender.Male
    }
}

/**
 * Attempts to parse a city from a given string. Defaults to the first element of the cities list if it's not recognized.
 * @param value Value to parse.
 * @param cities List of cities available.
 */
export const stringToCity = (value: string, cities: ReadonlyArray<City>) =>
    cities.find(city => city.name === value) || cities[0]

/**
 * Attempts to parse an instrument from a given string. Defaults to the first element of the instruments list if it's
 * not recognized.
 * @param value Value to parse.
 * @param instruments List of instruments available.
 */
export const stringToInstrument = (value: string, instruments: ReadonlyArray<Instrument>) =>
    instruments.find(instrument => instrument.name === value) || instruments[0]

/**
 * Attempts to parse a genre from a given string. Defaults to the first element of the genres list if it's not
 * recognized.
 * @param value Value to parse.
 * @param genres List of genres available.
 */
export const stringToGenre = (value: string, genres: ReadonlyArray<Genre>) =>
    genres.find(genre => genre.name === value) || genres[0]

/**
 * Attempts to parse a role from a given string. Defaults to the first element of the roles list if it's not
 * recognized.
 * @param value Value to parse.
 * @param roles List of roles available.
 */
export const stringToRole = (value: string, roles: ReadonlyArray<Role>) =>
    roles.find(role => role.name === value) || roles[0]
