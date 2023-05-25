[<RequireQualifiedAccess>]
module Duets.Cli.Text.Generic

open System
open AvsAnLib
open Duets.Common
open Duets.Entities

type VariableVerb = Have

/// Dictionary of conjugations of verb based on the gender.
let private verbConjugationByGender =
    [ (Have, [ (Male, "Has"); (Female, "Has"); (Other, "Have") ] |> Map.ofList) ]
    |> Map.ofList

/// Returns the formatted instrument name given its type.
let instrumentName instrumentType =
    match instrumentType with
    | Bass -> "Bass"
    | Drums -> "Drums"
    | Guitar -> "Guitar"
    | Vocals -> "Microphone"

/// Returns the formatted role name given its type.
let roleName instrumentType =
    match instrumentType with
    | Bass -> "Bassist"
    | Drums -> "Drummer"
    | Guitar -> "Guitarist"
    | Vocals -> "Singer"

/// Returns the correct pronoun for the given gender (he, she, they).
let subjectPronounForGender gender =
    match gender with
    | Male -> "He"
    | Female -> "She"
    | Other -> "They"

/// Returns the correct object pronoun for the given gender (him, her, them).
let objectPronounForGender gender =
    match gender with
    | Male -> "Him"
    | Female -> "Her"
    | Other -> "Them"

/// Returns the correct possessive for the given gender (his, her, its).
let possessiveAdjectiveForGender gender =
    match gender with
    | Male -> "His"
    | Female -> "Her"
    | Other -> "Its"

/// Returns the indeterminate article 'a' or 'an' that should precede a given
/// word.
let indeterminateArticleFor word = (AvsAn.Query word).Article

/// Returns the correct conjugation for the given verb that matches with the
/// specified gender.
let verbConjugationForGender verb gender =
    verbConjugationByGender |> Map.find verb |> Map.find gender

/// Returns the correct name of the given account holder.
let accountHolderName holder =
    match holder with
    | Character _ -> "your character"
    | Band _ -> "your band"

/// Returns the name of the given moment of the day.
let dayMomentName dayMoment =
    match dayMoment with
    | EarlyMorning -> "Early morning"
    | Morning -> "Morning"
    | Midday -> "Midday"
    | Afternoon -> "Afternoon"
    | Evening -> "Evening"
    | Night -> "Night"
    | Midnight -> "Midnight"

/// Returns the formatted time of a given day moment.
let dayMomentTime dayMoment =
    Calendar.Query.timeOfDayMoment dayMoment
    |> fun hour -> if hour > 9 then $"{hour}:00" else $"0{hour}:00"

/// Returns the name of the given day.
let dayName (day: DayOfWeek) = day.ToString()

/// Returns the name of the day in the given date.
let todayName (date: Date) = date.DayOfWeek.ToString()

/// Returns the formatted name for an album type.
let albumType t =
    match t with
    | Single -> "Single"
    | EP -> "EP"
    | LP -> "LP"

/// Returns the name of a city given its id.
let cityName id =
    match id with
    | Madrid -> "Madrid"
    | Prague -> "Prague"

/// Returns a formatted list as empty if it contains nothing, "a" if it contains
/// only one element, "a and b" with two elements and "a, b and c" for all other
/// lengths.
let rec listOf (stuff: 'a list) toStr =
    match stuff with
    | [] -> ""
    | [ head ] -> toStr head
    | [ head; tail ] -> $"{toStr head} and {toStr tail}"
    | head :: tail -> $"{toStr head}, {listOf tail toStr}"

/// Returns the given singular form if the quantity is 1, or the plural form
/// in all other cases.
let pluralOf singular plural quantity =
    match quantity with
    | quantity when quantity = LanguagePrimitives.Int32WithMeasure 1 -> singular
    | _ -> plural

/// Returns the given singular form if the quantity is 1, or the plural form
/// by putting an "s" in the end in all other cases. Only works for words that
/// have a simple plural where slapping an "s" on the end works, for more
/// complex cases use `pluralOf` directly.
let simplePluralOf singular quantity =
    pluralOf singular $"{singular}s" quantity

/// Formats a given amount of minutes into hours and minutes.
let duration (minutes: int<minute>) =
    let timeSpan = TimeSpan(0, minutes / 1<minute>, 0)

    let hours = timeSpan.Hours
    let minutes = timeSpan.Minutes

    $"""{hours} {simplePluralOf "hour" hours} and {minutes} {simplePluralOf "minute" minutes}"""

/// Formats the character status into a bar that can be shown to the user.
let infoBar
    (date: Date)
    (dayMoment: DayMoment)
    (attributes: (CharacterAttribute * CharacterAttributeAmount) list)
    =
    let baseBar =
        $"""{Emoji.dayMoment dayMoment} {dayMomentName dayMoment |> Styles.time} of {Date.withDayName date |> Styles.time}"""

    attributes
    |> List.fold
        (fun bar (attr, amount) ->
            let styledAmount =
                match attr with
                | CharacterAttribute.Drunkenness ->
                    Styles.Level.fromInverted amount
                | _ -> Styles.Level.from amount

            $"""{bar} | {Emoji.attribute attr amount} {styledAmount}""")
        baseBar

let gameName = "Duets"
let youAreIn place = $"You're currently in {place}"

let choiceSelection selection =
    $"""{Styles.prompt "You selected"} {Styles.object selection}"""

let multiChoiceMoreChoices =
    Styles.faded "(Move up and down to reveal more choices)"

let multiChoiceInstructions =
    $"""Press {Styles.information "space"} to select a choice and {Styles.information "enter"} to finish the selection"""

let noUnfinishedSongs =
    Styles.error "You don't have any songs, create one first"

let back = Styles.faded "Go back"
let cancel = Styles.faded "Cancel"
let doneOption = Styles.faded "Done"
let skipOption = Styles.faded "Skip"

let backToMainMenu = Styles.faded "Back to main menu"

let backToMap = Styles.faded "Back to map"

let backToPhone = Styles.faded "Back to phone"

let backToWorld = Styles.faded "Back to world"

let nothing = Styles.faded "Nothing"

let skills = "Skills"

let invalidDate =
    Styles.error
        $"""Couldn't recognize that date. Try the format {Styles.information "dd/mm/YYYY"} as in 03/07/2022 for 3rd of August of 2022"""

let invalidLength =
    Styles.error
        $"""Couldn't recognize that length. Try the format {Styles.information "mm:ss"} as in 6:55 (6 minutes, 55 seconds)"""

let invalidCommand =
    Styles.error
        $"""That command was not valid. Maybe try again or enter {Styles.information "help"} if you're lost"""

let dayMomentWithTime dayMoment =
    $"""{Styles.highlight (dayMomentName dayMoment)} {Styles.faded $"({dayMomentTime dayMoment})"}"""

let dateWithDay date =
    $"""{Styles.highlight (todayName date)}, {Styles.faded (Date.simple date)}"""

let dateWithDayMoment date dayMoment =
    $"{dateWithDay date} {dayMomentName dayMoment |> Styles.faded}"

let length l = $"{l.Minutes}:{l.Seconds}"

let songWithDetails name (quality: Quality) songLength =
    $"""{name} (Quality: {Styles.Level.from quality}%%, Length: {length songLength})"""

let instrument instrumentType = instrumentName instrumentType
let role instrumentType = roleName instrumentType

let itemName (item: Item) =
    match item.Type with
    | Consumable(Drink drink) ->
        match drink with
        | Beer _ -> $"{item.Brand} beer"
        | Coffee _ -> item.Brand |> String.lowercase
        | Cola _ -> "cola"
        | Lemonade _ -> "lemonade"
    | Consumable(Food food) ->
        match food with
        | Burger _ -> "burger"
        | Chips _ -> "chips"
        | Fries _ -> "fries"
        | Nachos _ -> "nachos"
        | _ -> item.Brand |> String.lowercase
    | Interactive(Electronics electronic) ->
        match electronic with
        | GameConsole -> item.Brand
        | TV -> $"{item.Brand} TV"
    | Interactive(Furniture furniture) ->
        match furniture with
        | Bed -> "bed"
        | Stove -> "stove"
    |> Styles.item

let itemNameWithDetail (item: Item) =
    match item.Type with
    | Consumable(Drink drink) ->
        match drink with
        | Beer(ml, alcohol) -> $"""{Styles.item "Beer"} ({ml}ml, {alcohol}%%)"""
        | Coffee ml -> $"""{Styles.item item.Brand} ({ml}ml of coffee)"""
        | Cola ml -> $"""{Styles.item "Cola"} ({ml}ml)"""
        | Lemonade ml -> $"""{Styles.item "Lemonade"} ({ml}ml)"""
    | Consumable(Food food) ->
        match food with
        | Burger mg
        | Chips mg
        | Chicken mg
        | Croissant mg
        | Falafel mg
        | Fries mg
        | Fruits mg
        | GranolaBowl mg
        | Gyozas mg
        | Nachos mg
        | NemCuon mg
        | BunBo mg
        | PhoBo mg
        | Ramen mg
        | Salad mg
        | Sandwich mg
        | Steak mg
        | Sushi mg
        | Wakame mg -> $"""{Styles.item item.Brand} ({mg} mg)"""
    | Interactive _ -> itemName item

let moreDates = Styles.faded "More dates"
