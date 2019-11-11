import { duetsCachedDatabasePath, readFile, writeFile } from '@infrastructure/electron.files'
import { tryParseJson } from '@infrastructure/json.utils'
import CachedDatabase from '@core/interfaces/database/cached.database'
import { Database } from '@core/entities/database'

const fileDatabase: CachedDatabase = {
    get: () =>
        readFile(duetsCachedDatabasePath())
            .then(tryParseJson)
            .then(json => json as Database),

    save: database => {
        const content = JSON.stringify(database)

        return writeFile(duetsCachedDatabasePath(), content).then(() => database)
    },
}

export default fileDatabase
