[<RequireQualifiedAccess>]
module Duets.Cli.Text.Generic

open System
open AvsAnLib
open Duets.Common
open Duets.Entities
open Duets.Simulation

type VariableVerb =
    | Have
    | Be

/// Dictionary of conjugations of verb based on the gender.
let private verbConjugationByGender =
    [ (Have, [ (Male, "Has"); (Female, "Has"); (Other, "Have") ] |> Map.ofList)
      (Be, [ (Male, "Is"); (Female, "Is"); (Other, "Are") ] |> Map.ofList) ]
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
let todayName (date: Date) =
    (Calendar.Query.dayOfWeek date).ToString()

/// Returns the formatted name for an album type.
let albumType t =
    match t with
    | Single -> "Single"
    | EP -> "EP"
    | LP -> "LP"

/// Returns the name of a city given its id.
let cityName id =
    match id with
    | London -> "London"
    | LosAngeles -> "Los Angeles"
    | Madrid -> "Madrid"
    | MexicoCity -> "Mexico City"
    | NewYork -> "New York"
    | Prague -> "Prague"
    | Sydney -> "Sydney"
    | Tokyo -> "Tokyo"

/// Returns a formatted list as empty if it contains nothing, "a" if it contains
/// only one element, "a and b" with two elements and "a, b and c" for all other
/// lengths.
let rec listOf (stuff: 'a list) toStr =
    match stuff with
    | [] -> ""
    | [ head ] -> toStr head
    | [ head; tail ] -> $"{toStr head} and {toStr tail}"
    | head :: tail -> $"{toStr head}, {listOf tail toStr}"

/// Returns a formatted list as empty if it contains nothing, "a" if it contains
/// only one element and a list of all the elements separated by the given
/// separator for all other lengths.
let rec listSeparatedBy (separator: string) (stuff: 'a list) toStr =
    match stuff with
    | [] -> ""
    | [ head ] -> toStr head
    | [ head; tail ] -> $"{toStr head} {separator} {toStr tail}"
    | head :: tail ->
        $"{toStr head} {separator} {listSeparatedBy separator tail toStr}"

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

let timeBar (date: Date) (dayMoment: DayMoment) =
    $"""{Emoji.dayMoment dayMoment} {dayMomentName dayMoment |> Styles.time} of {Date.withDayName date |> Styles.time}"""

/// Formats the character status into a bar that can be shown to the user.
let infoBar
    (date: Date)
    (dayMoment: DayMoment)
    (attributes: (CharacterAttribute * CharacterAttributeAmount) list)
    =
    attributes
    |> List.fold
        (fun bar (attr, amount) ->
            let styledAmount =
                match attr with
                | CharacterAttribute.Drunkenness ->
                    Styles.Level.fromInverted amount
                | _ -> Styles.Level.from amount

            $"""{bar} | {Emoji.attribute attr amount} {styledAmount}""")
        (timeBar date dayMoment)

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
    let mainProperty = item.Properties |> List.head

    match mainProperty with
    | Key(EntranceCard _) -> "entrance card"
    | _ -> item.Name |> String.lowercase
    |> Styles.item

let itemDetailedName (item: Item) =
    let mainProperty = item.Properties |> List.head

    match mainProperty with
    | Drinkable drink ->
        match drink.DrinkType with
        | Beer alcohol ->
            $"""{Styles.item $"{item.Brand}"} ({drink.Amount}ml, {alcohol}%%)"""
        | Coffee coffeeMl ->
            $"""{Styles.item item.Name} ({coffeeMl}ml of coffee)"""
        | Soda -> $"""{Styles.item item.Brand} ({drink.Amount}ml)"""
    | Edible food -> $"""{Styles.item item.Name} ({food.Amount}g)"""
    | Readable(Book book) when book.ReadProgress > 0<percent> ->
        $"{Styles.item book.Title} by {Styles.person book.Author} ({Styles.Level.from book.ReadProgress}%% read)"
    | Readable(Book book) ->
        $"{Styles.item book.Title} by {Styles.person book.Author}"
    | Key(TemporaryChip(cityId, placeId)) ->
        let place = Queries.World.placeInCityById cityId placeId

        Styles.item
            $"Chip for {place.Name |> Styles.place} in {cityName cityId |> Styles.place}"
    | Key(EntranceCard(cityId, placeId)) ->
        let place = Queries.World.placeInCityById cityId placeId

        Styles.item
            $"Entrance card for {place.Name |> Styles.place} in {cityName cityId |> Styles.place}"
    | _ -> itemName item

let moreDates = Styles.faded "More dates"

let miniGameName id =
    match id with
    | MiniGameId.Blackjack -> "blackjack"
