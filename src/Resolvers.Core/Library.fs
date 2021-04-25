module Resolvers.Core.Setup

open Resolvers.Core

let init () =
  // Initialize all queries
  Queries.CharacterSkillWithLevelQuery.register ()

  // Initialize all mutations.
  Mutations.StartGame.register ()
  Mutations.ComposeSong.register ()
  Mutations.ImproveSong.register ()
