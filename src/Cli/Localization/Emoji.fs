module Cli.Localization.Emoji

/// Emoji for representing actions related to concerts. IMPORTANT: Do NOT remove
/// the space in the end since the emoji does not seem to render well without it.
let concert = ":admission_tickets: "

/// Emoji for representing the energy of a character.
let energy = ":battery:"

/// Emoji for representing the fame of a character or band.
let fame = ":glowing_star:"

/// Emoji for representing the health of a character. IMPORTANT: Do NOT remove
/// the space in the end since the emoji does not seem to render well without it.
let health = ":anatomical_heart: "

/// Returns the correct smiley emoji for the current mood level.
let mood m =
    match m with
    | m when m < 20 -> ":slightly_frowning_face:"
    | m when m < 50 -> ":neutral_face:"
    | m when m < 75 -> ":slightly_smiling_face:"
    | _ -> ":beaming_face_with_smiling_eyes:"
