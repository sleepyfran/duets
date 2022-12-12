module Cli.Scenes.Phone.Apps.Jobs.Root

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Simulation

type private JobsMenuOption = | FindJob

let private textFromOption opt =
    match opt with
    | FindJob -> Phone.findJobOption

let rec jobsApp () =
    let currentCareer = Queries.Career.current (State.get ())
    
    match currentCareer with
    | Some job ->
        let currentJobPlace =
            job.Location ||> Queries.World.placeInCityById
        
        Phone.currentJobDescription job currentJobPlace.Name
    | None ->
        Phone.unemployed
    |> showMessage
    
    lineBreak ()
    
    let option =
        showOptionalChoicePrompt
            Phone.optionPrompt
            Generic.back
            textFromOption
            [ FindJob ]

    match option with
    | Some FindJob -> FindJob.findJob jobsApp currentCareer
    | _ -> Scene.Phone
