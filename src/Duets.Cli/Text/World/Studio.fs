module Duets.Cli.Text.World.Studio

open Duets.Cli.Text
open Duets.Entities

let rec description (studio: Studio) (place: Place) (roomType: RoomType) =
    match roomType with
    | RoomType.MasteringRoom
    | RoomType.RecordingRoom -> $"{studioSpaceDescription place}\n{studioPrice studio}"
    | _ -> failwith "Room type not supported in studio"

and private studioSpaceDescription place =
    match place.Quality with
    | q when q < 30<quality> ->
        Styles.Level.bad
            "The studio is in a terrible state. The walls are crumbling, the floor is cracked, and the ceiling is leaking. It's a miracle that the place is still standing at all. The only thing that seems to be in okay condition is the equipment. The producer is smoking a cigarette and doesn't seem to be paying attention to you"
    | q when q < 60<quality> ->
        Styles.Level.normal
            "The studio is in a decent state. It's not the best looking studio, but it's still a good place to record music. The equipment seems to be in a good condition, but it's not new and seems to have been used for a while. The producer is on its phone and doesn't seem to be paying attention to you"
    | _ ->
        Styles.Level.good
            "The studio looks great, seems to have been recently renovated. The equipment looks new and shiny. The producer is waiting for you to start recording!"

and private studioPrice studio =
    $"Each song will cost {Styles.money studio.PricePerSong} to record and produce"
