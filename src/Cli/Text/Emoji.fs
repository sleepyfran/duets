[<RequireQualifiedAccess>]
module Cli.Text.Emoji


open Entities

/// Emoji for representing actions inside of a plane.
let flying = ":airplane_departure:"

/// Emoji for representing actions related to concerts. IMPORTANT: Do NOT remove
/// the space in the end since the emoji does not seem to render well without it.
let concert = ":admission_tickets: "

/// Returns the correct emoji for showing the current day moment.
let dayMoment dayMoment =
    match dayMoment with
    | EarlyMorning -> ":six_o_clock:"
    | Morning -> ":ten_o_clock:"
    | Midday -> ":two_o_clock:"
    | Afternoon -> ":six_o_clock:"
    | Evening -> ":eight_o_clock:"
    | Night -> ":ten_o_clock:"
    | Midnight -> ":twelve_o_clock:"

let private mood m =
    match m with
    | m when m < 20 -> ":slightly_frowning_face:"
    | m when m < 50 -> ":neutral_face:"
    | m when m < 75 -> ":slightly_smiling_face:"
    | _ -> ":beaming_face_with_smiling_eyes:"

let attribute attr amount =
    match attr with
    | CharacterAttribute.Drunkenness -> ":woozy_face:"
    | CharacterAttribute.Energy -> ":battery:"
    | CharacterAttribute.Fame -> ":glowing_star:"
    | CharacterAttribute.Health ->
        ":anatomical_heart: " (* Do NOT remove the space in the end *)
    | CharacterAttribute.Mood -> mood amount
