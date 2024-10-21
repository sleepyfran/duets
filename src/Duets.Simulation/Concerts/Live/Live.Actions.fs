[<AutoOpen>]
module Duets.Simulation.Concerts.Live.Actions

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.Time.AdvanceTime

/// Sets the soundcheck to performed in the checklist.
let soundcheck state checklist =
    if checklist.SoundcheckDone then
        (* Technically the interaction should be disabled, but let's do nothing. *)
        []
    else
        let updatedChecklist = { checklist with SoundcheckDone = true }

        [ ConcertSoundcheckPerformed
          Situations.preparingConcert' updatedChecklist ]


/// Sets up the merch stand, which improves the ticket sales of the concert.
let setupMerchStand state checklist =
    if checklist.MerchStandSetup then
        (* Technically the interaction should be disabled, but let's do nothing. *)
        []
    else
        let updatedChecklist =
            { checklist with
                MerchStandSetup = true }

        [ MerchStandSetup; Situations.preparingConcert' updatedChecklist ]

/// Starts the given concert if the band is ready to play.
let startConcert state concert =
    let situation = Queries.Situations.current state

    match situation with
    | Concert(InConcert _) ->
        [] (* Concert already started, no need to do anything. *)
    | Concert(Preparing checklist) ->
        let initialPoints =
            if checklist.SoundcheckDone then 5<quality> else 0<quality>

        [ Situations.inConcert
              { Events = []
                Points = initialPoints
                Checklist = checklist
                Concert = concert } ]
    | _ -> [] (* Band hasn't started preparing, can't start concert. *)

/// Plays the given song in the concert with the specified energy. The result
/// depends on whether the song was already played or not and the energy.
let playSong state ongoingConcert finishedSong energy =
    let (Finished(song, quality)) = finishedSong
    let isLongSong = song.Length.Minutes > 10<minute>

    let playableCharacter = Queries.Characters.playableCharacter state

    { Event = PlaySong(song, energy)
      Limit = Penalized(1<times>, PointPenalization(-50))
      Effects =
        match energy with
        | PerformEnergy.Energetic ->
            [ yield!
                  Character.Attribute.add
                      playableCharacter
                      CharacterAttribute.Health
                      -2
              yield!
                  Character.Attribute.add
                      playableCharacter
                      CharacterAttribute.Energy
                      -5 ]
        | PerformEnergy.Normal ->
            Character.Attribute.add
                playableCharacter
                CharacterAttribute.Energy
                -3
        | PerformEnergy.Limited ->
            Character.Attribute.add
                playableCharacter
                CharacterAttribute.Energy
                -1
      ScoreRules =
        [ CharacterDrunkenness
          SongPractice(finishedSong)
          SongQuality(finishedSong) ]
      Multipliers =
        [ match energy with
          | PerformEnergy.Energetic when isLongSong -> 25
          | PerformEnergy.Energetic -> 15
          | PerformEnergy.Normal when isLongSong -> 15
          | PerformEnergy.Normal -> 8
          | PerformEnergy.Limited when isLongSong -> 8
          | PerformEnergy.Limited -> 2 ] }
    |> performAction state ongoingConcert


/// Greets the audience and returns the result. If the audience has been greeted
/// already then decreases points.
let greetAudience state ongoingConcert =
    { Event = GreetAudience
      Limit = Penalized(1<times>, PointPenalization(-10))
      Effects = []
      ScoreRules = []
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Adds a give speech event to the list, limiting the amount to 3.
let giveSpeech state ongoingConcert =
    { Event = GiveSpeech
      Limit = NoAction(3<times>)
      Effects = []
      ScoreRules = [ CharacterDrunkenness; CharacterSkill(SkillId.Speech) ]
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

/// Performs a bass solo, which grants up to 5 points depending on the bass
/// skills of the character.
let bassSolo state ongoingConcert =
    { Event = BassSolo
      Limit = Penalized(2<times>, PointPenalization(-5))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Bass) ]
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Performs a drum solo, which grants up to 5 points depending on the drumming
/// skills of the character.
let drumSolo state ongoingConcert =
    { Event = DrumSolo
      Limit = Penalized(2<times>, PointPenalization(-5))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Drums) ]
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Performs a guitar solo, which grants up to 5 points depending on the guitar
/// skills of the character.
let guitarSolo state ongoingConcert =
    { Event = GuitarSolo
      Limit = Penalized(2<times>, PointPenalization(-5))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Guitar) ]
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Makes the crowd sing-along to some chants and songs. Only works if the band
/// is famous enough and the singer is good.
let makeCrowdSing state ongoingConcert =
    { Event = MakeCrowdSing
      Limit = Penalized(2<times>, PointPenalization(-5))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Vocals) ]
      Multipliers = [ 5 ] }
    |> performAction state ongoingConcert

/// Makes the player's sticks spin, granting max 2 points based on their drums
/// skill level.
let spinDrumsticks state ongoingConcert =
    { Event = SpinDrumsticks
      Limit = NoAction(5<times>)
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Drums) ]
      Multipliers = [ 2 ] }
    |> performAction state ongoingConcert

/// Tunes the player's instruments and grants 1 point up to three times.
let tuneInstrument state ongoingConcert =
    { Event = TuneInstrument
      Limit = NoAction(3<times>)
      Effects = []
      ScoreRules = []
      Multipliers = [ 1 ] }
    |> performAction state ongoingConcert
