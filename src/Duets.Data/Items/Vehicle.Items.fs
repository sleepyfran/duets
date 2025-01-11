module Duets.Data.Items.Vehicles

open Duets.Entities

module Metro =
    let metroTrain: Item =
        { Brand = "Metro"
          Name = "Metro train"
          Properties = [ Rideable Metro ] }
