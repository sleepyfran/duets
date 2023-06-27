module Duets.Cli.Text.World.Hospital

open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Lobby ->
        "The lobby of the hospital is a spacious, well-lit area with high ceilings and walls painted in a soothing shade of pale blue. Large windows fill the space with natural light, revealing a glimpse of the outside world. A diverse group of people fills the room, some seated on comfortable chairs and sofas in small seating areas. They include individuals of varying ages, from elderly individuals patiently awaiting their turn to families with children seeking medical attention. The floor, made of polished marble tiles, reflects the overhead lights, and small side tables hold neatly arranged magazines and brochures about medical topics."
    | _ -> failwith "Room type not supported in hospital"
