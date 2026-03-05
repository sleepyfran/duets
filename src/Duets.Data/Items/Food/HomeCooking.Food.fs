module Duets.Data.Items.Food.HomeCooking

open Duets.Entities

/// The 20 most common western meals that people cook at home on a daily basis.
/// Not exposed via All.Food.fs (not available in shops), only used for the
/// cook command's default selection.
let all: PurchasableItem list =
    [ Item.Food.create "Scrambled Eggs" 150<gram> Healthy 0, 1.5m<dd>
      Item.Food.create "Grilled Cheese Sandwich" 200<gram> Unhealthy 0, 1.5m<dd>
      Item.Food.create "BLT Sandwich" 300<gram> Regular 0, 3.0m<dd>
      Item.Food.create "Pancakes" 200<gram> Regular 5, 2.0m<dd>
      Item.Food.create "Pasta with Tomato Sauce" 400<gram> Regular 5, 3.0m<dd>
      Item.Food.create "Macaroni and Cheese" 350<gram> Unhealthy 5, 2.5m<dd>
      Item.Food.create "Vegetable Stir Fry" 350<gram> Healthy 5, 3.0m<dd>
      Item.Food.create "Caesar Salad" 250<gram> Healthy 5, 3.5m<dd>
      Item.Food.create "Tomato Soup" 350<gram> Healthy 5, 2.5m<dd>
      Item.Food.create "Hamburger" 300<gram> Unhealthy 5, 4.0m<dd>
      Item.Food.create "French Omelette" 200<gram> Healthy 10, 3.0m<dd>
      Item.Food.create "Chicken Stir Fry" 400<gram> Healthy 10, 5.0m<dd>
      Item.Food.create "Beef Tacos" 350<gram> Regular 10, 5.5m<dd>
      Item.Food.create "Fried Rice" 400<gram> Regular 10, 3.5m<dd>
      Item.Food.create "Baked Chicken" 400<gram> Healthy 15, 6.0m<dd>
      Item.Food.create "Fish Fillets" 350<gram> Healthy 15, 7.0m<dd>
      Item.Food.create "Vegetable Curry" 400<gram> Healthy 15, 5.5m<dd>
      Item.Food.create "Meatballs in Tomato Sauce" 400<gram> Regular 15,
      6.0m<dd>
      Item.Food.create "Beef Stew" 500<gram> Regular 20, 8.0m<dd>
      Item.Food.create "Roast Chicken" 600<gram> Regular 20, 9.0m<dd> ]
    |> List.sortBy (fun (item, _) -> item.Name)
