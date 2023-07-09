module Duets.Data.Items.Food

open Duets.Entities

module BreakfastFood =
    let avocadoEggSandwich: PurchasableItem =
        { Brand = "Avocado Egg Sandwich"
          Type = Healthy 150<gram> |> Food |> Consumable },
        3.4m<dd>

    let bltSandwich: PurchasableItem =
        { Brand = "BLT Sandwich"
          Type = Regular 200<gram> |> Food |> Consumable },
        3.2m<dd>

    let croissant: PurchasableItem =
        { Brand = "Croissant"
          Type = Unhealthy 100<gram> |> Food |> Consumable },
        1.2m<dd>

    let fruitPlate: PurchasableItem =
        { Brand = "Fruit Plate"
          Type = Healthy 200<gram> |> Food |> Consumable },
        2.8m<dd>

    let granolaBowl: PurchasableItem =
        { Brand = "Yogurt Granola Bowl"
          Type = Healthy 250<gram> |> Food |> Consumable },
        3m<dd>

    let all =
        [ croissant; bltSandwich; fruitPlate; granolaBowl; avocadoEggSandwich ]

module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Burger"
          Type = Unhealthy 400<gram> |> Food |> Consumable },
        2.5m<dd>

    let genericChips: PurchasableItem =
        { Brand = "Chips"
          Type = Unhealthy 150<gram> |> Food |> Consumable },
        0.5m<dd>

    let genericFries: PurchasableItem =
        { Brand = "Fries"
          Type = Unhealthy 250<gram> |> Food |> Consumable },
        1.2m<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Nachos"
          Type = Unhealthy 300<gram> |> Food |> Consumable },
        2.3m<dd>

    let all = [ genericBurger; genericChips; genericFries; genericNachos ]

module JapaneseFood =
    let gyoza: PurchasableItem =
        { Brand = "Gyoza"
          Type = Regular 100<gram> |> Food |> Consumable },
        3.3m<dd>

    let misoRamen: PurchasableItem =
        { Brand = "Miso Ramen"
          Type = Healthy 450<gram> |> Food |> Consumable },
        6.4m<dd>

    let tonkotsuRamen: PurchasableItem =
        { Brand = "Tonkotsu Ramen"
          Type = Healthy 450<gram> |> Food |> Consumable },
        6.3m<dd>

    let salmonNigiriSushi: PurchasableItem =
        { Brand = "Salmon Nigiri"
          Type = Healthy 100<gram> |> Food |> Consumable },
        7m<dd>

    let tunaNigiriSushi: PurchasableItem =
        { Brand = "Tuna Nigiri"
          Type = Healthy 100<gram> |> Food |> Consumable },
        7m<dd>

    let avocadoNigiriSushi: PurchasableItem =
        { Brand = "Avocado Nigiri"
          Type = Healthy 100<gram> |> Food |> Consumable },
        7m<dd>

    let salmonMakiSushi: PurchasableItem =
        { Brand = "Salmon Maki"
          Type = Healthy 100<gram> |> Food |> Consumable },
        7.2m<dd>

    let avocadoMakiSushi: PurchasableItem =
        { Brand = "Avocado Maki"
          Type = Healthy 100<gram> |> Food |> Consumable },
        7.2m<dd>

    let californiaRollSushi: PurchasableItem =
        { Brand = "California Roll"
          Type = Healthy 150<gram> |> Food |> Consumable },
        7.8m<dd>

    let wakame: PurchasableItem =
        { Brand = "Wakame"
          Type = Healthy 100<gram> |> Food |> Consumable },
        2.5m<dd>

    let all =
        [ gyoza
          wakame
          misoRamen
          tonkotsuRamen
          avocadoMakiSushi
          avocadoNigiriSushi
          californiaRollSushi
          salmonMakiSushi
          salmonNigiriSushi
          tunaNigiriSushi ]

module MeatFood =
    let steakChimichurri: PurchasableItem =
        { Brand = "Steak Chimichurri"
          Type = Regular 300<gram> |> Food |> Consumable },
        9.5m<dd>

    let herbChicken: PurchasableItem =
        { Brand = "Herb Chicken"
          Type = Regular 300<gram> |> Food |> Consumable },
        6.5m<dd>

    let all = [ steakChimichurri; herbChicken ]

module VegetarianFood =
    let falafelWithHummus: PurchasableItem =
        { Brand = "Falafel with Hummus"
          Type = Regular 300<gram> |> Food |> Consumable },
        4.5m<dd>

    let falafelWithTahini: PurchasableItem =
        { Brand = "Falafel with Tahini"
          Type = Regular 300<gram> |> Food |> Consumable },
        4.3m<dd>

    let avocadoSalad =
        { Brand = "Avocado Salad"
          Type = Healthy 300<gram> |> Food |> Consumable },
        4.1m<dd>

    let goatCheeseSalad =
        { Brand = "Goat Cheese Salad"
          Type = Healthy 300<gram> |> Food |> Consumable },
        4.5m<dd>

    let all =
        [ falafelWithHummus; falafelWithTahini; avocadoSalad; goatCheeseSalad ]

module VietnameseFood =
    let bunBoNamBo: PurchasableItem =
        { Brand = "Bún Bò Nam Bộ"
          Type = Healthy 350<gram> |> Food |> Consumable },
        5.30m<dd>

    let nemCuonBo: PurchasableItem =
        { Brand = "Nem cuốn bò"
          Type = Healthy 100<gram> |> Food |> Consumable },
        3.50m<dd>

    let nemCuonTom: PurchasableItem =
        { Brand = "Nem cuốn tôm"
          Type = Healthy 100<gram> |> Food |> Consumable },
        3.35m<dd>

    let phoBo: PurchasableItem =
        { Brand = "Phở Bò"
          Type = Healthy 350<gram> |> Food |> Consumable },
        5.45m<dd>

    let all = [ phoBo; nemCuonBo; nemCuonTom; bunBoNamBo ]

let all =
    BreakfastFood.all
    @ FastFood.all
    @ JapaneseFood.all
    @ MeatFood.all
    @ VegetarianFood.all
    @ VietnameseFood.all
