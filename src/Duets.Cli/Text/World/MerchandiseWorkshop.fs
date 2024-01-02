module Duets.Cli.Text.World.MerchandiseWorkshop

open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Workshop ->
        "Inside the merchandise workshop, rows of glossy workbenches are strewn with an array of design tools: fabric markers, silkscreen frames, and digital tablets. Rolls of various textiles nestle against the walls, while a suite of machines—embroidery, printing, and button-making—sit poised for action. A laminated chart displays size specs and care instructions, and samples of completed T-shirts, hats, and tote bags dangle from a wire grid, showcasing potential designs. In the corner, shelves are stacked with blank merchandise, and a robust computer station commands the room, replete with design software and a library of custom fonts and graphics."
    | _ -> failwith "Room type not supported in bar"
