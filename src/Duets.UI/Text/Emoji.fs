module rec Duets.UI.Text.Emoji

open Duets.Entities

let attribute attr (amount: int) =
    match attr with
    | CharacterAttribute.Drunkenness -> "🥴"
    | CharacterAttribute.Energy -> "🔋"
    | CharacterAttribute.Fame -> "🌟"
    | CharacterAttribute.Health -> "🫀"
    | CharacterAttribute.Hunger -> "🍽️"
    | CharacterAttribute.Mood -> mood amount

let concert = "🎫"

let dayMoment =
    function
    | EarlyMorning -> "🕡"
    | Morning -> "🕙"
    | Midday -> "🕑"
    | Afternoon -> "🕕"
    | Evening -> "🕗"
    | Night -> "🕥"
    | Midnight -> "🕛"

let private mood =
    function
    | m when m < 20 -> "🙁"
    | m when m < 50 -> "😐"
    | m when m < 75 -> "🙂"
    | _ -> "😁"
