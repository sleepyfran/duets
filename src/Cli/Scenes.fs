module Cli.SceneIndex

open Agents
open Entities

/// Defines whether we should save before exiting the game or not.
[<RequireQualifiedAccess>]
type ExitMode =
    | SaveGame
    | SkipSave

/// Defines the index of all scenes available in the game that can be instantiated.
[<RequireQualifiedAccess>]
type Scene =
    | MainMenu of Savegame.SavegameState
    | CharacterCreator
    // Band creator needs the playable character  that was created in the
    // previous step.
    | BandCreator of Character
    /// World creator needs the playable character and the band created in
    /// the previous steps.
    | WorldCreator of Character * Band
    /// Shows the world and allows the character to move around and interact
    /// with different objects.
    | World
    /// Shows the world scene after character's movement same as before, but
    /// displaying details about the current place.
    | WorldAfterMovement
    | Phone
    // Saves the game and exits.
    | Exit of ExitMode
