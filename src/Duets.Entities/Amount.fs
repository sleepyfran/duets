module Duets.Entities.Amount

/// Creates an amount from a decimal value.
let fromDecimal (amount: decimal) : Amount = amount * 1m<dd>

/// Creates an amount from a float value.
let fromFloat (amount: float) : Amount = amount |> decimal |> fromDecimal

/// Concerts an amount to a decimal value.
let toDecimal (amount: Amount) : decimal = amount / 1m<dd>
