namespace Entities

[<AutoOpen>]
module CareerTypes =
    /// Defines all the different careers that the game supports.
    type CareerId = | Bartender

    /// Wrapper around a byte that holds the stage number in which a career
    /// is currently in.
    type CareerStageId = CareerStageId of byte
    
    /// Defines one specific stage inside of a career, which defines an ID and
    /// a base salary that will later get multiplied by the type of place that
    /// employees this stage.
    type CareerStage = {
        Id: CareerStageId
        BaseSalaryPerDayMoment: Amount
    }
    
    /// Defines the schedule of a job. Which each option meaning:
    /// - Free: No assigned schedule, player is free to work whenever they feel
    ///   like it.
    [<RequireQualifiedAccess>]
    type JobSchedule = Free

    /// Defines a job that either the character holds or can apply to.
    type Job = {
        Id: CareerId
        CurrentStage: CareerStage
        Location: WorldCoordinates
        Schedule: JobSchedule
    }