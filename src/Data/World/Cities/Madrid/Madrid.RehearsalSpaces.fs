module Data.World.Cities.Madrid.RehearsalSpaces

open Entities

let addJackOnTheRocks zone =
    let rehearsalSpace = { Price = 20m<dd> }

    { Id = "479ec3de-10ef-41a5-a158-882ef031c125" |> Identity.from |> PlaceId
      Name = "Jack on the Rocks"
      Quality = 50<quality>
      Type = RehearsalSpace rehearsalSpace
      Zone = zone }
    |> World.City.addPlace

let addPandorasVox zone =
    let rehearsalSpace = { Price = 65m<dd> }

    { Id = "85ebaab3-3e1c-4c3b-afb2-d6ba2944ab9c" |> Identity.from |> PlaceId
      Name = "Pandora's Vox"
      Quality = 80<quality>
      Type = RehearsalSpace rehearsalSpace
      Zone = zone }
    |> World.City.addPlace
