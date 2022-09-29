module UI.Text.Emoji

open Entities

let dayMoment =
    function
    | Dawn -> "🕡"
    | Morning -> "🕙"
    | Midday -> "🕑"
    | Sunset -> "🕕"
    | Dusk -> "🕗"
    | Night -> "🕥"
    | Midnight -> "🕛"

let private mood =
    function
    | m when m < 20 -> "🙁"
    | m when m < 50 -> "😐"
    | m when m < 75 -> "🙂"
    | _ -> "😁"

let attribute attr amount =
    match attr with
    | CharacterAttribute.Drunkenness -> "🥴"
    | CharacterAttribute.Energy -> "🔋"
    | CharacterAttribute.Fame -> "🌟"
    | CharacterAttribute.Health -> "🫀"
    | CharacterAttribute.Mood -> mood amount
