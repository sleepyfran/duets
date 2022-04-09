[<AutoOpen>]
module Simulation.Concerts.Live.GiveSpeech

open Common
open Entities
open Simulation
open Simulation.Skills.ImproveSkills.Common

type GiveSpeechResult =
    | LowSpeechSkill
    | MediumSpeechSkill
    | HighSpeechSkill
    | TooManySpeeches

/// Adds a new give speech event to the list and limits the amount that give
/// points to 3.
let giveSpeech state ongoingConcert =
    let character =
        Queries.Characters.playableCharacter state

    let event = CommonEvent GiveSpeech

    let speechesGiven =
        Concert.Ongoing.timesDoneEvent ongoingConcert event

    match speechesGiven with
    | times when times <= 3 ->
        let (_, speechSkillLevel) =
            Queries.Skills.characterSkillWithLevel
                state
                character.Id
                SkillId.Speech

        let skillImprovementEffects =
            applySkillModificationChance state character [ SkillId.Speech ] 2 10

        let points =
            (float speechSkillLevel / 100.0) * 5.0
            |> Math.roundToNearest

        match points with
        | result when result <= 35 -> LowSpeechSkill
        | result when result <= 85 -> MediumSpeechSkill
        | _ -> HighSpeechSkill
        |> Response.forEvent' ongoingConcert event points
        |> Response.addEffects skillImprovementEffects
    | _ -> Response.empty' ongoingConcert TooManySpeeches
