import WindowInteractor from '@core/interfaces/window'

export type ExitCommand = () => void

/**
 * Creates a command that allows the user to exit the game.
 * @param windowInteractor WindowInteractor dependency.
 */
export default (windowInteractor: WindowInteractor): ExitCommand => () => windowInteractor.exit()
