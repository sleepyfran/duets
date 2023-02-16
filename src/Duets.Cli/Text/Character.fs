module Duets.Cli.Text.Character

open Duets.Entities

let attributeName attr =
    match attr with
    | CharacterAttribute.Drunkenness -> "Drunkenness"
    | CharacterAttribute.Energy -> "Energy"
    | CharacterAttribute.Fame -> "Fame"
    | CharacterAttribute.Health -> "Health"
    | CharacterAttribute.Hunger -> "Hunger"
    | CharacterAttribute.Mood -> "Mood"
