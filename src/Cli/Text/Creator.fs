[<RequireQualifiedAccess>]
module Cli.Text.Creator

open Common

let characterInitialPrompt =
    $"""Creating a new game, what's the {Styles.highlight "name"} of your character?"""

let characterGenderPrompt =
    $"""What's their {Styles.highlight "gender"}?"""

let characterGenderMale = "Male"
let characterGenderFemale = "Female"
let characterGenderOther = "Other"

let characterAgePrompt =
    $"""How {Styles.highlight "old"} are they? (Minimum 18)"""

let bandInitialPrompt =
    $"""Let's create your first band. What's the {Styles.highlight "band's name"}?"""

let bandGenrePrompt =
    $"""What {Styles.highlight "genre"} are they going to be playing? You can always change this later"""

let bandInstrumentPrompt =
    $"""And lastly, what will you be {Styles.highlight "playing"}?"""

let bandConfirmationPrompt characterName bandName bandGenre instrument =
    $"""You'll be playing as {Styles.highlight characterName} in the band {Styles.highlight bandName} playing {Styles.highlight bandGenre} as a {Generic.roleName instrument
                                                                                                                                                 |> String.lowercase
                                                                                                                                                 |> Styles.highlight}"""

let errorCharacterNameTooShort =
    Styles.error "Your character's name is too short"

let errorCharacterNameTooLong =
    Styles.error "Your character's name is too long"

let errorCharacterAgeTooYoung =
    Styles.error "Your character is too young"

let errorCharacterAgeTooOld =
    Styles.error "Your character is too old"

let errorBandNameTooShort =
    Styles.error "Your band's name is too short"

let errorBandNameTooLong =
    Styles.error "Your band's name is too long"
