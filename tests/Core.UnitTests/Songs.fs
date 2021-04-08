module Core.UnitTests.Resolvers.Songs

open Expecto
open Mediator.Mutations.Songs
open Core.Resolvers.Songs.Composition.Mutations
open Entities.State

let mockMutateWith expect = fun (state: State) -> expect state

test "Compose song creates an unfinished song with the given input"
<| fun () ->
     let inputSong =
       { Name = "Test Song"
         Length = 100
         VocalStyle = "test" }

     let expectFn state =
       let songs = Map.find  state.UnfinishedSongs
       Expect.equal songs.Length 0

     let mutate = mockMutateWith expectFn
     composeSong 1 mutate inputSong
