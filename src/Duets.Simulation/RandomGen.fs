module Duets.Simulation.RandomGen

open Duets.Common

type GenFunc = unit -> int
type GenBetweenFunc = int -> int -> int

type RandomGenAgentMessage =
    | Change of System.Random
    | Reset
    | Gen of AsyncReplyChannel<int>
    | GenDouble of AsyncReplyChannel<double>
    | GenBetween of min: int * max: int * channel: AsyncReplyChannel<int>

/// Agent that encapsulates a random number generator to not have to pass a
/// function parameter to every part of the Simulation assembly using this
/// and allow to easy mocking during testing. An overblown solution? Maybe, but
/// it might evolve into an agent holding a DI framework if the situation
/// requires so in the future :)
type private RandomGenAgent() =
    let defaultRandom = System.Random()

    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop random =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Change r -> return! loop r
                    | Reset -> return! loop defaultRandom
                    | Gen channel ->
                        random.Next() |> channel.Reply
                        return! loop random
                    | GenDouble channel ->
                        random.NextDouble() |> channel.Reply
                        return! loop random
                    | GenBetween(min, max, channel) ->
                        random.Next(min, max) |> channel.Reply
                        return! loop random
                }

            loop defaultRandom

    member this.Change genFunc = genFunc |> agent.Post
    member this.Reset() = Reset |> agent.Post
    member this.Gen() = agent.PostAndReply Gen

    member this.GenDouble() = agent.PostAndReply GenDouble

    member this.GenBetween min max =
        agent.PostAndReply(fun channel -> GenBetween(min, max, channel))

let private randomGenAgent = RandomGenAgent()

let change impl = Change impl |> randomGenAgent.Change

let reset = randomGenAgent.Reset

let gen = randomGenAgent.Gen

let genBetween = randomGenAgent.GenBetween

let genDouble = randomGenAgent.GenDouble

/// Generates a random number between 0 and 100 and returns true if it is
/// less than or equal to the given amount.
let chance amount =
    let random = genBetween 0 100
    random <= amount

let sampleIndex list =
    let maxIndex = List.length list - 1
    let maxIndex = if maxIndex < 0 then 0 else maxIndex
    genBetween 0 maxIndex

let choice choices = List.item (sampleIndex choices) choices

let tryChoice choices =
    List.tryItem (sampleIndex choices) choices

/// Distributes a total amount of items into a list of items so that
/// the sum of the items is equal to the total. The items are distributed
/// randomly.
let distribute (total: int<_>) (items: 'a list) : Map<'a, int> =
    // Generate random weights and normalize them.
    let weights = items |> List.map (fun _ -> genDouble ())
    let totalWeight = List.sum weights
    let normalizedWeights = weights |> List.map (fun w -> w / totalWeight)

    // Distribute the items based on the normalized weights.
    let distributedItems =
        normalizedWeights
        |> List.map (fun w -> float total * w |> Math.ceilToNearest)

    List.zip items distributedItems |> Map.ofList

/// Selects a random element from a list of choices, where each choice has an
/// associated weight, and the probability of selecting each choice is
/// proportional to its weight. If the total weight is zero or negative, returns None.
let weightedRandomChoice (choices: ('T * float) list) : 'T option =
    let totalWeight = choices |> List.sumBy snd

    if totalWeight <= 0.0 then
        None
    else
        let rand = genDouble () * totalWeight

        let rec pick prevCumulative =
            function
            | [] -> None
            | (value, weight) :: tail ->
                let cumulative = prevCumulative + weight

                if rand <= cumulative then
                    Some value
                else
                    pick cumulative tail

        pick 0.0 choices
