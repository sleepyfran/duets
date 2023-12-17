module Duets.Data.Items.Drink

open Duets.Common
open Duets.Entities

module Beer =
    (* Australian beer. *)
    let victoriaBitter: PurchasableItem =
        Item.Beer.create "Victoria Bitter beer" 375<milliliter> 4.9, 3.0m<dd>

    let fostersLager: PurchasableItem =
        Item.Beer.create "Foster's Lager beer" 375<milliliter> 4.0, 2.8m<dd>

    let coopersPaleAle: PurchasableItem =
        Item.Beer.create "Coopers Pale Ale beer" 375<milliliter> 4.5, 3.2m<dd>

    let carltonDraught: PurchasableItem =
        Item.Beer.create "Carlton Draught beer" 375<milliliter> 4.6, 3.1m<dd>

    let tooheysNew: PurchasableItem =
        Item.Beer.create "Tooheys New beer" 375<milliliter> 4.6, 3.0m<dd>


    (* Czech beer. *)
    let cernaHoraPint: PurchasableItem =
        Item.Beer.create "Černa Horá beer" 500<milliliter> 4.8, 2.2m<dd>

    let gambrinusPint: PurchasableItem =
        Item.Beer.create "Gambrinus beer" 500<milliliter> 4.3, 1.5m<dd>

    let kozelPint: PurchasableItem =
        Item.Beer.create "Kozel beer" 500<milliliter> 4.6, 1.5m<dd>

    let matushkaPint: PurchasableItem =
        Item.Beer.create "Matuška California beer" 500<milliliter> 5.4, 2.4m<dd>

    let pilsnerUrquellPint: PurchasableItem =
        Item.Beer.create "Pilsner Urquell beer" 500<milliliter> 4.4, 1.8m<dd>

    let staropramenPint: PurchasableItem =
        Item.Beer.create "Staropramen beer" 500<milliliter> 4.7, 1.5m<dd>

    (* Spanish beer. *)
    let cruzcampo: PurchasableItem =
        Item.Beer.create "Cruzcampo beer" 330<milliliter> 4.8, 2.0m<dd>

    let estrellaDamm: PurchasableItem =
        Item.Beer.create "Estrella Damm beer" 330<milliliter> 4.6, 2.1m<dd>

    let mahou: PurchasableItem =
        Item.Beer.create "Mahou beer" 330<milliliter> 4.8, 2.2m<dd>

    let alhambra: PurchasableItem =
        Item.Beer.create "Alhambra beer" 330<milliliter> 4.6, 2.3m<dd>

    let sanMiguel: PurchasableItem =
        Item.Beer.create "San Miguel beer" 330<milliliter> 4.5, 2.0m<dd>

    (* Irish beer. *)
    let guinnessPint: PurchasableItem =
        Item.Beer.create "Guinness beer" 500<milliliter> 4.3, 2.1m<dd>

    (* Japanese beer. *)
    let asahiSuperDry: PurchasableItem =
        Item.Beer.create "Asahi Super Dry beer" 350<milliliter> 5.0, 2.7m<dd>

    let kirinIchiban: PurchasableItem =
        Item.Beer.create "Kirin Ichiban beer" 350<milliliter> 4.6, 2.8m<dd>

    let sapporoPremium: PurchasableItem =
        Item.Beer.create "Sapporo Premium Beer beer" 350<milliliter> 4.7,
        3.0m<dd>

    let suntoryPremiumMalts: PurchasableItem =
        Item.Beer.create "Suntory The Premium Malt's beer" 330<milliliter> 5.5,
        3.2m<dd>

    let yebisu: PurchasableItem =
        Item.Beer.create "Yebisu beer" 350<milliliter> 5.0, 3.3m<dd>

    (* Mexican beer. *)
    let coronaExtra: PurchasableItem =
        Item.Beer.create "Corona Extra beer" 355<milliliter> 4.5, 2.5m<dd>

    let modeloEspecial: PurchasableItem =
        Item.Beer.create "Modelo Especial beer" 355<milliliter> 4.4, 2.6m<dd>

    let pacifico: PurchasableItem =
        Item.Beer.create "Pacifico beer" 355<milliliter> 4.5, 2.7m<dd>

    let tecate: PurchasableItem =
        Item.Beer.create "Tecate beer" 355<milliliter> 4.5, 2.4m<dd>

    let dosEquis: PurchasableItem =
        Item.Beer.create "Dos Equis beer" 355<milliliter> 4.2, 2.6m<dd>

    (* US beer. *)
    let budLight: PurchasableItem =
        Item.Beer.create "Bud Light beer" 355<milliliter> 4.2, 2.5m<dd>

    let coorsLight: PurchasableItem =
        Item.Beer.create "Coors Light beer" 355<milliliter> 4.2, 2.6m<dd>

    let millerLite: PurchasableItem =
        Item.Beer.create "Miller Lite beer" 355<milliliter> 4.2, 2.4m<dd>

    let yuengling: PurchasableItem =
        Item.Beer.create "Yuengling beer" 355<milliliter> 4.4, 2.8m<dd>

    let samuelAdams: PurchasableItem =
        Item.Beer.create "Samuel Adams beer" 355<milliliter> 5.0, 3.0m<dd>

    (* UK beer. *)
    let fullersLondonPride: PurchasableItem =
        Item.Beer.create "Fuller's London Pride beer" 568<milliliter> 4.7,
        3.5m<dd>

    let timothyTaylorLandlord: PurchasableItem =
        Item.Beer.create "Timothy Taylor Landlord beer" 568<milliliter> 4.3,
        3.6m<dd>

    let shepherdNeameSpitfire: PurchasableItem =
        Item.Beer.create "Shepherd Neame Spitfire beer" 568<milliliter> 4.2,
        3.4m<dd>

    let adnamsBroadside: PurchasableItem =
        Item.Beer.create "Adnams Broadside beer" 568<milliliter> 4.7, 3.7m<dd>

    let stAustellTribute: PurchasableItem =
        Item.Beer.create "St Austell Tribute beer" 568<milliliter> 4.2, 3.6m<dd>

    let private americanBeers =
        [ budLight; coorsLight; millerLite; yuengling; samuelAdams ]

    /// Defines the most common beers by location.
    let byLocation =
        [ (London,
           [ fullersLondonPride
             timothyTaylorLandlord
             shepherdNeameSpitfire
             adnamsBroadside
             stAustellTribute ])
          (LosAngeles, americanBeers)
          (Madrid, [ cruzcampo; estrellaDamm; mahou; alhambra; sanMiguel ])
          (MexicoCity,
           [ coronaExtra; modeloEspecial; pacifico; tecate; dosEquis ])
          (NewYork, americanBeers)
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
        Item.Coffee.create "Espresso" 20<milliliter> 20<milliliter>, 1.8m<dd>

    let doubleEspresso: PurchasableItem =
        Item.Coffee.create "Double espresso" 40<milliliter> 40<milliliter>,
        2.2m<dd>

    let capuccino: PurchasableItem =
        Item.Coffee.create "Capuccino" 105<milliliter> 20<milliliter>, 2.5m<dd>

    let flatWhite: PurchasableItem =
        Item.Coffee.create "Flat White" 125<milliliter> 40<milliliter>, 2.7m<dd>

    let all = [ espresso; doubleEspresso; capuccino; flatWhite ]

module SoftDrinks =
    let cocaColaBottle: PurchasableItem =
        Item.Soda.create "Coca Cola" 330<milliliter>, 0.6m<dd>

    let homemadeLemonade: PurchasableItem =
        Item.Soda.create "Homemade lemonade" 500<milliliter>, 0.9m<dd>

    let all = [ cocaColaBottle; homemadeLemonade ]
