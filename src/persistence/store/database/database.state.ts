import { CitiesState } from '@persistence/store/database/cities/cities.state'

/**
 * Contains all the information retrieved from an external source and cannot be changed. Things like cities, game artists,
 * modifiers and any other static data will go here.
 */
export type DatabaseState = {
    cities: CitiesState
}
