module Cli.Scenes.InteractiveSpaces.ConcertSpace.Root

open Agents
open Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands
open Cli.Text
open Entities
open Simulation

let getRoomName room =
    match room with
    | Lobby _ -> I18n.translate (CommonText CommonLobbyName)
    | Bar _ -> I18n.translate (CommonText CommonBarName)
    | Stage _ -> I18n.translate (ConcertText ConcertSpaceStageName)
    | Backstage -> I18n.translate (ConcertText ConcertSpaceBackstageName)

let getRoomDescription space room =
    match room with
    | Lobby ->
        I18n.translate (ConcertSpaceLobbyDescription space |> ConcertText)
    | Bar -> I18n.translate (ConcertSpaceBarDescription space |> ConcertText)
    | Stage ->
        I18n.translate (ConcertSpaceStageDescription space |> ConcertText)
    | Backstage ->
        I18n.translate (
            ConcertSpaceBackstageDescription space
            |> ConcertText
        )

let getRoomObjects _ = []

let getRoomCommands room =
    match room with
    | Backstage ->
        let currentSituation =
            Queries.Situations.current (State.get ())

        match currentSituation with
        | InConcert ongoingConcert ->
            [ DoEncoreCommand.create ongoingConcert
              EndConcertCommand.create ongoingConcert ]
        | _ -> []
    | _ -> []
