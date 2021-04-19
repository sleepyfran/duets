module Mediator.Registry

type RegistryAction<'Id> =
  | AddHandler of 'Id * (obj -> obj)
  | GetHandler of 'Id * AsyncReplyChannel<(obj -> obj)>
  | RemoveHandler of 'Id

type Registry<'Id>() =
  let registry =
    MailboxProcessor.Start
    <| fun inbox ->
         let rec loop handlers =
           async {
             let! msg = inbox.Receive()

             match msg with
             | AddHandler (id, handler) ->
                 return! loop <| Map.add id handler handlers
             | GetHandler (id, channel) ->
                 channel.Reply(Map.find id handlers)
                 return! loop handlers
             | RemoveHandler id -> return! loop <| Map.remove id handlers
           }

         loop Map.empty

  member this.AddHandler id handler = registry.Post(AddHandler(id, handler))

  member this.GetHandler id =
    registry.PostAndReply(fun channel -> GetHandler(id, channel))

  member this.RemoveHandler id = registry.Post(RemoveHandler(id))
