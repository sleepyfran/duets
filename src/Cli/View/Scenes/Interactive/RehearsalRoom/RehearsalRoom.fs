module Cli.View.Scenes.Interactive.RehearsalRoom.Root

open Cli.View.Actions
open Cli.View.Scenes.Interactive.Objects
open Cli.View.TextConstants
open Entities
open Simulation.Queries.Bands

let private instrumentFromType instrumentType =
    let create fn =
        fn
            (TextConstant RehearsalRoomInstrumentPlayDescription)
            (seq { SubScene SubScene.RehearsalRoomCompose })

    match instrumentType with
    | InstrumentType.Bass -> create bass
    | InstrumentType.Drums -> create drums
    | InstrumentType.Guitar -> create guitar
    | InstrumentType.Vocals -> create microphone

/// Creates the rehearsal room which allows to access the compose and managing
/// section.
let rec rehearsalRoomScene state =
    let characterInstrument =
        currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    seq {
        yield Figlet <| TextConstant RehearsalRoomTitle

        yield
            InteractiveRoom
            <| { Description = TextConstant RehearsalRoomDescription
                 Objects = [ characterInstrument ]
                 ExtraCommands =
                     [ { Name = "manage"
                         Description =
                             TextConstant RehearsalRoomManageDescription
                         Handler =
                             HandlerWithNavigation
                                 (fun _ -> seq { Scene Management }) } ] }
    }
