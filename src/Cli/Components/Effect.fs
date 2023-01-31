[<RequireQualifiedAccess>]
module rec Cli.Effect

open Agents
open Cli.Components
open Cli.Text
open Common
open Entities
open Simulation

/// <summary>
/// Applies an effect to the simulation and displays any message or action that
/// is associated with that effect. For example, transferring money displays a
/// message with the transaction.
/// </summary>
/// <param name="effect">Effect to apply</param>
let apply effect =
    Simulation.tickOne (State.get ()) effect ||> digest

/// <summary>
/// Calls <c>apply</c> one time for each given effect in the list.
/// </summary>
/// <param name="effects">Effects to apply</param>
let applyMultiple effects =
    effects |> Simulation.tickMultiple (State.get ()) ||> digest

let private digest effects state =
    State.set state

    effects |> Seq.tap Log.appendEffect |> Seq.iter displayEffect

let private displayEffect effect =
    match effect with
    | AlbumRecorded (_, UnreleasedAlbum album) ->
        Studio.createAlbumRecorded album.Name |> showMessage
    | AlbumRenamed (_, UnreleasedAlbum album) ->
        Studio.continueRecordAlbumRenamed album.Name |> showMessage
    | AlbumReleased (_, releasedAlbum) ->
        Studio.commonAlbumReleased releasedAlbum.Album.Name |> showMessage
    | CharacterAttributeChanged (_, attr, Diff (previous, current)) ->
        match attr with
        | CharacterAttribute.Drunkenness ->
            if previous < current then
                (* We are drinking more. *)
                match current with
                | amount when amount < 25 -> Events.feelingTipsy
                | amount when amount < 50 -> Events.feelingDrunk
                | _ -> Events.feelingReallyDrunk
                |> showMessage
            else
                (* Character is sobering up. *)
                match (previous, current) with
                | prev, curr when prev > 25 && curr <= 25 ->
                    Events.soberingTipsy |> showMessage
                | prev, curr when prev > 50 && curr <= 50 ->
                    Events.soberingDrunk |> showMessage
                | _ -> ()
        | _ -> ()
    | CharacterHealthDepleted _ ->
        lineBreak ()
        wait 5000<millisecond>
        showMessage Events.healthDepletedFirst
        wait 5000<millisecond>
        showMessage Events.healthDepletedSecond
        wait 5000<millisecond>
        lineBreak ()
    | CharacterHospitalized _ ->
        showMessage Events.hospitalized
        lineBreak ()
    | CareerAccept (_, job) ->
        let place = job.Location ||> Queries.World.placeInCityById

        Career.careerChange job place.Name |> showMessage
    | CareerLeave (_, job) ->
        let place = job.Location ||> Queries.World.placeInCityById

        Career.careerLeft job place.Name |> showMessage
    | ConcertScheduled (_, ScheduledConcert (concert, _)) ->
        let place = Queries.World.placeInCityById concert.CityId concert.VenueId

        Phone.concertAssistantAppTicketDone place concert |> showMessage
    | ConcertFinished (_, pastConcert, income) ->
        let concert = Concert.fromPast pastConcert

        let quality =
            match pastConcert with
            | PerformedConcert (_, quality) -> quality
            | _ -> 0<quality>

        match quality with
        | q when q < 35<quality> -> Concert.finishedPoorly
        | q when q < 75<quality> -> Concert.finishedNormally
        | _ -> Concert.finishedGreat
        |> showMessage

        Concert.concertSummary concert.TicketsSold income |> showMessage
    | ConcertCancelled (band, FailedConcert (concert, reason)) ->
        let place = Queries.World.placeInCityById concert.CityId concert.VenueId

        match reason with
        | BandDidNotMakeIt -> Concert.failedBandMissing band place concert
        | CharacterPassedOut -> Concert.failedCharacterPassedOut
        |> showMessage
    | ItemAddedToInventory item ->
        Items.itemAddedToInventory item.Brand |> showMessage
    | ItemRemovedFromInventory item ->
        Items.itemRemovedFromInventory item.Brand |> showMessage
    | MoneyTransferred (holder, transaction) ->
        Phone.bankAppTransferSuccess holder transaction |> showMessage
    | NotificationEventHappeningSoon event ->
        match event with
        | CalendarEventType.Flight flight ->
            $"Flight from {Generic.cityName flight.Origin |> Styles.place}",
            flight.Date,
            flight.DayMoment
        | CalendarEventType.Concert concert ->
            let venue =
                Queries.World.placeInCityById concert.CityId concert.VenueId

            $"Concert at {venue.Name |> Styles.place} in {Generic.cityName concert.CityId |> Styles.place}",
            concert.Date,
            concert.DayMoment
        |> fun (typeText, date, dayMoment) ->
            $"{typeText} scheduled for {Date.simple date |> Styles.time} @ {Generic.dayMomentName dayMoment |> Styles.time}"
            |> Styles.highlight
        |> showNotification "Upcoming event"
    | SongImproved (_, Diff (before, after)) ->
        let (_, _, previousQuality) = before
        let (_, _, currentQuality) = after

        Rehearsal.improveSongCanBeFurtherImproved (
            previousQuality,
            currentQuality
        )
        |> showMessage
    | SongPracticed (_, (FinishedSong song, _)) ->
        Rehearsal.practiceSongImproved song.Name song.Practice |> showMessage
    | SongDiscarded (_, (UnfinishedSong song, _, _)) ->
        Rehearsal.discardSongDiscarded song.Name |> showMessage
    | SongFinished (_, (FinishedSong song, quality)) ->
        Rehearsal.finishSongFinished (song.Name, quality) |> showMessage
    | SkillImproved (character, Diff (before, after)) ->
        let (skill, previousLevel) = before
        let (_, currentLevel) = after

        Skill.skillImproved
            character.Name
            character.Gender
            skill
            previousLevel
            currentLevel
        |> showMessage
    | PlaceClosed place ->
        lineBreak ()

        Styles.danger $"{place.Name} is closing and they're kicking you out."
        |> showMessage

        "Choose where you want to go next" |> showMessage

        let rec displayMapUntilChoice () =
            match showMap () with
            | effects when effects.Length > 0 -> applyMultiple effects
            | _ -> displayMapUntilChoice ()

        displayMapUntilChoice ()
    | Wait _ ->
        let today = Queries.Calendar.today (State.get ())

        let currentDayMoment = Calendar.Query.dayMomentOf today

        Command.waitResult today currentDayMoment |> showMessage
    | WorldMoveTo (cityId, placeId) ->
        Queries.World.placeInCityById cityId placeId
        |> World.movedTo
        |> showMessage

        wait 1000<millisecond>
    | _ -> ()
