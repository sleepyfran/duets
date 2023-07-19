module Duets.Entities.Rental

/// Retrieves the due date from the rental
let dueDate rental =
    match rental.RentalType with
    | Monthly nextPaymentDate
    | OneTime(_, nextPaymentDate) -> nextPaymentDate
