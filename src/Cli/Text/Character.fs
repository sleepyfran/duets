module Cli.Text.Character

open Entities

let attributeName attr =
    match attr with
    | CharacterAttribute.Bladder -> "Bladder"
    | CharacterAttribute.Drunkenness -> "Drunkenness"
    | CharacterAttribute.Energy -> "Energy"
    | CharacterAttribute.Fame -> "Fame"
    | CharacterAttribute.Health -> "Health"
    | CharacterAttribute.Hunger -> "Hunger"
    | CharacterAttribute.Hygiene -> "Hygiene"
    | CharacterAttribute.Mood -> "Mood"
