module Duets.Cli.SceneIndex

open Duets.Entities

/// Defines whether we should save before exiting the game or not.
[<RequireQualifiedAccess>]
type ExitMode =
    | SaveGame
    | SkipSave

/// Defines the index of all scenes available in the game that can be instantiated.
[<RequireQualifiedAccess>]
type Scene =
    | MainMenu
    | Settings
    | Cheats
    | CharacterCreator
    /// World creator needs the playable character from the previous step.
    | WorldSelector of Character
    // Band creator needs the playable character that was created in the
    // previous step and the selected origin city.
    | BandCreator of Character * City
    /// Skill creator needs the playable character and the band created in
    /// the previous steps.
    | SkillEditor of Character * CurrentMember * Band * City
    /// Shows the world and allows the character to move around and interact
    /// with different objects.
    | World
    /// Shows the world scene after character's movement same as before, but
    /// displaying details about the current place.
    | WorldAfterMovement
    | Phone
    // Saves the game and exits.
    | Exit of ExitMode
