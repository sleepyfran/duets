import RemoteDatabase from '@core/interfaces/database/remote.database'
import { SkillType } from '@engine/entities/skill'

const githubDatabase: RemoteDatabase = {
    // TODO: Implement.
    get: () =>
        new Promise(resolve =>
            setTimeout(
                () =>
                    resolve({
                        cities: [{ name: 'Madrid', population: 3600000, country: { name: 'Spain', flagEmoji: '🇪🇸' } }],
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
                                type: SkillType.Music,
                            },
                            {
                                name: 'Lyrics writing',
                                type: SkillType.Music,
                            },
                            {
                                name: 'Improvisation',
                                type: SkillType.Music,
                            },
                            {
                                name: 'Genre',
                                type: SkillType.Music,
                            },
                            {
                                name: 'Instrument',
                                type: SkillType.Music,
                            },
                            {
                                name: 'Recording',
                                type: SkillType.Production,
                            },
                            {
                                name: 'Mixing',
                                type: SkillType.Production,
                            },
                            {
                                name: 'Mastering',
                                type: SkillType.Production,
                            },
                        ],
                        genres: [
                            {
                                name: 'Avant-Garde',
                                compatibleWith: [],
                            },
                            {
                                name: 'Blackgaze',
                                compatibleWith: [],
                            },
                        ],
                        roles: [
                            {
                                name: 'Guitarist',
                                forInstruments: [{ name: 'Guitar', allowsAnotherInstrument: false }],
                            },
                        ],
                    }),
                3000,
            ),
        ),
}

export default githubDatabase
