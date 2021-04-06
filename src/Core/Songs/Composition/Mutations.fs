module Core.Resolvers.Songs.Composition.Mutations

open Entities.State
open Mediator.Mutations.Storage

let composeSong _ mutate input =
  mutate
    (ModifyStateMutation(fun state ->
      let unfinishedSongsByBand =
        Map.tryFind state.Band.Id state.UnfinishedSongs
        |> Option.defaultValue []

      let unfinishedWithSong = unfinishedSongsByBand @ [ input ]

      { state with
          UnfinishedSongs =
            Map.add state.Band.Id unfinishedWithSong state.UnfinishedSongs }))
