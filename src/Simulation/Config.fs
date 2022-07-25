/// This module defines all properties that can affect gameplay in one way or
/// another. The default values attempt to give a "real-life-ish" balanced gameplay
/// while preserving the fun, but these can be updated to match your style.
module Simulation.Config

module LifeSimulation =
    /// Maximum amount of alcohol that the character can take before getting injured.
    let maximumAlcoholAmount = 500

module Revenue =
    /// Indicates how many dd a band makes per stream.
    let revenuePerStream = 0.5
