module Duets.Data.Items.Gym

open Duets.Entities

module WeightMachines =
    let benchPress: PurchasableItem =
        { Brand = "Bench Press"
          Properties = [ FitnessEquipment ] },
        650m<dd>

    let squatRack: PurchasableItem =
        { Brand = "Power Rack"
          Properties = [ FitnessEquipment ] },
        550m<dd>

    let legPress: PurchasableItem =
        { Brand = "Leg Press"
          Properties = [ FitnessEquipment ] },
        450m<dd>

module Treadmills =
    let treadmill: PurchasableItem =
        { Brand = "Treadmill"
          Properties = [ FitnessEquipment ] },
        550m<dd>

    let elliptical: PurchasableItem =
        { Brand = "Elliptical"
          Properties = [ FitnessEquipment ] },
        450m<dd>

let all =
    [ WeightMachines.benchPress
      WeightMachines.squatRack
      WeightMachines.legPress
      Treadmills.treadmill
      Treadmills.elliptical ]
