namespace Duets.Entities

[<AutoOpen>]
module MetroTypes =
    /// Defines all colors that can be used to represent a metro line in a
    /// simplified metro network for a city.
    type MetroLineId =
        | Red
        | Blue

    /// Defines the next and previous stations that connect to the current
    /// station, if any.
    type MetroStationConnection =
        | OnlyNext of ZoneId
        | OnlyPrevious of ZoneId
        | PreviousAndNext of previous: ZoneId * next: ZoneId

    /// Defines a metro line in a city, which consists of a color and a list of
    /// station coordinates where the metro line stops.
    type MetroLine =
        {
            Id: MetroLineId
            /// Map of stations that are part of the metro line, with the key
            /// being the coordinates of the station and the value being the
            /// connection to the next and previous stations, if any on either
            /// side.
            Stations: Map<ZoneId, MetroStationConnection>
            /// The usual waiting time for a train to arrive at the station. This
            /// is just a base value that will be modified by the time of the day
            /// so that the waiting times are more realistic.
            UsualWaitingTime: int<minute>
        }

    /// Defines a metro station in a city, which holds the line that the station
    /// belongs to and which street coordinates the station leaves to.
    type MetroStation =
        { Lines: MetroLineId list
          LeavesToStreet: StreetId
          PlaceId: PlaceId }
