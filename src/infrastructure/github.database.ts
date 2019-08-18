import { tryCatch } from 'fp-ts/lib/TaskEither'
import RemoteDatabase from '@core/interfaces/database/remote.database'

const githubDatabase: RemoteDatabase = {
    // TODO: Implement.
    getCities: tryCatch(
        () =>
            new Promise(resolve =>
                setTimeout(
                    () =>
                        resolve([{ name: 'Madrid', population: 3600000, country: { name: 'Spain', flagEmoji: '🇪🇸' } }]),
                    3000,
                ),
            ),
        error => new Error(String(error)),
    ),
}

export default githubDatabase
