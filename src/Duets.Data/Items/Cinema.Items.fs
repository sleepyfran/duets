module Duets.Data.Items.Cinema

open Duets.Entities

/// Brand used for all cinema concession items, used to identify them during
/// a movie to auto-consume what the player brought into the screening room.
let [<Literal>] brand = "DuetsCinemas"

let private snack name amount =
    { Brand = brand
      Name = name
      Properties = [ Edible { Amount = amount; FoodType = Unhealthy; CookingSkillRequired = 0 } ] }

let private soda name amount =
    { Brand = brand
      Name = name
      Properties = [ Drinkable { Amount = amount; DrinkType = Soda } ] }

/// Cinema concession stand items, priced with the traditional cinema markup.
let snacks: PurchasableItem list =
    [ snack "Large Popcorn" 150<gram>, 8.50m<dd>
      snack "Small Popcorn" 75<gram>, 5.50m<dd>
      snack "Nachos with Cheese" 200<gram>, 7.00m<dd>
      snack "Hotdog" 150<gram>, 6.50m<dd>
      snack "Candy Mix" 100<gram>, 4.50m<dd>
      snack "Chocolate Bar" 50<gram>, 3.50m<dd> ]

let drinks: PurchasableItem list =
    [ soda "Large Coca-Cola" 600<milliliter>, 6.50m<dd>
      soda "Medium Coca-Cola" 400<milliliter>, 5.00m<dd>
      soda "Large Sprite" 600<milliliter>, 6.50m<dd>
      soda "Bottled Water" 500<milliliter>, 4.00m<dd> ]

let all = snacks @ drinks
