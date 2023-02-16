module Test.Common.Generators.Character

open FsCheck

open Duets.Entities

type CharacterGenOptions = { Id: CharacterId option }

let defaultOptions = { Id = None }

let generator (opts: CharacterGenOptions) =
    gen {
        let! character = Arb.generate<Character>
        let id = defaultArg opts.Id character.Id

        return { character with Id = id }
    }
