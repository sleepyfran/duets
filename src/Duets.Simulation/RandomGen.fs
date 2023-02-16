module Duets.Simulation.RandomGen

type GenFunc = unit -> int
type GenBetweenFunc = int -> int -> int

type RandomGenAgentMessage =
    | Change of System.Random
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
                    | Gen channel -> random.Next() |> channel.Reply
                    | GenBetween (min, max, channel) ->
                        random.Next(min, max) |> channel.Reply

                    return! loop random
                }

            loop defaultRandom

    member this.Change genFunc = genFunc |> agent.Post
    member this.Gen = agent.PostAndReply Gen

    member this.GenBetween min max =
        agent.PostAndReply(fun channel -> GenBetween(min, max, channel))

let private randomGenAgent = RandomGenAgent()

let change impl = Change impl |> randomGenAgent.Change

let gen = randomGenAgent.Gen

let genBetween = randomGenAgent.GenBetween
