export default interface SavegameFetcher {
    /**
     * Retrieves the content of the default savegame if it exists as a string.
     */
    getDefault: () => Promise<string>
}
