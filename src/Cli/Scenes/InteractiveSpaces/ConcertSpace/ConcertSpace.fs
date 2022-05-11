module Cli.Scenes.InteractiveSpaces.ConcertSpace.Root

open Agents
open Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands
open Cli.Text
open Entities
open Simulation

let getRoomName room =
    match room with
    | Room.Lobby _ -> I18n.translate (CommonText CommonLobbyName)
    | Room.Bar _ -> I18n.translate (CommonText CommonBarName)
    | Room.Stage _ -> I18n.translate (ConcertText ConcertSpaceStageName)
    | Room.Backstage -> I18n.translate (ConcertText ConcertSpaceBackstageName)
    | _ -> Literal ""

let getRoomDescription space room =
    match room with
    | Room.Lobby ->
        I18n.translate (ConcertSpaceLobbyDescription space |> ConcertText)
    | Room.Bar ->
        I18n.translate (ConcertSpaceBarDescription space |> ConcertText)
    | Room.Stage ->
        I18n.translate (ConcertSpaceStageDescription space |> ConcertText)
    | Room.Backstage ->
        I18n.translate (
            ConcertSpaceBackstageDescription space
            |> ConcertText
        )
    | _ -> Literal ""

let getRoomObjects _ = []

let getRoomCommands room =
    match room with
    | Room.Backstage ->
        let currentSituation =
            Queries.Situations.current (State.get ())

        match currentSituation with
        | InConcert ongoingConcert ->
            [ DoEncoreCommand.create ongoingConcert
              EndConcertCommand.create ongoingConcert ]
        | _ -> []
    | _ -> []
