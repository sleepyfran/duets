module Duets.Simulation.Tests.Interactions.Read

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Data
open Duets.Simulation.Interactions

let private read item =
    Items.interact dummyState item InteractiveItemInteraction.Read

let private createBookItem readProgress =
    { Brand = "Book"
      Type =
        { Title = "Random Book"
          Author = "Random Author"
          BookEffects =
            [ SkillGain(SkillId.Barista, 10)
              MoodletGain(MoodletType.JetLagged, MoodletExpirationTime.Never) ]
          ReadProgress = readProgress }
        |> InteractiveItemType.Book
        |> ItemType.Interactive }

let private unwrapBook (item: Item) =
    match item.Type with
    | Interactive(InteractiveItemType.Book book) -> book
    | _ -> failwith "Unexpected item type"

[<Test>]
let ``reading something else than a book returns an error`` () =
    (fst Items.Drink.Beer.alhambra)
    |> read
    |> Result.unwrapError
    |> should be (ofCase <@ Items.ActionNotPossible @>)

[<Test>]
let ``reading updates the inventory item`` () =
    createBookItem 0<percent>
    |> read
    |> Result.unwrap
    |> List.filter (function
        | ItemChangedInInventory _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``reading adds 20% progress when read`` () =
    let updateEffect =
        createBookItem 0<percent>
        |> read
        |> Result.unwrap
        |> List.item 1 (* Head is TimeAdvanced *)

    match updateEffect with
    | ItemChangedInInventory(Diff(prev, curr)) ->
        let prevBook = unwrapBook prev
        let currBook = unwrapBook curr

        prevBook.ReadProgress |> should equal 0<percent>
        currBook.ReadProgress |> should equal 20<percent>
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``reading applies the book's effect when finishing it`` () =
    createBookItem 80<percent>
    |> read
    |> Result.unwrap
    |> List.filter (function
        | SkillImproved(_, Diff((_, prevLevel), (_, currLevel))) when
            prevLevel < currLevel
            ->
            true
        | CharacterMoodletsChanged _ -> true
        | _ -> false)
    |> should haveLength 2

[<Test>]
let ``reading on an already finished book does not re-apply effects`` () =
    createBookItem 100<percent>
    |> read
    |> Result.unwrap
    |> List.filter (function
        | SkillImproved(_, Diff((_, prevLevel), (_, currLevel))) when
            prevLevel < currLevel
            ->
            true
        | _ -> false)
    |> should haveLength 0
