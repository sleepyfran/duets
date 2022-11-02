module UI.Components.MultiProgressBar

open System
open System.Threading
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Common
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

type MultiProgressBarInput = {
    /// List of steps that will be shown with a progress bar.
    Steps: string list
    /// Duration in seconds of each of the steps.
    StepDuration: int<second>
    /// Callback once all the steps have finished.
    OnFinish: unit -> unit
}

let view input =
    Component.create (
        "ProgressBar",
        fun ctx ->
            let progresses =
                input.Steps
                |> List.map (fun _ -> 0)
                |> ctx.useState

            let timer =
                new PeriodicTimer(
                    TimeSpan.FromSeconds(
                        input.StepDuration / 1<second> |> float
                    )
                )

            ctx.useEffect (
                handler =
                    (fun _ ->
                        let mutable activeSteps =
                            progresses.Current
                            |> List.mapi (fun originalIndex _ -> {|
                                OriginalIndex = originalIndex
                            |})

                        let cts = new CancellationTokenSource()
                        let ct = cts.Token

                        let updateOp = task {
                            while true do
                                ct.ThrowIfCancellationRequested()
                                let! _ = timer.WaitForNextTickAsync()

                                let indexToUpdate =
                                    activeSteps |> List.sampleIndex

                                let itemToUpdate =
                                    activeSteps |> List.item indexToUpdate

                                (* TODO: Revisit this, it's ugly af. *)
                                progresses.Current
                                |> List.mapi (fun index progress ->
                                    if index = itemToUpdate.OriginalIndex then
                                        let updatedProgress =
                                            progress + 20 |> Math.clamp 0 100

                                        if updatedProgress = 100 then
                                            activeSteps <-
                                                List.removeAt
                                                    indexToUpdate
                                                    activeSteps

                                        updatedProgress
                                    else
                                        progress)
                                |> progresses.Set

                                let allFinished =
                                    progresses.Current
                                    |> List.forall (fun progress ->
                                        progress = 100)

                                if allFinished then
                                    timer.Dispose()
                                    cts.Cancel()
                                    cts.Dispose()
                                    input.OnFinish()
                        }

                        Async.Start(updateOp |> Async.AwaitTask, ct)),
                triggers = [ EffectTrigger.AfterInit ]
            )

            input.Steps
            |> List.mapi (fun idx element ->
                Layout.horizontal [
                    TextBlock.create [ TextBlock.text element ]
                    ProgressBar.create [
                        ProgressBar.minimum 0
                        ProgressBar.maximum 100
                        ProgressBar.value progresses.Current[idx]
                    ]
                ]
                :> IView)
            |> Layout.vertical
            :> IView
    )
