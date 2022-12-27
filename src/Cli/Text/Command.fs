[<RequireQualifiedAccess>]
module Cli.Text.Command

open Common
open Entities

let prompt = Styles.faded ">"

let commonPrompt date dayMoment attributes =
    $"""{Generic.infoBar date dayMoment attributes}
{Styles.prompt "What do you want to do? Type 'help' if you're lost"}"""

let adjustDrumsDescription =
    "Allows you to adjust the drum settings. Doesn't do much, but it's important to play comfortably"

let bassSoloDescription =
    "Allows you to play a bass solo and remind people that you are on stage"

let createAlbumDescription =
    "Allows you to record one or more finished songs to create and release an album"

let composeSongDescription = "Allows you to create a new song"

let disabledNotEnoughEnergy _ =
    Styles.error "You don't have enough energy to do that. Try to rest first"

let disabledNotEnoughHealth _ =
    Styles.error
        "You don't have enough health to do that. Try to rest or go to a doctor"

let disabledNotEnoughMood _ =
    Styles.error
        "You don't really feel like doing that... Maybe something more interesting might spark your interest"

let drumSoloDescription =
    "Allows you to play a drum solo and show the world that the drummer can also rock"

let editAlbumNameDescription =
    "Allows you to edit the name of an album you previously recorded but did not release"

let helpDescription = "Here are all the commands you can execute right now:"

let helpFooter =
    Styles.faded
        $"""Remember that when referencing items you {Styles.highlight "don't"} need to write diacritics"""

let discardSongDescription =
    "Prompts you for a song to discard from the list of unfinished songs"

let giveSpeechDescription =
    "Allows you to give a speech to the crowd. Make sure you're actually good with words, otherwise people might not understand you"

let playDescription = "Allows you to choose a song to play in the concert"

let dedicateSongDescription =
    "Dedicates a song and then plays it. Might give you a little boost in points as long as you don't overdo it"

let getOffStageDescription =
    "Moves you to the backstage, where you can decide if you want to do an encore (if possible) or finish the concert for good"

let greetAudienceDescription =
    "Allows you to be a polite musician and say hello before blasting the crowd's ears"

let doEncoreDescription = "Moves you back to the stage where you can play more!"

let finishConcertDescription = "Finishes the concert"

let faceBandDescription = "Makes you look towards your band, isn't that cool?"

let faceCrowdDescription = "Makes you look towards the crowd again"

let finishSongDescription =
    "Prompts your for a song to finish from the list of unfinished songs"

let fireMemberDescription = "Allows you to fire a member of your band"

let guitarSoloDescription =
    "Allows you to perform a guitar solo and elevate your ego to a higher dimension"

let hireMemberDescription = "Allows you to hire a new member for your band"

let improveSongDescription =
    "Prompts you for a song to improve a previously composed song"

let listMembersDescription = "Lists all current and past members of your band"

let lookDescription = "Shows all the objects you have around you"

let lookExit exit =
    $"""There's also an exit to {Styles.place exit} ({Styles.information "out"})"""

let lookNoObjectsAround = "There are no objects around you"

let lookVisibleObjectsPrefix = "You can see:"

let outDescription = "Exits the current place"

let exitDescription = "Exits the game saving the progress"

let makeCrowdSingDescription =
    "Allows you to try to make the crowd sing along a chant or a song, so get your voice ready"

let phoneDescription =
    "Opens your phone where you can check statistics and manage your bank"

let practiceSongDescription =
    "Prompts you for a song to practice, which improves the quality when performing the song live"

let putMicOnStandDescription = "Allows you to put the mic back on the stand"

let releaseAlbumDescription =
    "Allows you to release an album you previously recorded but did not release"

let takeMicDescription =
    "Allows you to take the mic from the stand and move freely on the stage"

let talkDescription =
    $"""Allows you to talk with a character in the world. Use as {Styles.information "talk to {name}"}. You can reference characters by their full name or just their first name"""

let talkInvalidInput =
    Styles.error
        $"""I didn't quite catch that. Make sure you are referencing characters by their first or full name with {Styles.information "talk to {name}"}"""

let talkNpcNotFound name =
    Styles.error $"There are no characters named '{name}' around"

let talkNothing = "Nothing"

let tuneInstrumentDescription =
    "Tunes your instrument, which looks cool sometimes"

let waitDescription =
    $"""Waits for the given amount of time. Use as {Styles.information "wait 4"}, where the number is the amount of day moments to wait"""

let waitInvalidTimes input =
    Styles.error $"The given amount '{input}' is not valid. Try a real number"

let waitResult date dayMoment =
    $"""You waited and it's now {Generic.dayMomentName dayMoment |> String.lowercase |> Styles.highlight} on the {Generic.formatDate date |> Styles.highlight}"""

let sleepDescription =
    "Allows you to get some sleep and restore your energy and health"

let playConsoleDescription =
    "Allows you to play some video games and restore your mood"

let watchTvDescription = "Allows you to watch TV and restore your mood"

let orderDescription =
    $"""Allows you to order an item from the establishment's menu. Use either as {Styles.information "order {item name}"} or use without arguments to select interactively"""

let seeMenuDescription =
    "Displays the available items that can be bought in this establishment"

let inventoryDescription = "Displays the items that you are currently carrying"

let drinkDescription =
    $"""Allows you to drink a given item. Use as {Styles.information "drink {item name}"}"""

let eatDescription =
    $"""Allows you to eat a given item. Use as {Styles.information "eat {item name}"}"""

let meDescription = "Shows information about your character"

let private meItem header value =
    $"""- {Styles.header header}: {value}"""

let meName name = meItem "Name" name

let meBirthdayAge birthday age =
    meItem "Birthday" $"{Generic.formatDate birthday} ({age} years old)"

let wrongUsage usageSample =
    Styles.error $"Can't recognize that. Use as: {usageSample}"

let mapDescription =
    "Shows all the places in the current city that you can travel to"

let mapChoosePlaceTypePrompt =
    Styles.prompt "Which type of place do you want to travel to?"

let mapChoosePlace = Styles.prompt "Which place do you want to travel to?"

let mapCurrentCity cityId =
    Styles.header
        $"""You are currently in {Generic.cityName cityId |> Styles.highlight}"""

let mapTip =
    Styles.faded
        "Tip: In order to travel anywhere outside this city, use the airport to fly to another city"

let boardPlaneDescription flight =
    $"Boards your plane to {Generic.cityName flight.Destination}"

let waitForLandingDescription =
    "Makes you wait until the plane lands and lets you enter the destination airport"

let startConcertDescription = "Starts the concert that you scheduled here"

let workDescription (job: Job) =
    $"Starts a shift in your work as {Career.name job.Id}. It will take {Career.shiftDurationDescription job.Schedule}"
