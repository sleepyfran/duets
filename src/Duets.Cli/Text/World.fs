[<RequireQualifiedAccess>]
module Duets.Cli.Text.World

open Duets.Agents
open Duets.Entities
open Duets.Simulation

let private defaultPlaceDescription (place: Place) =
    $"You are at {Styles.place place.Name}"

let private concertSpaceDescription (place: Place) =
    let currentBand = Queries.Bands.currentBand (State.get ())

    let scheduledConcert =
        Queries.Concerts.scheduledForRightNow
            (State.get ())
            currentBand.Id
            place.Id

    match scheduledConcert with
    | Some scheduledConcert ->
        let concert = Concert.fromScheduled scheduledConcert
        let attendancePercentage = Queries.Concerts.attendancePercentage concert

        match attendancePercentage with
        | att when att = 0 ->
            Styles.Level.bad
                "The place is eerily silent as you make your way in. The only sound is the echo of your footsteps on the empty floor. You can't believe that no one came to the concert. The stage is set up and ready to go, with instruments and equipment waiting to be played. The lights are dimmed, but you can see that the rows of seats in the auditorium are completely empty."
        | att when att <= 10 ->
            Styles.Level.bad
                "The concert venue is almost completely silent as you make your way in. The only sound is the echo of your footsteps on the nearly empty floor. You can't help but feel surprised that only a handful of people came to the concert. The stage is set up and ready to go, with instruments and equipment waiting to be played. The lights are dimmed, but you can see that the rows of seats in the auditorium are mostly empty."
        | att when att <= 35 ->
            Styles.Level.bad
                "The concert venue is relatively quiet as you make your way in. The only sound is the echo of your footsteps on the mostly empty floor. You can't help but feel surprised that only a fraction of the seats are filled. The stage is set up and ready to go, with instruments and equipment waiting to be played. The lights are dimmed, but you can see that the rows of seats in the auditorium are mostly empty."
        | att when att <= 60 ->
            Styles.Level.normal
                "The concert venue is relatively quiet as you make your way in. The only sound is the echo of your footsteps on the mostly empty floor. You can't help but feel surprised that only half of the seats are filled, it seems like the crowd is not what you were expecting. The stage is set up and ready to go, with instruments and equipment waiting to be played. The lights are dimmed, but you can see that the rows of seats in the auditorium are half filled."
        | att when att <= 85 ->
            Styles.Level.good
                "The concert venue is bustling with excitement as you make your way in. The only sound is the murmur of the crowd as they take their seats. You can't help but feel surprised that almost all the seats are filled, it seems like the crowd is more than you were expecting. The stage is set up and ready to go, with instruments and equipment waiting to be played. The lights are dimmed, but you can see that the rows of seats in the auditorium are almost full."
        | _ ->
            Styles.Level.great
                "The concert venue is packed with excitement as you make your way in. The only sound is the roar of the crowd as they take their seats. You can't help but feel surprised that every single seat is filled and there are even people standing in the back, it seems like the crowd is way more than you were expecting. The stage is set up and ready to go, with instruments and equipment waiting to be played. The lights are dimmed, but you can see that the rows of seats in the auditorium are completely filled and there's not even an inch of empty space."
    | None -> defaultPlaceDescription place

let studioSpaceDescription place =
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

let studioPrice studio =
    $"Each song will cost {Styles.money studio.PricePerSong} to record and produce"

let placeDescription (place: Place) =
    match place.Type with
    | PlaceType.ConcertSpace _ -> concertSpaceDescription place
    | PlaceType.Studio studio ->
        $"{studioSpaceDescription place}\n{studioPrice studio}"
    | _ -> defaultPlaceDescription place

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Airport -> "Airport"
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.Cafe -> "Café"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Restaurant -> "Restaurant"
    | PlaceTypeIndex.Studio -> "Studio"

let placeWithZone (place: Place) =
    $"{Styles.place place.Name} ({place.Zone.Name})"

let movedTo (place: Place) = $"You make your way to {place.Name}..."
