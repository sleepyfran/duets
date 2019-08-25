import { tryCatch } from 'fp-ts/lib/TaskEither'
import RemoteDatabase from '@core/interfaces/database/remote.database'
import { SkillType } from '@engine/entities/skill'

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
                            skills: [
                                {
                                    name: 'Composition',
                                    type: SkillType.music,
                                },
                                {
                                    name: 'Lyrics writing',
                                    type: SkillType.music,
                                },
                                {
                                    name: 'Improvisation',
                                    type: SkillType.music,
                                },
                                {
                                    name: 'Genre',
                                    type: SkillType.music,
                                },
                                {
                                    name: 'Instrument',
                                    type: SkillType.music,
                                },
                                {
                                    name: 'Recording',
                                    type: SkillType.production,
                                },
                                {
                                    name: 'Mixing',
                                    type: SkillType.production,
                                },
                                {
                                    name: 'Mastering',
                                    type: SkillType.production,
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
