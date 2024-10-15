/// This module defines all properties that can affect gameplay in one way or
/// another. The default values attempt to give a "real-life-ish" balanced gameplay
/// while preserving the fun, but these can be updated to match your style.
module Duets.Simulation.Config

open Duets.Entities

module LifeSimulation =
    module Interactions =
        /// Minimum amount of health needed to perform any interaction.
        let minimumHealthRequired = 10

        /// Minimum amount of energy needed to do any non-movement or item
        /// consumption related interaction.
        let minimumEnergyRequired = 10

        /// Step by which the percentage of a book read is increased when the
        /// character reads a book.
        let readPercentageIncrease = 20<percent>

    module Energy =
        /// How much energy is increased when the character exercises.
        let exerciseIncrease = -25

    module Health =
        /// How much health is recovered when the character exercises.
        let exerciseIncrease = 8

    module Mood =
        /// How much the character's mood is improved when drinking alcohol.
        let alcoholIncrease = 3

        /// How much the character's mood is improved when drinking coffee.
        let coffeeIncrease = 1

        /// How much the character's mood changes after failing a concert.
        let concertFailIncrease = -20

        /// How much the character's mood changes after playing a bad concert.
        let concertPoorResultIncrease = -10

        /// How much the character's mood is improved after playing an okay concert.
        let concertNormalResultIncrease = 5

        /// How much the character's mood is improved after playing a good concert.
        let concertGoodResultIncrease = 10

        /// How much the character's mood is improve when playing video games.
        let playingVideoGamesIncrease = 10

        /// How much the character's mood is improved when reading a book.
        let readingBookIncrease = 5

        /// How much the character's mood is improved when winning a non-interactive game.
        let winningNonInteractiveGameIncrease = 10

        /// How much the character's mood is reduced when losing a non-interactive game.
        let losingNonInteractiveGameIncrease = -10

        /// How much the character's mood is improved when playing music.
        let playingMusicIncrease = 2

        /// How much the character's mood is improved when watching TV.
        let watchingTvIncrease = 5

    /// Rate at which the character's energy is reduced each time a unit of time
    /// passes.
    let energyReductionRate = -20

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

    /// Rate at which the character's hunger is reduced each time a unit of
    /// time passes.
    let hungerReductionRate = -15

    /// Rate at which the character's health is reduced each time a unit of
    /// time passes when the hunger gets below 5.
    let hungerHealthReduceRate = -20

module Moodlets =
    module NotInspired =
        /// Number of days that need to pass between composing songs for the character
        /// not to get a NotInspired moodlet.
        let daysBetweenSongsToSlowDown = 7<days>

        /// Defines the amount by which any song composition scores will be
        /// reduced when the character is not inspired.
        let songCompositionReduction = 0.25

module MusicSimulation =
    /// Minimum number of fans that a band has to have in order to produce
    /// reviews after releasing an album.
    let minimumFanBaseForReviews = 2000

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
    let defaultMarketSize = 1000000 (* 1 million. *)

    /// Percentage that when multiplied with the daily streams, will yield the
    /// amount of new fans that the band got that day.
    let fanIncreasePercentage = 0.2

    /// Percentage out of the total number of fans that will stream an album or
    /// song daily.
    let fanStreamingPercentage = 0.10

    module Merch =
        /// Time it takes to do a soundcheck.
        let soundcheckTime = 1<dayMoments>

        /// Time it takes to set-up a merch stand.
        let standSetupTime = 1<dayMoments>

        /// Price of ordering one CD.
        let cdPrice = 10m<dd>

        /// Price of ordering one vinyl.
        let vinylPrice = 20m<dd>

        /// Price of ordering a t-shirt.
        let tShirtPrice = 10m<dd>

        /// Price of ordering a hoodie.
        let hoodiePrice = 20m<dd>

        /// Price of ordering a tote bag.
        let toteBagPrice = 5m<dd>

        /// Minimum amount of physical media orders that a band needs to order
        /// via a merchandise workshop.
        let minimumPhysicalMediaOrders = 50<quantity>

        /// Maximum amount of physical media orders that a band can order via
        /// a merchandise workshop.
        let maximumPhysicalMediaOrders = 1000<quantity>

        /// Minimum amount of wearable orders that a band needs to order via a
        /// merchandise workshop.
        let minimumWearableOrders = 100<quantity>

        /// Maximum amount of wearable orders that a band can order via a
        /// merchandise workshop.
        let maximumWearableOrders = 2000<quantity>

module Population =
    /// The chance that the generated population for a given place will be known
    /// by the character instead of randomly generated.
    let chanceOfKnownPeopleAtPlace = 5 (* 5% *)

module Revenue =
    /// Indicates how many dd a band makes per stream.
    let revenuePerStream = 0.0055

module Time =
    /// Number of minutes that a day moment has, which is effectively the
    /// number of minutes we allow the player to perform per turn.
    let minutesPerDayMoment = 180<minute>

module Travel =
    /// Price per kilometers for buying plane tickets.
    let pricePerKm = 0.067m
