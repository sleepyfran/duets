module Duets.Data.Items.Vehicles

open Duets.Entities

module Car =
    (* == Budget cars. == *)
    let toyotaCorolla: PurchasableItem =
        { Brand = "Toyota"
          Name = "Corolla"
          Properties = [ Rideable(Car { Power = 169<horsepower> }) ] },
        20000m<dd>

    let hondaCivic: PurchasableItem =
        { Brand = "Honda"
          Name = "Civic"
          Properties = [ Rideable(Car { Power = 180<horsepower> }) ] },
        25000m<dd>

    let fordFocus: PurchasableItem =
        { Brand = "Ford"
          Name = "Focus"
          Properties = [ Rideable(Car { Power = 160<horsepower> }) ] },
        23000m<dd>

    let volkswagenGolf: PurchasableItem =
        { Brand = "Volkswagen"
          Name = "Golf"
          Properties = [ Rideable(Car { Power = 147<horsepower> }) ] },
        26000m<dd>

    let budget: PurchasableItem list =
        [ toyotaCorolla; fordFocus; hondaCivic; volkswagenGolf ]

    (* == Medium-range cars. == *)

    let mazdaCX5: PurchasableItem =
        { Brand = "Mazda"
          Name = "CX-5"
          Properties = [ Rideable(Car { Power = 227<horsepower> }) ] },
        37000m<dd>

    let bmw3Series: PurchasableItem =
        { Brand = "BMW"
          Name = "3 Series"
          Properties = [ Rideable(Car { Power = 255<horsepower> }) ] },
        55000m<dd>

    let audiA4: PurchasableItem =
        { Brand = "Audi"
          Name = "A4"
          Properties = [ Rideable(Car { Power = 261<horsepower> }) ] },
        56000m<dd>

    let mercedesBenzCClass: PurchasableItem =
        { Brand = "Mercedes-Benz"
          Name = "C-Class"
          Properties = [ Rideable(Car { Power = 255<horsepower> }) ] },
        60000m<dd>

    let lexusES: PurchasableItem =
        { Brand = "Lexus"
          Name = "ES"
          Properties = [ Rideable(Car { Power = 203<horsepower> }) ] },
        43000m<dd>

    let volvoS60: PurchasableItem =
        { Brand = "Volvo"
          Name = "S60"
          Properties = [ Rideable(Car { Power = 250<horsepower> }) ] },
        46000m<dd>

    let midRange: PurchasableItem list =
        [ mazdaCX5; bmw3Series; audiA4; mercedesBenzCClass; lexusES; volvoS60 ]

    (* == Premium cars. == *)

    let porscheTaycan: PurchasableItem =
        { Brand = "Porsche"
          Name = "Taycan"
          Properties = [ Rideable(Car { Power = 625<horsepower> }) ] },
        160000m<dd>

    let porsche911: PurchasableItem =
        { Brand = "Porsche"
          Name = "911"
          Properties = [ Rideable(Car { Power = 443<horsepower> }) ] },
        145000m<dd>

    let jaguarFType: PurchasableItem =
        { Brand = "Jaguar"
          Name = "F-Type"
          Properties = [ Rideable(Car { Power = 575<horsepower> }) ] },
        135000m<dd>

    let mercedesBenzSClass: PurchasableItem =
        { Brand = "Mercedes-Benz"
          Name = "S-Class"
          Properties = [ Rideable(Car { Power = 429<horsepower> }) ] },
        120000m<dd>

    let audiR8: PurchasableItem =
        { Brand = "Audi"
          Name = "R8"
          Properties = [ Rideable(Car { Power = 562<horsepower> }) ] },
        155000m<dd>

    let bmw7Series: PurchasableItem =
        { Brand = "BMW"
          Name = "7 Series"
          Properties = [ Rideable(Car { Power = 335<horsepower> }) ] },
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
