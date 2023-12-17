module rec Duets.Simulation.Interactions.Actions.Read

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Starts or continues reading a book. If the book is finished, it applies
/// the effects assigned to the book.
let read (item: Item) book state =
    if book.ReadProgress < 100<percent> then
        read' item book state
    else
        []

let private read' (item: Item) book state =
    let updatedReadPercentage =
        book.ReadProgress
        + Config.LifeSimulation.Interactions.readPercentageIncrease
        |> Math.clamp 0<percent> 100<percent>

    let bookUpdateEffect =
        let updatedBook =
            { book with
                ReadProgress = updatedReadPercentage }

        let updatedProperties =
            item.Properties
            |> List.filter (function
                | Readable _ -> true
                | _ -> false)
            |> (@) [ Readable(Book updatedBook) ]

        let updatedItem =
            { item with
                Properties = updatedProperties }

        Diff(item, updatedItem) |> ItemChangedInInventory

    if updatedReadPercentage = 100<percent> then
        bookUpdateEffect :: applyBookEffects book state
    else
        bookUpdateEffect |> List.singleton

let private applyBookEffects book state =
    let character = Queries.Characters.playableCharacter state

    book.BookEffects
    |> List.fold
        (fun effectAcc effect ->
            match effect with
            | MoodletGain(moodlet, expiration) ->
                let currentDate = Queries.Calendar.today state

                Moodlet.create moodlet currentDate expiration
                |> Character.Moodlets.apply state
                |> List.singleton
            | SkillGain(skill, amount) ->
                Skills.Improve.Common.improveCharacterSkills
                    state
                    character
                    [ skill ]
                    amount
            @ effectAcc)
        []
