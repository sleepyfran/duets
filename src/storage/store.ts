import { Storage } from '@core/entities/storage'
import { TimeOfDay } from '@engine/entities/calendar'
import { Gender } from '@engine/entities/gender'
import { Dialog } from '@core/entities/dialog'
import { Listener, MemoryStorage } from '@core/interfaces/memory-storage'

const createStore = (state: Storage): MemoryStorage => {
    let listeners: Listener[] = []

    return {
        set: (update: Storage) => {
            state = { ...update }
            listeners.forEach(listener => listener())
        },

        get: () => state,

        subscribe: (listener: Listener) => {
            listeners.push(listener)
        },

        unsubscribe: (listener: Listener) => {
            listeners = listeners.filter(l => l !== listener)
        },
    }
}

export default createStore({
    database: {
        cities: [],
        genres: [],
        instruments: [],
        roles: [],
        skills: [],
    },
    game: {
        band: undefined,
        calendar: {
            date: new Date(),
            time: TimeOfDay.Dawn,
        },
        character: {
            birthday: new Date(),
            fame: 0,
            gender: Gender.Male,
            health: 0,
            mood: 0,
            name: '',
            skills: [],
        },
    },
    ui: {
        changelogList: 'loading',
        dialog: Dialog.Hide,
    },
})
