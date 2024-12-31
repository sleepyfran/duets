module rec Duets.Data.World.Cities.LosAngeles.Root

open Duets.Entities

let generate () =
    let city =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset> hollywood

    city

let hollywood =
    let studioRow = StudioRow.studioRow
    let boulevardOfStars = BoulevardOfStars.boulevardOfStars
    let alleyway = Alleyway.alleyway

    let metroStation =
        { Line = Blue
          LeavesToStreet = studioRow.Id }

    World.Zone.create "Hollywood" (World.Node.create studioRow.Id studioRow)
    |> World.Zone.addStreet (
        World.Node.create boulevardOfStars.Id boulevardOfStars
    )
    |> World.Zone.addStreet (World.Node.create alleyway.Id alleyway)
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation metroStation
