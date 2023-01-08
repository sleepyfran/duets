namespace Entities

[<AutoOpen>]
module CareerTypes =
    /// Defines all the different careers that the game supports.
    type CareerId =
        | Barista
        | Bartender

    /// Wrapper around a byte that holds the stage number in which a career
    /// is currently in.
    type CareerStageId = CareerStageId of byte

    /// Defines one specific stage inside of a career, which defines an ID and
    /// a base salary that will later get multiplied by the type of place that
    /// employees this stage.
    type CareerStage =
        { Id: CareerStageId
          BaseSalaryPerDayMoment: Amount }

    /// Number of day moments that it takes to perform a shift in a job.
    [<RequireQualifiedAccess>]
    type ShiftDuration = int<dayMoments>

    /// Defines the schedule of a job. Which each option meaning:
    /// - Free: No assigned schedule, player is free to work whenever they feel
    ///   like it.
    [<RequireQualifiedAccess>]
    type JobSchedule = Free of duration: ShiftDuration

    /// Defines the effect that performing a job's shift has on the character's
    /// attribute.
    type JobShiftAttributeEffect = CharacterAttribute * int

    /// Defines a job that either the character holds or can apply to.
    type Job =
        { Id: CareerId
          CurrentStage: CareerStage
          Location: WorldCoordinates
          Schedule: JobSchedule
          ShiftAttributeEffect: JobShiftAttributeEffect list }
