module Storage.State

open Entities.State

type StateMessage =
  | Get of AsyncReplyChannel<State>
  | Set of State

type StateAgent() =
  let state =
    MailboxProcessor.Start
    <| fun inbox ->
         let rec loop state =
           async {
             let! msg = inbox.Receive()

             match msg with
             | Get channel ->
                 channel.Reply state
                 return! loop state
             | Set value -> return! loop value
           }

         loop empty

  member this.Get() = state.PostAndReply Get

  member this.Set value = Set value |> state.Post

let staticAgent = StateAgent()

/// Returns the state of the game.
let getState = staticAgent.Get

/// Sets the state of the game to a given value.
let setState = staticAgent.Set

/// Passes the state of the game to the modify function and sets the state to
/// the return value.
let modifyState modify = getState () |> modify |> setState
