module Duets.Types

open Entities

/// Defines all the possible screens that can be navigated from each screen with
/// their dependencies.
type Screen =
    | MainMenu
    | CharacterCreator
    | BandCreator of Character
    | Game

/// Interface to be implemented in a class that has access to all the scenes.
/// Defines a method that takes a screen defined above and instantiates the
/// correct scene and transitions to it.
type INavigator =
    /// Navigates to the given screen.
    abstract member Navigate : Screen -> unit
    
    /// Navigates to the given screen applying a fade-out transition in between.
    abstract member NavigateWithTransition : Screen -> unit
