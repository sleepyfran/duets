module Duets.Simulation.RandomGen

type GenFunc = unit -> int
type GenBetweenFunc = int -> int -> int

type RandomGenAgentMessage =
    | Change of System.Random
    | Reset
    | Gen of AsyncReplyChannel<int>
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
                    | Gen channel -> random.Next() |> channel.Reply
                    | GenBetween(min, max, channel) ->
                        random.Next(min, max) |> channel.Reply

                    return! loop random
                }

            loop defaultRandom

    member this.Change genFunc = genFunc |> agent.Post
    member this.Reset() = Reset |> agent.Post
    member this.Gen() = agent.PostAndReply Gen

    member this.GenBetween min max =
        agent.PostAndReply(fun channel -> GenBetween(min, max, channel))

let private randomGenAgent = RandomGenAgent()

let change impl = Change impl |> randomGenAgent.Change

let reset = randomGenAgent.Reset

let gen = randomGenAgent.Gen

let genBetween = randomGenAgent.GenBetween

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
