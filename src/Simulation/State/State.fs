module Simulation.State.Root

open Aether
open Entities

/// Applies an effect to the state.
let applyEffect state effect =
    match effect with
    | AlbumRecorded (band, album) ->
        let modifiedState =
            Albums.addUnreleased band album state

        let (UnreleasedAlbum ua) = album

        ua.TrackList
        |> List.map (fun ((FinishedSong fs), _) -> fs.Id)
        |> List.fold
            (fun currentState song ->
                Songs.removeFinished band song currentState)
            modifiedState
    | AlbumRenamed (band, unreleasedAlbum) ->
        let (UnreleasedAlbum album) =
            unreleasedAlbum

        Albums.removeUnreleased band album.Id state
        |> Albums.addUnreleased band unreleasedAlbum
    | AlbumReleased (band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeUnreleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | AlbumReleasedUpdate (band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeReleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | CharacterAttributeChanged (character, attribute, amount) ->
        Characters.setAttribute character.Id attribute amount state
    | CharacterHealthDepleted _ -> state
    | CharacterHospitalized (_, (cityId, nodeId)) ->
        World.move cityId nodeId state
    | ConcertScheduled (band, concert) ->
        Concerts.addScheduledConcert band concert state
    | ConcertUpdated (band, concert) ->
        Concerts.removeScheduledConcert band concert state
        |> Concerts.addScheduledConcert band concert
    | ConcertFinished (band, pastConcert) ->
        let concert = Concert.fromPast pastConcert

        Concerts.removeScheduledConcert band (ScheduledConcert concert) state
        |> Concerts.addPastConcert band pastConcert
    | ConcertCancelled (band, pastConcert) ->
        let concert = Concert.fromPast pastConcert

        Concerts.removeScheduledConcert band (ScheduledConcert concert) state
        |> Concerts.addPastConcert band pastConcert
    | GameCreated state -> state
    | GenreMarketsUpdated genreMarkets -> Market.set genreMarkets state
    | MemberHired (band, character, currentMember, skills) ->
        let stateWithMember =
            Characters.add character state
            |> Bands.addMember band currentMember

        skills
        |> List.fold
            (fun currentState skill ->
                Skills.add currentMember.CharacterId skill currentState)
            stateWithMember
    | MemberFired (band, currentMember, pastMember) ->
        Bands.removeMember band currentMember state
        |> Bands.addPastMember band pastMember
    | MoneyTransferred (account, transaction) ->
        Bank.setBalance account transaction state
    | MoneyEarned (account, transaction) ->
        Bank.setBalance account transaction state
    | SkillImproved (character, Diff (_, skill)) ->
        Skills.add character.Id skill state
    | SongStarted (band, unfinishedSong) ->
        Songs.addUnfinished band unfinishedSong state
    | SongImproved (band, (Diff (_, unfinishedSong))) ->
        Songs.addUnfinished band unfinishedSong state
    | SongFinished (band, finishedSong) ->
        let song = Song.fromFinished finishedSong

        Songs.removeUnfinished band song.Id state
        |> Songs.addFinished band finishedSong
    | SongPracticed (band, finishedSong) ->
        let song = Song.fromFinished finishedSong

        Songs.removeFinished band song.Id state
        |> Songs.addFinished band finishedSong
    | SongDiscarded (band, unfinishedSong) ->
        let song =
            Song.fromUnfinished unfinishedSong

        Songs.removeUnfinished band song.Id state
    | SituationChanged situation ->
        Optic.set Lenses.State.situation_ situation state
    | TimeAdvanced time -> Calendar.setTime time state
    | WorldMoveTo (cityId, nodeId) -> World.move cityId nodeId state
    | Wait _ -> state
