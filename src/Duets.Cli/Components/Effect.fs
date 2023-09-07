[<RequireQualifiedAccess>]
module rec Duets.Cli.Effect

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation

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
    | AlbumReleased(_, releasedAlbum) ->
        Studio.commonAlbumReleased releasedAlbum.Album.Name |> showMessage
    | AlbumReviewsReceived(_, releasedAlbum) ->
        $"The reviews for your album {releasedAlbum.Album.Name} just came in!"
        |> showMessage

        let accepted = showConfirmationPrompt "Do you want to see them now?"

        if accepted then
            showReviews releasedAlbum
        else
            "Reviews are always accessible via the phone"
            |> Styles.faded
            |> showMessage
    | BandSwitchedGenre(band, Diff(prevGenre, currGenre)) ->
        $"Your band {band.Name} is now playing {currGenre |> Styles.genre} instead of {prevGenre |> Styles.genre}"
        |> showMessage
    | CareerAccept(_, job) ->
        let place = job.Location ||> Queries.World.placeInCityById

        Career.careerChange job place.Name |> showMessage
    | CareerLeave(_, job) ->
        let place = job.Location ||> Queries.World.placeInCityById

        Career.careerLeft job place.Name |> showMessage
    | CareerPromoted(job, salary) ->
        let place = job.Location ||> Queries.World.placeInCityById

        Career.careerPromoted job place.Name salary |> showMessage
    | CareerShiftPerformed(_, payment) ->
        Career.workShiftFinished payment |> showMessage
    | CharacterAttributeChanged(_, attr, Diff(previous, current)) ->
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
    | ConcertScheduled(_, ScheduledConcert(concert, _)) ->
        let place = Queries.World.placeInCityById concert.CityId concert.VenueId

        Phone.concertAssistantAppTicketDone place concert |> showMessage
    | ConcertFinished(_, pastConcert, income) ->
        let concert = Concert.fromPast pastConcert

        let quality =
            match pastConcert with
            | PerformedConcert(_, quality) -> quality
            | _ -> 0<quality>

        match quality with
        | q when q < 35<quality> -> Concert.finishedPoorly
        | q when q < 75<quality> -> Concert.finishedNormally
        | _ -> Concert.finishedGreat
        |> showMessage

        Concert.concertSummary concert income |> showMessage
    | ConcertCancelled(band, FailedConcert(concert, reason)) ->
        let place = Queries.World.placeInCityById concert.CityId concert.VenueId

        match reason with
        | BandDidNotMakeIt -> Concert.failedBandMissing band place concert
        | CharacterPassedOut -> Concert.failedCharacterPassedOut
        |> showMessage
    | ItemAddedToInventory item ->
        Items.itemAddedToInventory item.Brand |> showMessage
    | ItemRemovedFromInventory item ->
        Items.itemRemovedFromInventory item.Brand |> showMessage
    | MoneyTransferred(holder, transaction) ->
        Phone.bankAppTransferSuccess holder transaction |> showMessage
    | Notification notification ->
        let createCalendarNotification typeText date dayMoment =
            $"{typeText} scheduled for {Date.simple date |> Styles.time} @ {Generic.dayMomentName dayMoment |> Styles.time}"
            |> Styles.highlight
            |> showNotification "Upcoming event"

        let createRentalNotification text =
            text |> Styles.highlight |> showNotification "Upcoming payment"

        match notification with
        | Notification.CalendarEvent(CalendarEventType.Flight flight) ->
            createCalendarNotification
                $"Flight from {Generic.cityName flight.Origin |> Styles.place}"
                flight.Date
                flight.DayMoment
        | Notification.CalendarEvent(CalendarEventType.Concert concert) ->
            let venue =
                Queries.World.placeInCityById concert.CityId concert.VenueId

            createCalendarNotification
                $"Concert at {venue.Name |> Styles.place} in {Generic.cityName concert.CityId |> Styles.place}"
                concert.Date
                concert.DayMoment
        | Notification.RentalNotification(RentalNotificationType.RentalDueTomorrow rental) ->
            let cityId, _ = rental.Coords
            let place = rental.Coords ||> Queries.World.placeInCityById

            $"Your rental of {place.Name} in {Generic.cityName cityId |> Styles.place} will expire tomorrow if you don't pay the next rent.\nYou can do so from your phone's bank app"
            |> createRentalNotification
        | Notification.RentalNotification(RentalNotificationType.RentalDueInOneWeek rental) ->
            let cityId, _ = rental.Coords
            let place = rental.Coords ||> Queries.World.placeInCityById

            $"Your rental of {place.Name} in {Generic.cityName cityId |> Styles.place} is set to expire in one week unless you pay the next rent.\nHead over to your phone's bank app to do so"
            |> createRentalNotification
    | PlaceClosed place ->
        lineBreak ()

        Styles.danger $"{place.Name} is closing and they're kicking you out."
        |> showMessage

        "Choose where you want to go next" |> showMessage

        showMapUntilChoice () |> applyMultiple
    | RentalKickedOut _ ->
        "Since your rental has ran out, you need to go somewhere else"
        |> Styles.error
        |> showMessage

        showMapUntilChoice () |> applyMultiple
    | RentalExpired rental ->
        lineBreak ()

        let expiredPlace = rental.Coords ||> Queries.World.placeInCityById
        let cityId, _ = rental.Coords

        match rental.RentalType with
        | Monthly _ ->
            $"You didn't pay this month's rent, so you can no longer access {expiredPlace.Name |> Styles.place} in {Generic.cityName cityId |> Styles.place}"
        | OneTime _ ->
            $"You rental of {expiredPlace.Name |> Styles.place} in {Generic.cityName cityId |> Styles.place} has expired, so you can no longer access it"
        |> Styles.warning
        |> showNotification "Rental expired"
    | SongImproved(_, Diff(before, after)) ->
        let (Unfinished(_, _, previousQuality)) = before
        let (Unfinished(_, _, currentQuality)) = after

        Rehearsal.improveSongCanBeFurtherImproved (
            previousQuality,
            currentQuality
        )
        |> showMessage
    | SongPracticed(_, Finished(song, _)) ->
        Rehearsal.practiceSongImproved song.Name song.Practice |> showMessage
    | SongDiscarded(_, Unfinished(song, _, _)) ->
        Rehearsal.discardSongDiscarded song.Name |> showMessage
    | SongFinished(_, Finished(song, quality)) ->
        Rehearsal.finishSongFinished (song.Name, quality) |> showMessage
    | SkillImproved(character, Diff(before, after)) ->
        let (skill, previousLevel) = before
        let (_, currentLevel) = after

        Skill.skillImproved
            character.Name
            character.Gender
            skill
            previousLevel
            currentLevel
        |> showMessage
    | Wait _ ->
        let today = Queries.Calendar.today (State.get ())

        let currentDayMoment = Calendar.Query.dayMomentOf today

        Command.waitResult today currentDayMoment |> showMessage
    | _ -> ()
