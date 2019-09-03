import { GameState } from './game.state'
import { GameActions } from './game.actions'
import { TimeOfDay } from '@engine/entities/calendar'
import { Gender } from '@engine/entities/gender'

const initialState: GameState = {
    calendar: {
        date: new Date(),
        time: TimeOfDay.Morning,
    },
    character: {
        name: '',
        birthday: new Date(),
        skills: [],
        fame: 0,
        health: 0,
        mood: 0,
        gender: Gender.Male,
    },
}

export default (state: GameState = initialState, action: GameActions) => {
    return action.type === 'saveSkillAction' ? action.game : state
}
