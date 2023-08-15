module Duets.Data.Items.Gym

open Duets.Entities

module WeightMachines =
    let benchPress: PurchasableItem =
        { Brand = "Bench Press"
          Type = WeightMachine |> GymEquipment |> Interactive },
        650m<dd>

    let squatRack: PurchasableItem =
        { Brand = "Power Rack"
          Type = WeightMachine |> GymEquipment |> Interactive },
        550m<dd>

    let legPress: PurchasableItem =
        { Brand = "Leg Press"
          Type = WeightMachine |> GymEquipment |> Interactive },
        450m<dd>

module Treadmills =
    let treadmill: PurchasableItem =
        { Brand = "Treadmill"
          Type = Treadmill |> GymEquipment |> Interactive },
        550m<dd>

    let elliptical: PurchasableItem =
        { Brand = "Elliptical"
          Type = Treadmill |> GymEquipment |> Interactive },
        450m<dd>

let all =
    [ WeightMachines.benchPress
      WeightMachines.squatRack
      WeightMachines.legPress
      Treadmills.treadmill
      Treadmills.elliptical ]
