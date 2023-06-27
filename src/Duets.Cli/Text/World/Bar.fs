module Duets.Cli.Text.World.Bar

open Duets.Common
open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Bar ->
        [ "The bar is bustling with activity as a lively crowd fills every corner. Laughter and conversation echo throughout the room as people jostle for space at the counter. It's a vibrant and energetic atmosphere, with patrons mingling and enjoying their drinks amidst the lively chatter of a packed house."
          "The bar is moderately busy, with a comfortable number of patrons scattered across the space. There are enough people to create a pleasant ambiance, but not so many that it feels overcrowded. It's a cozy setting, with pockets of conversation and occasional bursts of laughter breaking the tranquil hum of voices."
          "The bar is relatively quiet, with only a few patrons scattered around. The calm atmosphere allows for easy conversation, as individuals sit at the bar or relax in the dimly lit booths. The soft clinking of glasses and the occasional murmurs create a tranquil backdrop, providing a peaceful environment for those seeking a more intimate experience."
          "The bar is nearly empty, with only a couple of people huddled together at a corner table. The subdued lighting highlights the empty stools at the counter, giving the place a somewhat deserted feel. It's a perfect spot for someone seeking solitude or a quiet drink, with the occasional burst of laughter from the small group serving as a reminder of the rare company." ]
        |> List.sample
    | _ -> failwith "Room type not supported in bar"
