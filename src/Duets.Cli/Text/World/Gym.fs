[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Gym

open Duets.Common
open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.ChangingRoom ->
        [ "The changing room appears neglected, with just one person quickly changing in the dim lighting. The benches and lockers show signs of wear, reflecting a lack of attention."
          "The changing room bustles with activity as people go about their post-workout routines. Towels are exchanged, conversations sparked, and personal transformations unfold. The atmosphere is functional, if not opulent."
          "The changing room exudes a sense of luxury as individuals move gracefully through their post-exercise rituals. Well-maintained lockers and pristine facilities contribute to an environment of self-care. Conversations and reflections intertwine in the air." ]
        |> List.sample
    | RoomType.Gym ->
        [ "The gym feels neglected, with just one lone soul half-heartedly lifting weights and pacing on treadmills. The atmosphere is a bit lackluster, mirroring the faded equipment."
          "The gym hums with activity as people engage in various exercises. Some chat between sets, while others focus intently on their routines. The atmosphere is functional, catering to a mix of goals."
          "The gym buzzes with energy and camaraderie as a lot of fitness enthusiasts pursue their goals. Weightlifting, cardio, and stretches harmonize into a symphony of dedication. The atmosphere exudes vibrancy and progress." ]
        |> List.sample
    | RoomType.Lobby ->
        [ "The gym reception area is abuzz with a lot of people. The receptionist multitasks, checking members in, answering questions, and offering workout tips."
          "The gym reception area is quiet, with only a few people glancing at workout schedules." ]
        |> List.sample
    | _ -> failwith "Room type not supported in gym"
