import { IO } from 'fp-ts/lib/IO'

export default interface WindowInteractor {
    exit: IO<void>
    openInBrowser(url: string): IO<void>
}
