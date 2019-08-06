/**
 * Defines a changelog of a version of the game.
 */
export type Changelog = {
    version: string
    releaseDate: Date
    body: string
}

/**
 * Defines a list of the previously defined changelog.
 */
export type ChangelogList = Changelog[]
