[<AutoOpen>]
module Simulation.Concerts.Live.Actions

open Entities

/// Plays the given song in the concert with the specified energy. The result
/// depends on whether the song was already played or not and the energy.
let playSong state ongoingConcert (finishedSong, _) energy =
    let (FinishedSong song) = finishedSong

    { Event = PlaySong(song, energy)
      Limit = Penalized(1<times>, PointPenalization(-50))
      Effects = []
      AffectingQualities =
          // TODO: Add this after testing; SongQuality(finishedSong, quality)
          [ SongPractice(finishedSong) ]
      Multipliers =
          [ match energy with
            | PerformEnergy.Energetic -> 15
            | PerformEnergy.Normal -> 8
            | PerformEnergy.Limited -> 2 ] }
    |> performAction state ongoingConcert


/// Greets the audience and returns the result. If the audience has been greeted
/// already then decreases points.
let greetAudience state ongoingConcert =
    { Event = GreetAudience
      Limit = Penalized(1<times>, PointPenalization(-10))
      Effects = []
      AffectingQualities = []
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Adds a give speech event to the list, limiting the amount to 3.
let giveSpeech state ongoingConcert =
    { Event = GiveSpeech
      Limit = NoAction(3<times>)
      Effects = []
      AffectingQualities = [ CharacterSkill(SkillId.Speech) ]
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Performs the given song adding a dedication, which grants 10 additional points
/// on the performance only if done less than 2 times.
let dedicateSong state ongoingConcert song energy =
    let dedicationsGiven =
        Concert.Ongoing.timesDoneEvent ongoingConcert DedicateSong

    match dedicationsGiven with
    | times when times < 2<times> ->
        playSong state ongoingConcert song energy
        |> Response.addEvent DedicateSong
        |> Response.addPoints 10
    | _ -> Response.empty' ongoingConcert TooManyRepetitionsNotDone
