module rec Duets.Simulation.Events.Place.ClosingTime (* Open all the doors and let you out into the wooooorld... *)

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

/// Checks if the given time is outside of the opening hours of the place
/// the character is currently in and, if so, creates a PlaceClosed effect.
let checkCurrentPlace state =
    let currentPlace = Queries.World.currentPlace state
    let currentTime = Queries.Calendar.today state

    let currentlyClosed =
        Queries.World.placeCurrentlyOpen currentPlace currentTime |> not

    let skipKickingOut = shouldPreserveCharacterInPlace state

    if currentlyClosed && not skipKickingOut then
        let firstExit = currentPlace.Exits |> Map.head

        [ PlaceClosed currentPlace; Navigation.exitTo firstExit state ]
    else
        []

// Returns true if the character should not be kicked out of the place.
let private shouldPreserveCharacterInPlace state =
    let currentCoordinates = Queries.World.currentCoordinates state

    let isWorkplace () =
        match Queries.Career.current state with
        | Some job -> job.Location = currentCoordinates
        | None -> false

    let hasConcertInPlace () =
        let bandId = Queries.Bands.currentBandId state
        let cityId, placeId, _ = currentCoordinates

        Queries.Concerts.scheduledAroundDate state bandId
        |> List.exists (fun concert ->
            concert.CityId = cityId && concert.VenueId = placeId)

    [ isWorkplace; hasConcertInPlace ] |> List.exists (fun fn -> fn ())
