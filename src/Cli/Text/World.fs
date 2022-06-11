[<RequireQualifiedAccess>]
module Cli.Text.World

open Common
open Entities

let title = "World"

let streetDescription name descriptors =
    $"""{Styles.place name} is a {Generic.listOf descriptors (Generic.descriptorText >> String.lowercase)} street"""

let boulevardDescription name descriptors =
    $"""{Styles.place name} is a {Generic.listOf descriptors (Generic.descriptorText >> String.lowercase)} boulevard"""

let squareDescription name descriptors =
    $"""{Styles.place name} is a {Generic.listOf descriptors (Generic.descriptorText >> String.lowercase)} square"""

let concertSpaceKickedOutOfStage =
    $"""Initially the people in the bar were looking weirdly at you thinking what were you doing in there. Then the {Styles.person "bouncer"} came and kicked you out warning you {Styles.danger
                                                                                                                                                                                       "not to get in the stage again if you're not part of the band playing"}"""

let concertSpaceKickedOutOfBackstage =
    $"""You tried to sneak into the {Styles.place "backstage"}, but the bouncers catch you as soon as you enter and kicked you out warning you {Styles.danger "not to enter in there if you're not part of the band playing"}"""

let backstageName = "Backstage"
let barName = "Bar"
let bedroomName = "Bedroom"
let kitchenName = "Kitchen"
let livingRoomName = "Living room"
let lobbyName = "Lobby"

let masteringRoomName = "Mastering Room"

let recordingRoomName = "Recording Room"

let rehearsalRoomName = "Rehearsal Room"

let stageName = "Stage"

let backstageDescription space =
    match space.Quality with
    | quality when quality < 20<quality> ->
        $"The backstage of {Styles.place space.Name} has absolutely nothing to offer. It's incredibly small, dark and full of unused stuff from the venue. It seems like you'll barely fit your gear in here"
    | quality when quality < 50<quality> ->
        $"The backstage of {Styles.place space.Name} it's not that bad, but it's still a bit small. You have a section to put your gear, but there's not much more room for anything else. There seems to be a corner with some drinks, so at least something"
    | quality when quality < 80<quality> ->
        $"The backstage of {Styles.place space.Name} is pretty big and has a lot of space for the entire band. There's a corner with some drinks and food, so make sure you get something before you go out!"
    | _ ->
        $"The backstage of {Styles.place space.Name} is absolutely amazing, there's free drinks, food and even a corner with a jacuzzi for you to relax. Make sure you relax as much as possible before the big concert!"

let barDescription (space: Place) =
    $"""With a lot of overpriced drinks and a bunch of drunk people lining up for the concert, the bar of {Styles.place space.Name} doesn't look as bad as you'd imagine."""

let bedroomDescription =
    "Your silent bedroom is looking great and cozy."

let kitchenDescription =
    "You still have some rest of yesterday's food, but otherwise the kitchen is very clean."

let livingRoomDescription =
    "The living room at your place looks really nice, the couch is big and comfy and right in front of a big TV."

let lobbyDescription (space: Place) =
    $"""The lobby of {Styles.place space.Name} is mostly empty right now. Only a person asking for tickets is to be seen."""

let rehearsalRoomDescription =
    $"""You are in the {Styles.place "rehearsal room"} inside an old and quite smelly building. You can feel the smoke in the air and hear {Styles.band "AC/DC"} being played in the room nearby."""

let masteringRoomDescription =
    "You are in the mastering room, where the producer sits in front of a computer and a bunch of knobs"

let recordingRoomDescription =
    "A recording room with all the instruments you can imagine, although for now the only one that matters is the one that you can play."

let stageDescription (space: Place) =
    $"""You go up the stage of {Styles.place space.Name} and you're temporarily blinded by the lights pointing towards you. After a few seconds you begin to see some faces in the crowd and the people start whistling and applauding. Time to give your everything!"""
