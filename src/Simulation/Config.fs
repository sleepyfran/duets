/// This module defines all properties that can affect gameplay in one way or
/// another. The default values attempt to give a "real-life-ish" balanced gameplay
/// while preserving the fun, but these can be updated to match your style.
module Simulation.Config

module LifeSimulation =
    module Interactions =
        /// Minimum amount of health needed to perform any interaction.
        let minimumHealthRequired = 10
        
        /// Minimum amount of energy needed to do any non-movement or item
        /// consumption related interaction.
        let minimumEnergyRequired = 10
    
    /// Energy gained per milliliter of coffee ingested.
    let energyPerCoffeeMl = 4
    
    /// Maximum amount of alcohol that the character can take before getting injured.
    let maximumAlcoholAmount = 350

    /// Rate at which the character's drunkenness is reduced each time a unit of
    /// time passes.
    let drunkennessReduceRate = -5

    /// Rate at which the character's health is reduced each time a unit of
    /// time passes when the drunkenness surpasses 85% of the level above.
    let drunkHealthReduceRate = -15
    
    /// Rate at which the character's health is reduced each time a unit of
    /// time passes when the hunger gets below 5.
    let hungerHealthReduceRage = -20

module MusicSimulation =
    /// Amount that will divide the album quality and then multiply the useful
    /// genre market to get the total amount of streams of non-fans of the band.
    /// The bigger the number the smaller the amount of streams a band will get
    /// from non-fans, which is kept as a big number on purpose to slow down the
    /// growth of the band. This is the base cap since it'll be reduced by factors
    /// of 10ths the bigger the fame of the band.
    let baseNonFanStreamCap = 100000000.0 (* 100 million. *)

    /// Indicates the percentage of the crowd of a concert that the band will
    /// lose as fans after getting low points on a concert.
    let concertLowPointFanDecreaseRate = -0.30

    /// Indicates the percentage of the crowd of a concert that the band will
    /// gain as fans after getting medium points on a concert.
    let concertMediumPointFanIncreaseRate = 0.15

    /// Indicates the percentage of the crowd of a concert that the band will
    /// gain as fans after getting good points on a concert.
    let concertGoodPointFanIncreaseRate = 0.25

    /// Indicates the percentage of the crowd of a concert that the band will
    /// gain as fans after getting high points on a concert.
    let concertHighPointFanIncreaseRate = 0.5

    /// Base amount of people that is willing to listen to a genre. When
    /// multiplied with the market point of a genre market, it gives the actual
    /// amount of people interested in a genre in a specific point in time.
    let defaultMarketSize =
        1000000 (* 1 million. *)

    /// Percentage that when multiplied with the daily streams, will yield the
    /// amount of new fans that the band got that day.
    let fanIncreasePercentage = 0.2

    /// Percentage out of the total number of fans that will stream an album or
    /// song daily.
    let fanStreamingPercentage = 0.10

module Revenue =
    /// Indicates how many dd a band makes per stream.
    let revenuePerStream = 0.0055

module Travel =
    /// Price per kilometers for buying plane tickets.
    let pricePerKm = 0.067m
