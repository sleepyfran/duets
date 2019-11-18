import { Game } from '@core/entities/game'

export default interface Savegame {
    /**
     * Retrieves the content of the default savegame if it exists as a string.
     */
    getDefault: () => Promise<string>

    /**
     * Parses a JSON string into a game.
     * @param savegame String to parse.
     */
    parse: (savegame: string) => Promise<Game>

    /**
     * Transforms the game object into a string and saves it in the savegame file.
     * @param game Game to save.
     */
    save: (game: Game) => Promise<void>
}
