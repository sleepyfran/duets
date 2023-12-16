module Duets.Cli.Text.Shop

open Duets.Entities

let itemNameHeader = Styles.header "Name"

let itemTypeHeader = Styles.header "Type"

let itemPriceHeader = Styles.header "Price"

let itemType t = Generic.itemDetailedName t

let itemPrice price = Styles.money price

let itemInteractiveRow (item, price) = $"{item.Brand} ({Styles.money price})"

let itemPrompt =
    "Which item do you want to order?"

let itemNotFound input =
    Styles.error $"There's no \"{input}\" in the menu"

let notEnoughFunds =
    Styles.error
        "The personnel are looking at you very weirdly as your credit card keeps on getting rejected. You don't have enough funds to buy that"
