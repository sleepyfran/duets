module Duets.Cli.Text.World.Bookstore

open Duets.Common
open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.ReadingRoom ->
        [ "You step into a world of knowledge and imagination as the scent of old paper and ink mingles with the subtle aroma of coffee from the in-store cafe. Shelves stretch up to the ceiling, lined with books of every conceivable genre. The soft rustling of pages being turned provides a rhythmic backdrop to whispers of excited discussion amongst patrons about their latest literary finds."
          "The bookstore offers a cozy nook amid the day's hustle, with stacks of well-thumbed novels and gleaming new releases fighting for attention. A gentle lull presides as readers meander through the aisles, enveloped in the quiet yet palpable excitement that a good book awaits just around the corner. Comfortable chairs are sporadically occupied by absorbed individuals, lost within the pages of their chosen stories."
          "Décor resembling an old study gives the bookstore an air of intimacy and scholarly charm. Only a handful of customers browse the shelves, each exuding a contemplative demeanor as they peruse the curated selection. The space is intimate, perfect for book lovers seeking a tranquil ambiance where they can consult with knowledgeable staff or become engrossed in a rare find."
          "The quiet bookstore has an almost sacred silence, punctuated only by the quiet thud of a book being set on a table or the distant sound of a single conversation between the clerk and a regular customer. Dim reading lamps cast a warm light over the few patrons who have found solace in the shadowy corners amongst the towering rows of stories yet untold and wisdom yet to be shared." ]
        |> List.sample
    | _ -> failwith "Room type not supported in bookstore"
