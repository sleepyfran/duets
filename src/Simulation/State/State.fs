module Simulation.State.Root

open Entities

/// Applies an effect to the state.
let applyEffect state effect =
    match effect with
    | GameCreated state -> state
    | TimeAdvanced time -> Calendar.setTime time state
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
        let song = Song.fromUnfinished unfinishedSong
        Songs.removeUnfinished band song.Id state
    | MemberHired (band, currentMember, skills) ->
        let stateWithMember = Bands.addMember band currentMember state

        skills
        |> List.fold
            (fun currentState skill ->
                Skills.add currentMember.Character skill currentState)
            stateWithMember
    | MemberFired (band, currentMember, pastMember) ->
        Bands.removeMember band currentMember state
        |> Bands.addPastMember band pastMember
    | SkillImproved (character, Diff (_, skill)) ->
        Skills.add character skill state
    | MoneyTransferred (account, transaction) ->
        Bank.setBalance account transaction state
    | MoneyEarned (account, transaction) ->
        Bank.setBalance account transaction state
    | AlbumRecorded (band, album) ->
        let modifiedState = Albums.addUnreleased band album state
        let (UnreleasedAlbum ua) = album

        ua.TrackList
        |> List.map (fun ((FinishedSong fs), _) -> fs.Id)
        |> List.fold
            (fun currentState song ->
                Songs.removeFinished band song currentState)
            modifiedState
    | AlbumRenamed (band, unreleasedAlbum) ->
        let (UnreleasedAlbum album) = unreleasedAlbum

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
    | GenreMarketsUpdated genreMarkets -> Market.set genreMarkets state
    | WorldMoveTo (cityId, nodeId) -> World.move cityId nodeId state
