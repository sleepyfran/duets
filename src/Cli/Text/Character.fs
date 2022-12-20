module Cli.Text.Character

open Entities

let attributeName attr =
    match attr with
    | CharacterAttribute.Drunkenness -> "Drunkenness"
    | CharacterAttribute.Energy -> "Energy"
    | CharacterAttribute.Fame -> "Fame"
    | CharacterAttribute.Health -> "Health"
    | CharacterAttribute.Mood -> "Mood"
