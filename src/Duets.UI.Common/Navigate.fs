namespace Duets.UI.Common

/// Describes where a scene wants to navigate after completing.
/// Hosts (CLI, GUI) map these to their own scene index types.
[<RequireQualifiedAccess>]
type Navigate =
    | MainMenu
    | NewGame
    | InGame
    | Exit
