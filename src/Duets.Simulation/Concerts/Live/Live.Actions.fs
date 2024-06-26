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

        let timeEffects =
            Config.MusicSimulation.Merch.soundcheckTime
            |> advanceDayMoment' state

        [ Situations.preparingConcert' updatedChecklist; yield! timeEffects ]


/// Sets up the merch stand, which improves the ticket sales of the concert.
let setupMerchStand state checklist =
    if checklist.MerchStandSetup then
        (* Technically the interaction should be disabled, but let's do nothing. *)
        []
    else
        let updatedChecklist =
            { checklist with
                MerchStandSetup = true }

        let timeEffects =
            Config.MusicSimulation.Merch.standSetupTime
            |> advanceDayMoment' state

        [ Situations.preparingConcert' updatedChecklist; yield! timeEffects ]

/// Starts the given concert if the band is ready to play.
let startConcert state band scheduledConcert =
    let concert = Concert.fromScheduled scheduledConcert
    let situation = Queries.Situations.current state

    match situation with
    | Concert(InConcert _) ->
        [] (* Concert already started, no need to do anything. *)
    | Concert(Preparing checklist) ->
        let initialPoints =
            if checklist.SoundcheckDone then 5<quality> else 0<quality>

        let attendancePercentage = Queries.Concerts.attendancePercentage concert

        [ Situations.inConcert
              { Events = []
                Points = initialPoints
                Checklist = checklist
                Concert = concert }
          ConcertStarted(band, concert, attendancePercentage) ]
    | _ -> [] (* Band hasn't started preparing, can't start concert. *)

let private playSong' state ongoingConcert finishedSong energy =
    let (Finished(song, quality)) = finishedSong
    let isLongSong = song.Length.Minutes > 10<minute>

    let playableCharacter = Queries.Characters.playableCharacter state

    { Event = PlaySong(finishedSong, energy)
      Limit = Penalized(1<times>, PointPenalization(-50<points>))
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

/// Plays the given song in the concert with the specified energy. The result
/// depends on whether the song was already played or not and the energy.
let playSong state ongoingConcert song energy =
    playSong' state ongoingConcert song energy |> toEffects state ongoingConcert

/// Greets the audience and returns the result. If the audience has been greeted
/// already then decreases points.
let greetAudience state ongoingConcert =
    { Event = GreetAudience
      Limit = Penalized(1<times>, PointPenalization(-10<points>))
      Effects = []
      ScoreRules = []
      Multipliers = [ 5 ] }
    |> toEffects state ongoingConcert

/// Adds a give speech event to the list, limiting the amount to 3.
let giveSpeech state ongoingConcert =
    { Event = GiveSpeech
      Limit = NoAction(3<times>)
      Effects = []
      ScoreRules = [ CharacterDrunkenness; CharacterSkill(SkillId.Speech) ]
      Multipliers = [ 5 ] }
    |> toEffects state ongoingConcert

/// Performs the given song adding a dedication, which grants 10 additional points
/// on the performance only if done less than 2 times.
let dedicateSong state ongoingConcert finishedSong energy =
    let song = Song.fromFinished finishedSong
    let dedicateSongEvent = DedicateSong(finishedSong, energy)

    let dedicationsGiven =
        Concert.Ongoing.timesDoneEvent ongoingConcert dedicateSongEvent

    match dedicationsGiven with
    | times when times < 2<times> ->
        let song = Song.fromFinished finishedSong
        let playSongAction = playSong' state ongoingConcert finishedSong energy

        let result, points =
            { playSongAction with
                Multipliers = 10 :: playSongAction.Multipliers }
            |> performAction state ongoingConcert

        let updatedConcert =
            ongoingConcert
            |> addEvent playSongAction.Event
            |> addEvent dedicateSongEvent
            |> addPoints points

        ConcertActionPerformed(
            PlaySong(finishedSong, energy),
            updatedConcert,
            result,
            points
        )
        :: playSongAction.Effects
    | _ ->
        [ ConcertActionPerformed(
              dedicateSongEvent,
              ongoingConcert,
              TooManyRepetitionsNotDone,
              0<points>
          ) ]

/// Performs a bass solo, which grants up to 5 points depending on the bass
/// skills of the character.
let bassSolo state ongoingConcert =
    { Event = BassSolo
      Limit = Penalized(2<times>, PointPenalization(-5<points>))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Bass) ]
      Multipliers = [ 5 ] }
    |> toEffects state ongoingConcert

/// Performs a drum solo, which grants up to 5 points depending on the drumming
/// skills of the character.
let drumSolo state ongoingConcert =
    { Event = DrumSolo
      Limit = Penalized(2<times>, PointPenalization(-5<points>))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Drums) ]
      Multipliers = [ 5 ] }
    |> toEffects state ongoingConcert

/// Performs a guitar solo, which grants up to 5 points depending on the guitar
/// skills of the character.
let guitarSolo state ongoingConcert =
    { Event = GuitarSolo
      Limit = Penalized(2<times>, PointPenalization(-5<points>))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Guitar) ]
      Multipliers = [ 5 ] }
    |> toEffects state ongoingConcert

/// Makes the crowd sing-along to some chants and songs. Only works if the band
/// is famous enough and the singer is good.
let makeCrowdSing state ongoingConcert =
    { Event = MakeCrowdSing
      Limit = Penalized(2<times>, PointPenalization(-5<points>))
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Vocals) ]
      Multipliers = [ 5 ] }
    |> toEffects state ongoingConcert

/// Makes the player's sticks spin, granting max 2 points based on their drums
/// skill level.
let spinDrumsticks state ongoingConcert =
    { Event = SpinDrumsticks
      Limit = NoAction(5<times>)
      Effects = []
      ScoreRules =
        [ CharacterDrunkenness; CharacterSkill(SkillId.Instrument Drums) ]
      Multipliers = [ 2 ] }
    |> toEffects state ongoingConcert

/// Tunes the player's instruments and grants 1 point up to three times.
let tuneInstrument state ongoingConcert =
    { Event = TuneInstrument
      Limit = NoAction(3<times>)
      Effects = []
      ScoreRules = []
      Multipliers = [ 1 ] }
    |> toEffects state ongoingConcert

/// Makes the character face the band, grants no points.
let faceBand state ongoingConcert =
    { Event = TuneInstrument
      Limit = NoLimit
      Effects = []
      ScoreRules = []
      Multipliers = [ 0 ] }
    |> toEffects state ongoingConcert

/// Takes the mic and starts singing, grants no points.
let takeMic state ongoingConcert =
    { Event = TakeMic
      Limit = NoLimit
      Effects = []
      ScoreRules = []
      Multipliers = [ 0 ] }
    |> toEffects state ongoingConcert

/// Puts the mic on the stand, grants no points.
let putMicOnStand state ongoingConcert =
    { Event = PutMicOnStand
      Limit = NoLimit
      Effects = []
      ScoreRules = []
      Multipliers = [ 0 ] }
    |> toEffects state ongoingConcert

/// Adjusts the drums, grants no points.
let adjustDrums state ongoingConcert =
    { Event = AdjustDrums
      Limit = NoLimit
      Effects = []
      ScoreRules = []
      Multipliers = [ 0 ] }
    |> toEffects state ongoingConcert
