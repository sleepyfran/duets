namespace Duets.Entities

[<AutoOpen>]
module NotificationTypes =
    /// Represents a notification related to rentals.
    [<RequireQualifiedAccess>]
    type RentalNotificationType =
        | RentalDueInOneWeek of Rental
        | RentalDueTomorrow of Rental

    /// Represents the type of delivery that is being notified.
    [<RequireQualifiedAccess>]
    type DeliveryType = Merchandise

    /// Represents a notification that needs to be raised to the player.
    [<RequireQualifiedAccess>]
    type Notification =
        | CalendarEvent of CalendarEventType
        | DeliveryArrived of CityId * PlaceId * DeliveryType
        | RentalNotification of RentalNotificationType

    /// Defines all notifications that have to be raised at a certain date and
    /// day moment. Dates should have their time erased so that they're easily
    /// comparable.
    type Notifications = Map<Date, Map<DayMoment, Notification list>>
