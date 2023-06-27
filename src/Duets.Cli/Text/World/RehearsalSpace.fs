module Duets.Cli.Text.World.RehearsalSpace

open Duets.Cli.Text
open Duets.Entities

let rec description (place: Place) (roomType: RoomType) =
    match roomType with
    | RoomType.Bar ->
        "The bar is managed by the same person that takes care of the rooms. It's a small place, but it has a decent selection of drinks and snacks"
    | RoomType.Lobby ->
        "The entrance of the building is decorated with posters of bands that have rehearsed here before, there's a board with the schedule of the rooms and some hand-written ads of bands looking for members"
    | RoomType.RehearsalRoom -> rehearsalRoomDescription place
    | _ -> failwith "Room type not supported in rehearsal space"

and private rehearsalRoomDescription (place: Place) =
    match place.Quality with
    | q when q < 30<quality> ->
        Styles.Level.bad
            "The room looks terrible. There's empty cans of beer overflowing the trash can, the floor is covered in cigarette butts, and the walls are covered in graffiti. All the instruments seem to be overused, but they'll do the job"
    | q when q < 60<quality> ->
        Styles.Level.normal
            "The room looks okay. It's not the best looking place, but it's still a good place to rehearse. The instruments seem to be in a good condition"
    | _ ->
        Styles.Level.good
            "The room looks great, seems to have been recently renovated. The instruments look new and shiny"
