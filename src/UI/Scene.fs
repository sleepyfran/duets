module UI.SceneIndex


/// Defines the index of all scenes available in the game that can be instantiated.
[<RequireQualifiedAccess>]
type Scene =
    | MainMenu
    | NewGame
    | InGame
