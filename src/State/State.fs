[<RequireQualifiedAccess>]
module State.Root

open Entities

type StateMessage =
    | Get of AsyncReplyChannel<State>
    | Set of State

type StateAgent() =
    let state =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop state =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Get channel ->
                        channel.Reply state
                        return! loop state
                    | Set value -> return! loop value
                }

            loop State.empty

    member this.Get() = state.PostAndReply Get

    member this.Set value = Set value |> state.Post

    member this.Map fn = this.Get() |> fn |> this.Set

let staticAgent = StateAgent()

/// Returns the state of the game.
let get = staticAgent.Get

/// Sets the state of the game. Prefer to use `apply` instead, only exposed
/// for the savegame loading.
let set = staticAgent.Set

/// Applies an effect to the state.
let apply effect =
    match effect with
    | GameCreated state -> staticAgent.Set state
    | SongStarted (band, unfinishedSong) ->
        Songs.addUnfinished staticAgent.Map band unfinishedSong
    | SongImproved (band, (Diff (_, unfinishedSong))) ->
        Songs.addUnfinished staticAgent.Map band unfinishedSong
    | SongFinished (band, finishedSong) ->
        let song = Song.fromFinished finishedSong
        Songs.removeUnfinished staticAgent.Map band song.Id
        Songs.addFinished staticAgent.Map band finishedSong
    | SongDiscarded (band, unfinishedSong) ->
        let song = Song.fromUnfinished unfinishedSong
        Songs.removeUnfinished staticAgent.Map band song.Id
    | MemberHired (band, currentMember) ->
        Bands.addMember staticAgent.Map band currentMember
    | MemberFired (band, currentMember, pastMember) ->
        Bands.removeMember staticAgent.Map band currentMember
        Bands.addPastMember staticAgent.Map band pastMember
    | SkillImproved (character, Diff (_, skill)) ->
        Skills.add staticAgent.Map character skill
    | MoneyTransferred (account, transaction) ->
        Bank.transfer staticAgent.Map account transaction
    | AlbumRecorded (band, album) ->
        Albums.addUnreleased staticAgent.Map band album
        let (UnreleasedAlbum ua) = album

        ua.TrackList
        |> List.map (fun ((FinishedSong fs), _) -> fs.Id)
        |> List.iter (Songs.removeFinished staticAgent.Map band)
    | AlbumRenamed (band, unreleasedAlbum) ->
        let (UnreleasedAlbum album) = unreleasedAlbum
        Albums.removeUnreleased staticAgent.Map band album.Id
        Albums.addUnreleased staticAgent.Map band unreleasedAlbum
    | AlbumReleased (band, releasedAlbum) ->
        let (ReleasedAlbum(album, _)) = releasedAlbum
        Albums.removeUnreleased staticAgent.Map band album.Id
        Albums.addReleased staticAgent.Map band releasedAlbum
