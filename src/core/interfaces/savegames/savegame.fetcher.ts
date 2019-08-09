import { TaskEither } from 'fp-ts/lib/TaskEither'

export default interface SavegameFetcher {
    /**
     * Retrieves the content of the default savegame if it exists as a string.
     */
    getDefault: TaskEither<Error, string>
}
