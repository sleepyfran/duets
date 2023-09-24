module rec Duets.Cli.Scenes.Phone.Apps.Jobs.FindJob

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Data.Careers
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Careers

let findJob jobsApp (currentCareer: Job option) =
    let jobType =
        showOptionalChoicePrompt
            Phone.findJobTypePrompt
            Generic.cancel
            Career.typeName
            Careers.all

    match jobType with
    | Some jobType -> findJobForType jobsApp currentCareer jobType
    | None -> jobsApp ()

let private findJobForType jobsApp currentCareer jobType =
    let availableJobs =
        JobBoard.availableJobsInCurrentCity (State.get ()) jobType

    if List.isEmpty availableJobs then
        "There are no available jobs like this at this time or your skills are too low for any of them"
        |> Styles.error
        |> showMessage
    else
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
                job
                newCareerPlace.Name
                currentJob
                currentPlace.Name
        | None -> Phone.findJobAcceptConfirmation job newCareerPlace.Name
        |> showConfirmationPrompt

    if confirmed then
        Employment.acceptJob (State.get ()) job |> Effect.applyMultiple
    else
        ()

let private jobItemText job =
    let place = job.Location ||> Queries.World.placeInCityById

    Phone.findJobSelectItem job place.Name
