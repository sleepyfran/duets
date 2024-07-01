module Duets.Agents.Stats

open System
open System.Diagnostics
open Duets.Common
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

type Stats = { SecondsPlayed: int<second> }

let private empty = { SecondsPlayed = 0<second> }

let private loadStats () =
    Files.statsPath ()
    |> Files.readAll
    |> Option.bind Serializer.deserialize
    |> Option.defaultValue empty

let private writeStats (stats: Stats) =
    stats |> Serializer.serialize |> Files.write (Files.statsPath ())

let private addElapsedToStats (elapsed: TimeSpan) =
    let stats = loadStats ()
    let elapsedSeconds = elapsed.Seconds |> (*) 1<second>

    { SecondsPlayed = stats.SecondsPlayed + elapsedSeconds } |> writeStats

let private startTracking (sw: Stopwatch) =
    if sw.IsRunning then
        Log.appendMessage
            "Attempted to start a timewatch that was already running"
    else
        sw.Start()

let private stopTracking (sw: Stopwatch) =
    match sw with
    | stopWatch when stopWatch.IsRunning ->
        stopWatch.Stop()
        stopWatch.Elapsed |> addElapsedToStats
    | _ ->
        Log.appendMessage "Attempted to stop a timewatch that was not running"

type private SavegameAgentMessage =
    | Read of AsyncReplyChannel<Stats>
    | StartTracking
    | StopTracking of AsyncReplyChannel<unit>

/// Agent in charge of writing and loading the stats of the game.
type private StatsAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop stopWatch =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Read channel ->
                        try
                            loadStats () |> channel.Reply
                        with _ ->
                            channel.Reply empty

                        return! loop stopWatch
                    | StartTracking ->
                        startTracking stopWatch
                        return! loop stopWatch
                    | StopTracking channel ->
                        try
                            stopTracking stopWatch
                        with ex ->
                            Log.appendMessage
                                $"Saving stats failed with error {ex.Message}"

                        channel.Reply()

                        return! loop stopWatch
                }

            loop (Stopwatch())

    member _.Read() = agent.PostAndReply Read
    member _.StartTracking() = agent.Post StartTracking
    member _.StopTracking() = agent.PostAndReply StopTracking

let private statsAgent = StatsAgent()

/// Attempts to load the game stats from the file if it exist.
let retrieve = statsAgent.Read

/// Starts tracking the elapsed time.
let startTrackingTime = statsAgent.StartTracking

/// Stops tracking the elapsed time and saves it in the stats file, adding the
/// elapsed time to the current amount of seconds saved on the file.
let stopTrackingAndSave = statsAgent.StopTracking
