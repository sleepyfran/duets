namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Careers.Work

[<RequireQualifiedAccess>]
module WorkCommand =
    /// Command that allows the player to start a shift in their work.
    let create job =
        { Name = "work"
          Description = Command.workDescription job
          Handler =
            fun _ ->

                let result = workShift (State.get ()) job

                match result with
                | Ok effects ->
                    Career.workShiftEvent job |> showMessage
                    wait 5000<millisecond>
                    Effect.applyMultiple effects

                    Scene.WorldAfterMovement
                | Error AttemptedToWorkDuringClosingTime ->
                    "The place is currently close, you can't work now!"
                    |> Styles.error
                    |> showMessage

                    Scene.World
                | Error AttemptedToWorkOnNonScheduledDay ->
                    let workDaysText =
                        match job.CurrentStage.Schedule with
                        | JobSchedule.Fixed(workDays, workDayMoments, _) ->
                            let dayNames =
                                workDays |> List.map Calendar.DayOfWeek.name
                            let dayMomentNames =
                                workDayMoments |> List.map Calendar.DayMoment.name
                            
                            let daysText = Generic.listOf dayNames id
                            let momentsText = Generic.listOf dayMomentNames id
                            
                            $"{daysText} during {momentsText}"
                        | JobSchedule.Free _ -> "" // This shouldn't happen but just in case

                    $"Today is a free day for you. Try again on: {workDaysText}"
                    |> Styles.error
                    |> showMessage

                    Scene.World

        }
