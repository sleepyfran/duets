import { Dispatch } from 'redux'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import { createActionFor, GenericAction } from '@persistence/store/generator'

export default (dispatch: Dispatch<GenericAction<any>>): InMemoryDatabase => ({
    save(database) {
        dispatch(createActionFor('skills', database.skills))
        dispatch(createActionFor('instruments', database.instruments))
        dispatch(createActionFor('cities', database.cities))
        dispatch(createActionFor('genres', database.genres))
        dispatch(createActionFor('roles', database.roles))
        return database
    },
})
