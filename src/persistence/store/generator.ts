import { Reducer } from 'redux'
import { Key } from '@persistence/store/keys'

export type GenericAction<State> = {
    type: string
    payload: State
}

/**
 * Creates a generic reducer that allows saving operations for a specific type.
 * @param key Key that uniquely identifies the state.
 * @param initialState Initial state of the type.
 */
export const createReducerFor = <State>(key: Key, initialState: State): Reducer<State, GenericAction<State>> => (
    state: State = initialState,
    action: GenericAction<State>,
): State => {
    return action.type === `save_${key}` ? action.payload : state
}

/**
 * Creates a generic action to save an specific type.
 * @param key Key that uniquely identifies the state.
 * @param payload Payload of the action.
 */
export const createActionFor = <State>(key: Key, payload: State): GenericAction<State> => ({
    type: `save_${key}`,
    payload,
})
