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
    let effects, state = Simulation.tick (State.get ()) effect

    State.set state

    effects
    |> Seq.tap Log.append
    |> Seq.iter displayEffect

and private displayEffect effect =
    match effect with
    | SongImproved (_, Diff (before, after)) ->
        let (_, _, previousQuality) = before
        let (_, _, currentQuality) = after

        ImproveSongCanBeFurtherImproved(previousQuality, currentQuality)
        |> RehearsalSpaceText
        |> I18n.translate
        |> showMessage
    | SongPracticed (_, (FinishedSong song, _)) ->
        PracticeSongImproved(song.Name, song.Practice)
        |> RehearsalSpaceText
        |> I18n.translate
        |> showMessage
    | SkillImproved (character, Diff (before, after)) ->
        let (skill, previousLevel) = before
        let (_, currentLevel) = after

        CommonSkillImproved(
            character.Name,
            character.Gender,
            skill,
            previousLevel,
            currentLevel
        )
        |> CommonText
        |> I18n.translate
        |> showMessage
    | MoneyTransferred (holder, transaction) ->
        BankAppTransferSuccess(holder, transaction)
        |> PhoneText
        |> I18n.translate
        |> showMessage
    | AlbumRecorded (_, UnreleasedAlbum album) ->
        StudioCreateAlbumRecorded album.Name
        |> StudioText
        |> I18n.translate
        |> showMessage
    | AlbumRenamed (_, UnreleasedAlbum album) ->
        StudioContinueRecordAlbumRenamed album.Name
        |> StudioText
        |> I18n.translate
        |> showMessage
    | AlbumReleased (_, releasedAlbum) ->
        StudioCommonAlbumReleased releasedAlbum.Album.Name
        |> StudioText
        |> I18n.translate
        |> showMessage
    | ConcertScheduled (_, concert) ->
        let venue =
            Queries.World.concertSpaceById
                (State.get ())
                concert.CityId
                concert.VenueId
            |> Option.get

        SchedulerAssistantAppTicketDone(venue, concert)
        |> PhoneText
        |> I18n.translate
        |> showMessage
    | _ -> ()
