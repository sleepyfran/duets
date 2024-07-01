module Duets.Agents.State

open Duets.Entities

type Handler = State -> unit

type StateMessage =
    | Get of AsyncReplyChannel<State>
    | Set of State
    | Subscribe of id: Identity * handler: Handler
    | Unsubscribe of id: Identity

type StateAgent() =
    let state =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop (state, listeners) =
                async {
                    let! msg = inbox.Receive()

                    let notifyAll value =
                        listeners |> Map.iter (fun _ handler -> handler value)

                    match msg with
                    | Get channel ->
                        channel.Reply state
                        return! loop (state, listeners)
                    | Set value ->
                        notifyAll value
                        return! loop (value, listeners)
                    | Subscribe(id, handler) ->
                        let updated = listeners |> Map.add id handler

                        handler state
                        return! loop (state, updated)
                    | Unsubscribe id ->
                        let updated = listeners |> Map.remove id
                        return! loop (state, updated)
                }

            loop (State.empty, Map.empty)

    member this.Agent = state

let private staticAgent = StateAgent()

type private Subscription(id: Identity) =
    interface System.IDisposable with
        member this.Dispose() =
            Unsubscribe id |> staticAgent.Agent.Post

/// Returns the state of the game.
let get () = staticAgent.Agent.PostAndReply Get

/// Sets the state of the game.
let set value = Set value |> staticAgent.Agent.Post

/// Subscribes to changes on the state and returns the ID that identifies the
/// subscription.
let subscribe handler : System.IDisposable =
    let id = Identity.create ()
    Subscribe(id, handler) |> staticAgent.Agent.Post
    new Subscription(id)
