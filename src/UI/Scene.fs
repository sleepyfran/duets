module UI.SceneIndex

open Entities

/// Defines the index of all scenes available in the game that can be instantiated.
[<RequireQualifiedAccess>]
type Scene =
    | MainMenu
    | NewGame
    // Band creator needs a character the character that was created in the
    // previous step.
    | BandCreator of Character
    | InGame
