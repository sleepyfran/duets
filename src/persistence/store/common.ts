/**
 * General action for saving an error into a certain state.
 */
export type SaveErrorAction = {
    type: 'saveErrorAction'
    error: Error
}

export const createSaveErrorAction = (error: Error): SaveErrorAction => ({
    type: 'saveErrorAction',
    error,
})

/**
 * Defines when a fetch action is still loading.
 */
export type Loading = 'loading'

/**
 * Defines when a fetch action has not been asked to load yet.
 */
export type NotAsked = 'notAsked'
