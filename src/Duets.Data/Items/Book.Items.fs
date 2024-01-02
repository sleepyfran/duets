module Duets.Data.Items.Book

open Duets.Common
open Duets.Data
open Duets.Entities

type DataBook = { Title: string; Author: string }

let private specialBooks =
    [
      (* Music Theory volumes. *)
      { Title = "Music Theory Vol. I"
        Author = "Duets Academy"
        BookEffects = [ SkillGain(SkillId.Composition, 5) ]
        ReadProgress = 0<percent> },
      30m<dd>

      { Title = "Music Theory Vol. II"
        Author = "Duets Academy"
        BookEffects = [ SkillGain(SkillId.Composition, 10) ]
        ReadProgress = 0<percent> },
      60m<dd>

      (* Music production volumes. *)
      { Title = "Music Production Vol. I"
        Author = "Duets Academy"
        BookEffects = [ SkillGain(SkillId.MusicProduction, 3) ]
        ReadProgress = 0<percent> },
      30m<dd>

      { Title = "Music Production Vol. II"
        Author = "Duets Academy"
        BookEffects = [ SkillGain(SkillId.MusicProduction, 8) ]
        ReadProgress = 0<percent> },
      30m<dd> ]

/// Returns all literary books and special books in the game.
let all: PurchasableItem list =
    let dataBooks: DataBook list = ResourceLoader.load Files.Books

    dataBooks
    |> List.map (fun dataBook ->
        { Title = dataBook.Title
          Author = dataBook.Author
          BookEffects = []
          ReadProgress = 0<percent> },
        15m<dd>)
    |> (@) specialBooks
    |> List.map (fun (book, price) ->
        { Brand = book.Author
          Name = book.Title
          Properties = [ Book(book) |> Readable; PlaceableInStorage(Shelf) ] },
        price)
