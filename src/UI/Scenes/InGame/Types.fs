module UI.Scenes.InGame.Types

open Avalonia.FuncUI.Types
open Entities

/// Defines an action that happens after an interaction with a component in the
/// UI.
type InGameAction =
    /// Returns a list of inline elements that should be added to a text block.
    | Message of IView list
    /// Returns the list of views to display.
    | Subcomponent of IView list
    /// Returns the list of effects to apply.
    | Effects of Effect list
    /// Returns nothing to be done.
    | Nothing
