module Entities.BankAccount

/// Creates a bank account for the given character ID.
let forCharacter id =
    { Holder = Character id
      Transactions = [] }

/// Creates a bank account for the given character ID with an initial transaction
/// of the given balance.
let forCharacterWithBalance id balance =
    { Holder = Character id
      Transactions = [ Incoming balance ] }

/// Creates a bank account for the given band ID.
let forBand id = { Holder = Band id; Transactions = [] }
