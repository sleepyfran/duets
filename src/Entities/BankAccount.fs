module Entities.BankAccount

/// Creates a bank account for the given character ID.
let forCharacter (id: CharacterId) =
    { Holder = Character id
      Transactions = [] }

/// Creates a bank account for the given band ID.
let forBand (id: BandId) = { Holder = Band id; Transactions = [] }
