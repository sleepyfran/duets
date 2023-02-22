namespace Duets.Entities

[<AutoOpen>]
module NotificationTypes =
    /// Represents a notification related to rentals.
    [<RequireQualifiedAccess>]
    type RentalNotificationType =
        | RentalDueInOneWeek of Rental
        | RentalDueTomorrow of Rental

    /// Represents a notification that needs to be raised to the player.
    [<RequireQualifiedAccess>]
    type Notification =
        | CalendarEvent of CalendarEventType
        | RentalNotification of RentalNotificationType
