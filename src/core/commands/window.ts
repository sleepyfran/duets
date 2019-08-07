import { IO } from 'fp-ts/lib/IO'
import WindowInteractor from '@core/interfaces/window/window'

export interface WindowCommands {
    exit: IO<void>
}

export default (windowInteractor: WindowInteractor): WindowCommands => ({
    exit: () => windowInteractor.exit(),
})
