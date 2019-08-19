import { tryCatch } from 'fp-ts/lib/TaskEither'
import RemoteDatabase from '@core/interfaces/database/remote.database'

const githubDatabase: RemoteDatabase = {
    // TODO: Implement.
    get: tryCatch(
        () =>
            new Promise(resolve =>
                setTimeout(
                    () =>
                        resolve({
                            cities: [
                                { name: 'Madrid', population: 3600000, country: { name: 'Spain', flagEmoji: '🇪🇸' } },
                            ],
                            instruments: [
                                {
                                    name: 'Guitar',
                                    allowsAnotherInstrument: false,
                                },
                                {
                                    name: 'Vocals',
                                    allowsAnotherInstrument: true,
                                },
                            ],
                        }),
                    3000,
                ),
            ),
        error => new Error(String(error)),
    ),
}

export default githubDatabase
