[<RequireQualifiedAccess>]
module Duets.Cli.Text.Creator

open Duets.Common
open Duets.Entities

let characterInitialPrompt =
    $"""Creating a new game, what's the {Styles.highlight "name"} of your character?"""

let characterGenderPrompt = $"""What's their {Styles.highlight "gender"}?"""

let characterGenderMale = "Male"
let characterGenderFemale = "Female"
let characterGenderOther = "Other"

let characterBirthdayInfo =
    Styles.faded
        $"The game starts on {Calendar.gameBeginning.Year} and your character has to be at least 18 by then"

let characterBirthdayPrompt gender =
    $"""When was {Generic.possessiveAdjectiveForGender gender |> String.lowercase} {Styles.highlight "born"}? (format {Styles.information "YYYY"})"""

let bandInitialPrompt =
    $"""Let's create your first band. What's the band's {Styles.highlight "name"}?"""

let bandGenrePrompt =
    $"""What {Styles.highlight "genre"} are they going to be playing? You can always change this later"""

let bandInstrumentPrompt =
    $"""And lastly, which {Styles.highlight "role"} will you have in the band?"""

let bandConfirmationPrompt characterName bandName bandGenre instrument =
    $"""You'll be playing as {Styles.highlight characterName} in the band {Styles.highlight bandName} playing {Styles.highlight bandGenre} as a {Generic.roleName instrument |> String.lowercase |> Styles.highlight}"""

let cityInfo =
    Styles.faded
        "You need an initial city to start, but you will be able to move anywhere else after the game starts"

let cityPrompt = $"""Which city do you want to {Styles.highlight "live"} in?"""

let errorCharacterNameTooShort =
    Styles.error "Your character's name is too short"

let errorCharacterNameTooLong = Styles.error "Your character's name is too long"

let errorCharacterAgeTooYoung = Styles.error "Your character is too young"

let errorCharacterAgeTooOld = Styles.error "Your character is too old"

let errorBandNameTooShort = Styles.error "Your band's name is too short"

let errorBandNameTooLong = Styles.error "Your band's name is too long"
