module rec Duets.Simulation.Bands.Generation

open Aether
open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands

type FameLevel =
    | Low
    | Medium
    | High

module Name =
    /// Generates a random name for a band, which gives a 33% chance to three
    /// different types of names:
    /// - Adverb + Adjective, something like Abnormally Waterproof
    /// - Adjective + Noun, something like Wheeled Hearts
    /// - The + Noun, something like The License
    let generate () =
        let chance n = RandomGen.genBetween 0 100 > n

        if chance 33 then
            (* Will generate something like Abnormally Waterproof. *)
            $"""{Words.adverbs |> RandomGen.choice} {Words.adjectives |> RandomGen.choice}"""
        elif chance 33 then
            (* Will generate something like Wheeled Heads. *)
            $"""{Words.adjectives |> RandomGen.choice} {Words.nouns |> RandomGen.choice}"""
        else
            (* Will generate something like The License. *)
            $"""the {Words.nouns |> RandomGen.choice}"""
        |> String.titleCase

module Fans =
    /// Generates a random number of fans given the fan level.
    /// - For low fame level, between 0.01 and 0.1 of the genre's market
    /// - For medium fame level, between 0.1 and 0.6 of the genre's market
    /// - For high fame level, between 0.6 and 1.0 of the genre's market
    let generate state fameLevel genre =
        let marketCap =
            match fameLevel with
            | Low -> 0.01, 0.1
            | Medium -> 0.1, 0.6
            | High -> 0.6, 1.0

        let usefulMarket = Queries.GenreMarkets.usefulMarketOf state genre

        let upperBound = usefulMarket * (fst marketCap)
        let lowerBound = usefulMarket * (snd marketCap)

        RandomGen.choice [ upperBound; lowerBound ] |> Math.ceilToNearest

module Members =
    /// Generate a list of members of a band based on the usual roles that a
    /// band of this genre has. Randomly generates their information, age
    /// and skill based on the band's start date and the fame level.
    let generate state (bandStartDate: Date) genre fameLevel =
        let currentDate = Queries.Calendar.today state
        let averageBirthdayYear = bandStartDate.Year - 10
        let averageAge = currentDate.Year - averageBirthdayYear

        let averageSkills =
            match fameLevel with
            | Low -> RandomGen.genBetween 10 50
            | Medium -> RandomGen.genBetween 51 70
            | High -> RandomGen.genBetween 71 100

        Roles.forGenre genre
        |> List.map (
            Members.createMemberForHire
                bandStartDate
                averageSkills
                averageAge
                genre
        )

module StartDate =
    /// Generates a date when a band should start playing by randomly adding
    /// between 0 to -30 years to the current date.
    let generate state =
        let startDateYearDifference = RandomGen.genBetween -30 0

        Queries.Calendar.today state
        |> Calendar.Ops.addYears startDateYearDifference

/// Generates a band with a random start date from 30 years from the current
/// time to today, a random name, random genre, random number of fans based
/// on the provided fame level and random members that match the genre. Returns
/// a list of members that compose the band and the band itself. The members
/// are returned so that they can be added to the state itself, since otherwise
/// the band would reference non-existent characters. Same thing for their skills.
let generate state fameLevel =
    let bandStartDate = StartDate.generate state
    let bandName = Name.generate ()
    let genre = Genres.all |> RandomGen.choice
    let bandFans = Fans.generate state fameLevel genre
    let members = Members.generate state bandStartDate genre fameLevel

    let initialMember =
        members |> List.head |> Band.Member.fromMemberForHire bandStartDate

    let band =
        Band.from bandName genre initialMember bandStartDate
        |> Optic.set Lenses.Band.fans_ bandFans
        |> Optic.set
            Lenses.Band.members_
            (members |> List.map (Band.Member.fromMemberForHire bandStartDate))

    members, band

/// Generates a hundred bands from 30 years prior the start of the game to
/// the current date. Out of that 100, 50 will be not famous (as in having less
/// than 10% of their correspondent market size), 45 will be moderately famous
/// (having between 10 to 60 percent of their market size) and the remaining 5
/// will be really famous (more than 60 percent of their market size).
///
/// All these bands are later added to the state, as well as all the characters
/// belonging to these bands.
let addInitialBandsToState state =
    addInitialBandsToState' 50 Low state
    |> addInitialBandsToState' 45 Medium
    |> addInitialBandsToState' 5 High

let private addInitialBandsToState' n fameLevel state =
    [ 1..n ]
    |> List.fold (fun acc _ -> addSimulatedBandToState acc fameLevel) state

let private addSimulatedBandToState state fameLevel =
    let members, band = generate state fameLevel

    State.Bands.addSimulated band state
    |> List.fold'
        (fun acc (mem: MemberForHire) ->
            State.Characters.add mem.Character acc
            |> State.Skills.addMultiple mem.Character.Id mem.Skills)
        members
