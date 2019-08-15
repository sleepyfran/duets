import { CitiesState } from '@persistence/store/database/cities/cities.state'
import { CitiesActions } from '@persistence/store/database/cities/cities.actions'

const initialState: CitiesState = 'loading'

export default (state: CitiesState = initialState, action: CitiesActions) =>
    action.type === 'saveCitiesAction' ? action.cities : state
