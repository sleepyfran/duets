[<RequireQualifiedAccess>]
module Duets.Cli.Text.Emoji


open Duets.Entities

/// Emoji for representing actions inside of a plane.
let flying = ":airplane_departure:"

/// Emoji for representing actions related to concerts. IMPORTANT: Do NOT remove
/// the space in the end since the emoji does not seem to render well without it.
let concert = ":admission_tickets: "

/// Emoji for representing actions related to socializing.
let socializing = ":mouth:"

/// Emoji for representing a notification.
let notification = ":bell:"

/// Emoji for a clock in a random time.
let clock = ":eight_o_clock:"

/// Emoji for the Mastodon app.
let mastodon = ":elephant:"

/// Emoji for boosts in social network apps.
let boost = ":star:"

/// Emoji for card games.
let cards = ":joker:"

/// Emoji for clubs.
let clubs = ":club_suit:"

/// Emoji for diamonds.
let diamonds = ":diamond_suit:"

/// Emoji for hearts.
let hearts = ":heart_suit:"

/// Emoji for spades.
let spades = ":spade_suit:"

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
    | CharacterAttribute.Hunger -> ":pot_of_food:"
    | CharacterAttribute.Mood -> mood amount

let moodlet m =
    match m with
    | MoodletType.JetLagged -> ":sleepy_face:"
    | MoodletType.NotInspired -> ":expressionless_face:"
    | MoodletType.TiredOfTouring -> ":minibus:"
