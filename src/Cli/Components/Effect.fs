[<RequireQualifiedAccess>]
module Cli.Effect

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
let rec apply effect =
    let effects, state =
        Simulation.tick (State.get ()) effect

    State.set state

    effects
    |> Seq.tap Log.append
    |> Seq.iter displayEffect

/// <summary>
/// Calls <c>apply</c> one time for each given effect in the list.
/// </summary>
/// <param name="effects">Effects to apply</param>
and applyMultiple effects = effects |> List.iter apply

and private displayEffect effect =
    match effect with
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
    | SongImproved (_, Diff (before, after)) ->
        let (_, _, previousQuality) = before
        let (_, _, currentQuality) = after

        Rehearsal.improveSongCanBeFurtherImproved (
            previousQuality,
            currentQuality
        )
        |> showMessage
    | SongPracticed (_, (FinishedSong song, _)) ->
        Rehearsal.practiceSongImproved song.Name song.Practice
        |> showMessage
    | SongDiscarded (_, (UnfinishedSong song, _, _)) ->
        Rehearsal.discardSongDiscarded song.Name
        |> showMessage
    | SongFinished (_, (FinishedSong song, quality)) ->
        Rehearsal.finishSongFinished (song.Name, quality)
        |> showMessage
    | SkillImproved (character, Diff (before, after)) ->
        let (skill, previousLevel) = before
        let (_, currentLevel) = after

        Generic.skillImproved
            character.Name
            character.Gender
            skill
            previousLevel
            currentLevel
        |> showMessage
    | MoneyTransferred (holder, transaction) ->
        Phone.bankAppTransferSuccess holder transaction
        |> showMessage
    | AlbumRecorded (_, UnreleasedAlbum album) ->
        Studio.createAlbumRecorded album.Name
        |> showMessage
    | AlbumRenamed (_, UnreleasedAlbum album) ->
        Studio.continueRecordAlbumRenamed album.Name
        |> showMessage
    | AlbumReleased (_, releasedAlbum) ->
        Studio.commonAlbumReleased releasedAlbum.Album.Name
        |> showMessage
    | ConcertScheduled (_, ScheduledConcert concert) ->
        let coordinates =
            Queries.World.Common.coordinatesOfPlace
                (State.get ())
                (Node concert.VenueId)
            |> Option.get

        Phone.schedulerAssistantAppTicketDone coordinates.Place concert
        |> showMessage
    | ConcertFinished (_, pastConcert) ->
        let quality =
            match pastConcert with
            | PerformedConcert (_, quality) -> quality
            | _ -> 0<quality>

        match quality with
        | q when q < 35<quality> -> Concert.finishedPoorly q
        | q when q < 75<quality> -> Concert.finishedNormally q
        | q -> Concert.finishedGreat q
        |> showMessage
    | ConcertCancelled (band, FailedConcert (concert, reason)) ->
        let coordinates =
            Queries.World.Common.coordinatesOfPlace
                (State.get ())
                (Node concert.VenueId)
            |> Option.get

        match reason with
        | BandDidNotMakeIt ->
            Concert.failedBandMissing band coordinates.Place concert
        | CharacterPassedOut -> Concert.failedCharacterPassedOut
        |> showMessage
    | Wait _ ->
        let today =
            Queries.Calendar.today (State.get ())

        let currentDayMoment =
            Calendar.Query.dayMomentOf today

        Command.waitResult today currentDayMoment
        |> showMessage
    | _ -> ()
