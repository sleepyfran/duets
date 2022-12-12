module rec Cli.Scenes.Phone.Apps.Jobs.FindJob

open Agents
open Cli
open Cli.Components
open Cli.Text
open Data.Careers
open Entities
open Simulation
open Simulation.Careers

let findJob jobsApp (currentCareer: Job option) =
    let jobType =
        showOptionalChoicePrompt
            Phone.findJobTypePrompt
            Generic.cancel
            Career.name
            Careers.all

    match jobType with
    | Some jobType -> findJobForType jobsApp currentCareer jobType
    | None -> jobsApp ()

let private findJobForType jobsApp currentCareer jobType =
    let availableJobs =
        JobBoard.availableJobsInCurrentCity (State.get ()) jobType

    let selectedJob =
        showOptionalChoicePrompt
            Phone.findJobSelectPrompt
            Generic.back
            jobItemText
            availableJobs

    match selectedJob with
    | Some job -> askForJobConfirmation currentCareer job
    | None -> ()

    jobsApp ()

let private askForJobConfirmation currentCareer job =
    let newCareerPlace = job.Location ||> Queries.World.placeInCityById

    let confirmed =
        match currentCareer with
        | Some currentJob ->
            let currentPlace =
                currentJob.Location ||> Queries.World.placeInCityById

            Phone.findJobAcceptLeaveConfirmation
                job.Id
                newCareerPlace.Name
                currentJob.Id
                currentPlace.Name
        | None -> Phone.findJobAcceptConfirmation job.Id newCareerPlace.Name
        |> showConfirmationPrompt

    if confirmed then
        Employment.acceptJob (State.get ()) job |> Effect.applyMultiple
    else
        ()

let private jobItemText job =
    let place = job.Location ||> Queries.World.placeInCityById

    Phone.findJobSelectItem job place.Name
