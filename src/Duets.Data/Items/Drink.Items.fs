module Duets.Data.Items.Drink

open Duets.Common
open Duets.Entities

module Beer =
    (* Australian beer. *)
    let victoriaBitter: PurchasableItem =
        { Brand = "Victoria Bitter"
          Type = Beer(375<milliliter>, 4.9) |> Drink |> Consumable },
        3.0m<dd>

    let fostersLager: PurchasableItem =
        { Brand = "Foster's Lager"
          Type = Beer(375<milliliter>, 4.0) |> Drink |> Consumable },
        2.8m<dd>

    let coopersPaleAle: PurchasableItem =
        { Brand = "Coopers Pale Ale"
          Type = Beer(375<milliliter>, 4.5) |> Drink |> Consumable },
        3.2m<dd>

    let carltonDraught: PurchasableItem =
        { Brand = "Carlton Draught"
          Type = Beer(375<milliliter>, 4.6) |> Drink |> Consumable },
        3.1m<dd>

    let tooheysNew: PurchasableItem =
        { Brand = "Tooheys New"
          Type = Beer(375<milliliter>, 4.6) |> Drink |> Consumable },
        3.0m<dd>


    (* Czech beer. *)
    let cernaHoraPint: PurchasableItem =
        { Brand = "Černa Horá"
          Type = Beer(500<milliliter>, 4.8) |> Drink |> Consumable },
        2.2m<dd>

    let gambrinusPint: PurchasableItem =
        { Brand = "Gambrinus"
          Type = Beer(500<milliliter>, 4.3) |> Drink |> Consumable },
        1.5m<dd>

    let kozelPint: PurchasableItem =
        { Brand = "Kozel"
          Type = Beer(500<milliliter>, 4.6) |> Drink |> Consumable },
        1.5m<dd>

    let matushkaPint: PurchasableItem =
        { Brand = "Matuška California"
          Type = Beer(500<milliliter>, 5.4) |> Drink |> Consumable },
        2.4m<dd>

    let pilsnerUrquellPint: PurchasableItem =
        { Brand = "Pilsner Urquell"
          Type = Beer(500<milliliter>, 4.4) |> Drink |> Consumable },
        1.8m<dd>

    let staropramenPint: PurchasableItem =
        { Brand = "Staropramen"
          Type = Beer(500<milliliter>, 4.7) |> Drink |> Consumable },
        1.5m<dd>

    (* Spanish beer. *)
    let cruzcampo: PurchasableItem =
        { Brand = "Cruzcampo"
          Type = Beer(330<milliliter>, 4.8) |> Drink |> Consumable },
        2.0m<dd>

    let estrellaDamm: PurchasableItem =
        { Brand = "Estrella Damm"
          Type = Beer(330<milliliter>, 4.6) |> Drink |> Consumable },
        2.1m<dd>

    let mahou: PurchasableItem =
        { Brand = "Mahou"
          Type = Beer(330<milliliter>, 4.8) |> Drink |> Consumable },
        2.2m<dd>

    let alhambra: PurchasableItem =
        { Brand = "Alhambra"
          Type = Beer(330<milliliter>, 4.6) |> Drink |> Consumable },
        2.3m<dd>

    let sanMiguel: PurchasableItem =
        { Brand = "San Miguel"
          Type = Beer(330<milliliter>, 4.5) |> Drink |> Consumable },
        2.0m<dd>

    (* Irish beer. *)
    let guinnessPint: PurchasableItem =
        { Brand = "Guinnes"
          Type = Beer(500<milliliter>, 4.3) |> Drink |> Consumable },
        2.1m<dd>

    (* Japanese beer. *)
    let asahiSuperDry: PurchasableItem =
        { Brand = "Asahi Super Dry"
          Type = Beer(350<milliliter>, 5.0) |> Drink |> Consumable },
        2.7m<dd>

    let kirinIchiban: PurchasableItem =
        { Brand = "Kirin Ichiban"
          Type = Beer(350<milliliter>, 4.6) |> Drink |> Consumable },
        2.8m<dd>

    let sapporoPremium: PurchasableItem =
        { Brand = "Sapporo Premium Beer"
          Type = Beer(355<milliliter>, 4.7) |> Drink |> Consumable },
        3.0m<dd>

    let suntoryPremiumMalts: PurchasableItem =
        { Brand = "Suntory The Premium Malt's"
          Type = Beer(330<milliliter>, 5.5) |> Drink |> Consumable },
        3.2m<dd>

    let yebisu: PurchasableItem =
        { Brand = "Yebisu"
          Type = Beer(350<milliliter>, 5.0) |> Drink |> Consumable },
        3.3m<dd>

    (* Mexican beer. *)
    let coronaExtra: PurchasableItem =
        { Brand = "Corona Extra"
          Type = Beer(355<milliliter>, 4.5) |> Drink |> Consumable },
        2.5m<dd>

    let modeloEspecial: PurchasableItem =
        { Brand = "Modelo Especial"
          Type = Beer(355<milliliter>, 4.4) |> Drink |> Consumable },
        2.6m<dd>

    let pacifico: PurchasableItem =
        { Brand = "Pacifico"
          Type = Beer(355<milliliter>, 4.5) |> Drink |> Consumable },
        2.7m<dd>

    let tecate: PurchasableItem =
        { Brand = "Tecate"
          Type = Beer(355<milliliter>, 4.5) |> Drink |> Consumable },
        2.4m<dd>

    let dosEquis: PurchasableItem =
        { Brand = "Dos Equis"
          Type = Beer(355<milliliter>, 4.2) |> Drink |> Consumable },
        2.6m<dd>

    (* US beer. *)
    let budLight: PurchasableItem =
        { Brand = "Bud Light"
          Type = Beer(355<milliliter>, 4.2) |> Drink |> Consumable },
        2.5m<dd>

    let coorsLight: PurchasableItem =
        { Brand = "Coors Light"
          Type = Beer(355<milliliter>, 4.2) |> Drink |> Consumable },
        2.6m<dd>

    let millerLite: PurchasableItem =
        { Brand = "Miller Lite"
          Type = Beer(355<milliliter>, 4.2) |> Drink |> Consumable },
        2.4m<dd>

    let yuengling: PurchasableItem =
        { Brand = "Yuengling"
          Type = Beer(355<milliliter>, 4.4) |> Drink |> Consumable },
        2.8m<dd>

    let samuelAdams: PurchasableItem =
        { Brand = "Samuel Adams"
          Type = Beer(355<milliliter>, 5.0) |> Drink |> Consumable },
        3.0m<dd>

    (* UK beer. *)
    let fullersLondonPride: PurchasableItem =
        { Brand = "Fuller's London Pride"
          Type = Beer(568<milliliter>, 4.7) |> Drink |> Consumable },
        3.5m<dd>

    let timothyTaylorLandlord: PurchasableItem =
        { Brand = "Timothy Taylor Landlord"
          Type = Beer(568<milliliter>, 4.3) |> Drink |> Consumable },
        3.6m<dd>

    let shepherdNeameSpitfire: PurchasableItem =
        { Brand = "Shepherd Neame Spitfire"
          Type = Beer(568<milliliter>, 4.2) |> Drink |> Consumable },
        3.4m<dd>

    let adnamsBroadside: PurchasableItem =
        { Brand = "Adnams Broadside"
          Type = Beer(568<milliliter>, 4.7) |> Drink |> Consumable },
        3.7m<dd>

    let stAustellTribute: PurchasableItem =
        { Brand = "St Austell Tribute"
          Type = Beer(568<milliliter>, 4.2) |> Drink |> Consumable },
        3.6m<dd>

    /// Defines the most common beers by location.
    let byLocation =
        [ (London,
           [ fullersLondonPride
             timothyTaylorLandlord
             shepherdNeameSpitfire
             adnamsBroadside
             stAustellTribute ])
          (Madrid, [ cruzcampo; estrellaDamm; mahou; alhambra; sanMiguel ])
          (MexicoCity,
           [ coronaExtra; modeloEspecial; pacifico; tecate; dosEquis ])
          (NewYork, [ budLight; coorsLight; millerLite; yuengling; samuelAdams ])
          (Prague,
           [ gambrinusPint; kozelPint; pilsnerUrquellPint; staropramenPint ])
          (Sydney,
           [ victoriaBitter
             fostersLager
             coopersPaleAle
             carltonDraught
             tooheysNew ])
          (Tokyo,
           [ asahiSuperDry
             kirinIchiban
             sapporoPremium
             suntoryPremiumMalts
             yebisu ]) ]
        |> Map.ofList

    let all =
        byLocation
        |> List.ofMapValues
        |> List.concat
        |> List.distinctBy (fun (item, _) -> item.Brand)

module Coffee =
    let espresso: PurchasableItem =
        { Brand = "Espresso"
          Type = Coffee 20<milliliter> |> Drink |> Consumable },
        1.8m<dd>

    let doubleEspresso: PurchasableItem =
        { Brand = "Double espresso"
          Type = Coffee 40<milliliter> |> Drink |> Consumable },
        2.2m<dd>

    let capuccino: PurchasableItem =
        { Brand = "Capuccino"
          Type = Coffee 20<milliliter> |> Drink |> Consumable },
        2.5m<dd>

    let flatWhite: PurchasableItem =
        { Brand = "Flat White"
          Type = Coffee 40<milliliter> |> Drink |> Consumable },
        2.7m<dd>

    let all = [ espresso; doubleEspresso; capuccino; flatWhite ]

module SoftDrinks =
    let cocaColaBottle: PurchasableItem =
        { Brand = "Coca Cola"
          Type = Soda 330<milliliter> |> Drink |> Consumable },
        0.6m<dd>

    let homemadeLemonade: PurchasableItem =
        { Brand = "Homemade lemonade"
          Type = Soda 500<milliliter> |> Drink |> Consumable },
        0.9m<dd>

    let all = [ cocaColaBottle; homemadeLemonade ]
