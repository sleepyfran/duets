module Duets.Simulation.Character.Npc

open Duets.Entities
open Duets.Data
open Duets.Simulation

let private randomAdultBirthday state =
    let age = RandomGen.genBetween 18 65
    let dayVariation = RandomGen.genBetween -30 0
    let currentDate = Queries.Calendar.today state

    currentDate
    |> Calendar.Ops.addYears -age
    |> Calendar.Ops.addDays dayVariation

/// Generates a random NPC with a name and a gender from the database and a
/// random birthday between 18 to 65 years ago.
let generateRandom state =
    let name, gender = Npcs.random ()
    let birthday = randomAdultBirthday state

    Character.from name gender birthday
