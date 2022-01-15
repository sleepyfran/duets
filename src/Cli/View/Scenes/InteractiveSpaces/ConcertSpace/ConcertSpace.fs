module Cli.View.Scenes.InteractiveSpaces.ConcertSpace

open Cli.View.Actions
open Cli.View.Scenes.InteractiveSpaces
open Cli.View.Text
open Entities
open Simulation.Queries.Bands

let private instrumentFromType instrumentType =
    let create fn =
        fn
            (I18n.translate (ConcertSpaceText ConcertSpaceStartConcert))
            Seq.empty

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let getPlaceName room =
    match room with
    | Lobby space -> Literal space.Name
    | Bar space -> Literal space.Name
    | Stage space -> Literal space.Name

let getRoomName room =
    match room with
    | Lobby _ -> I18n.translate (CommonText CommonLobbyName)
    | Bar _ -> I18n.translate (CommonText CommonBarName)
    | Stage _ -> I18n.translate (ConcertSpaceText ConcertSpaceStageName)

let getRoomDescription room =
    match room with
    | Lobby space ->
        I18n.translate (
            ConcertSpaceLobbyDescription space
            |> ConcertSpaceText
        )
    | Bar space ->
        I18n.translate (
            ConcertSpaceBarDescription space
            |> ConcertSpaceText
        )
    | Stage space ->
        I18n.translate (
            ConcertSpaceStageDescription space
            |> ConcertSpaceText
        )

let getRoomObjects room =
    let state = State.Root.get ()

    let characterInstrument =
        currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | Lobby _ -> []
    | Bar _ -> []
    | Stage _ -> [ characterInstrument ]

let getRoomCommands _ = []
