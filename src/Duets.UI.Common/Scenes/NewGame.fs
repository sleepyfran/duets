module Duets.UI.Common.Scenes.NewGame

open Duets.Agents
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Setup
open Duets.UI.Common

let private tryParseYear (s: string) : Date option =
    match System.Int32.TryParse s with
    | true, y -> Character.validateBirthday (y * 1<years>) |> Result.toOption
    | _ -> None

let private applyEffects (effects: Effect list) : Scene<unit> =
    fun _ ->
        async {
            effects
            |> List.iter (fun effect ->
                let moreEffects, newState =
                    Simulation.tickOne (State.get ()) effect

                State.set newState
                moreEffects |> Seq.iter Log.appendEffect)
        }

let scene (navigate: Navigate -> unit) : Scene<unit> =
    scene {
        do!
            showText
                "Welcome to Duets, let's start by creating your character and band."

        do! showSep (Some "Your character")
        let! charName = askText "Character's name"
        let! gender = askChoice [ Male; Female; Other ] Text.Character.gender

        let! birthYear =
            askParsed
                "Birth year (e.g. 1990, must be 18+ before game start)"
                tryParseYear

        do! showSep (Some "Your band")
        let! bandName = askText "Band's name"
        let! genre = askChoice Duets.Data.Genres.all id
        let! instrument = askChoice Duets.Data.Roles.all Text.Music.roleName

        do! showSep (Some "Starting city")

        let! city =
            askChoice (Queries.World.allCities) (fun c ->
                Text.World.Cities.name c.Id)

        let character = Character.from charName gender birthYear

        let bandMember =
            Band.Member.from character.Id instrument Calendar.gameBeginning

        let band =
            Band.from bandName genre bandMember Calendar.gameBeginning city.Id

        do! applyEffects [ startGame character band [] city ]

        navigate Navigate.InGame
    }
