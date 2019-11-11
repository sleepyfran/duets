import { Dispatch } from 'redux'
import InMemoryDatabase from '@core/interfaces/database/inmemory.database'
import { createSaveCitiesAction } from '@persistence/store/database/cities/cities.actions'
import { createSaveInstrumentsAction } from '@persistence/store/database/instruments/instruments.actions'
import { DatabaseActions } from '@persistence/store/database/database.actions'
import { createSaveSkillsAction } from '@persistence/store/database/skills/skills.actions'

export default (dispatch: Dispatch<DatabaseActions>): InMemoryDatabase => ({
    save(database) {
        dispatch(createSaveCitiesAction(database))
        dispatch(createSaveInstrumentsAction(database))
        dispatch(createSaveSkillsAction(database))
        return database
    },
})
