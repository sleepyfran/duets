module Cli.SceneIndex

open Agents
open Entities

/// Defines the index of all scenes available in the game that can be instantiated.
[<RequireQualifiedAccess>]
type Scene =
    | MainMenu of Savegame.SavegameState
    | CharacterCreator
    // Band creator needs a character the character that was created in the
    // previous step.
    | BandCreator of Character
    | World
    | Phone
    // Saves the game and exits.
    | Exit
