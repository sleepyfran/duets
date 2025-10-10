module Duets.Data.Items.Vehicles

open Duets.Entities

module Car =
    (* == Budget cars. == *)
    let toyotaCorolla: PurchasableItem =
        { Brand = "Toyota"
          Name = "Corolla"
          Properties = [ Rideable Car ] },
        20000m<dd>

    let hondaCivic: PurchasableItem =
        { Brand = "Honda"
          Name = "Civic"
          Properties = [ Rideable Car ] },
        25000m<dd>

    let fordFocus: PurchasableItem =
        { Brand = "Ford"
          Name = "Focus"
          Properties = [ Rideable Car ] },
        23000m<dd>

    let volkswagenGolf: PurchasableItem =
        { Brand = "Volkswagen"
          Name = "Golf"
          Properties = [ Rideable Car ] },
        26000m<dd>

    let budget: PurchasableItem list =
        [ toyotaCorolla; fordFocus; hondaCivic; volkswagenGolf ]

    (* == Medium-range cars. == *)

    let mazdaCX5: PurchasableItem =
        { Brand = "Mazda"
          Name = "CX-5"
          Properties = [ Rideable Car ] },
        37000m<dd>

    let bmw3Series: PurchasableItem =
        { Brand = "BMW"
          Name = "3 Series"
          Properties = [ Rideable Car ] },
        55000m<dd>

    let audiA4: PurchasableItem =
        { Brand = "Audi"
          Name = "A4"
          Properties = [ Rideable Car ] },
        56000m<dd>

    let mercedesBenzCClass: PurchasableItem =
        { Brand = "Mercedes-Benz"
          Name = "C-Class"
          Properties = [ Rideable Car ] },
        60000m<dd>

    let lexusES: PurchasableItem =
        { Brand = "Lexus"
          Name = "ES"
          Properties = [ Rideable Car ] },
        43000m<dd>

    let volvoS60: PurchasableItem =
        { Brand = "Volvo"
          Name = "S60"
          Properties = [ Rideable Car ] },
        46000m<dd>

    let midRange: PurchasableItem list =
        [ mazdaCX5; bmw3Series; audiA4; mercedesBenzCClass; lexusES; volvoS60 ]

    (* == Premium cars. == *)

    let porscheTaycan: PurchasableItem =
        { Brand = "Porsche"
          Name = "Taycan"
          Properties = [ Rideable Car ] },
        160000m<dd>

    let porsche911: PurchasableItem =
        { Brand = "Porsche"
          Name = "911"
          Properties = [ Rideable Car ] },
        145000m<dd>

    let jaguarFType: PurchasableItem =
        { Brand = "Jaguar"
          Name = "F-Type"
          Properties = [ Rideable Car ] },
        135000m<dd>

    let mercedesBenzSClass: PurchasableItem =
        { Brand = "Mercedes-Benz"
          Name = "S-Class"
          Properties = [ Rideable Car ] },
        120000m<dd>

    let audiR8: PurchasableItem =
        { Brand = "Audi"
          Name = "R8"
          Properties = [ Rideable Car ] },
        155000m<dd>

    let bmw7Series: PurchasableItem =
        { Brand = "BMW"
          Name = "7 Series"
          Properties = [ Rideable Car ] },
        108000m<dd>

    let premium: PurchasableItem list =
        [ porsche911
          porscheTaycan
          jaguarFType
          mercedesBenzSClass
          audiR8
          bmw7Series ]

module Metro =
    let metroTrain: Item =
        { Brand = "Metro"
          Name = "Metro train"
          Properties = [ Rideable Metro ] }
