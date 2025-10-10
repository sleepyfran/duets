module Duets.Cli.Text.Shop

open Duets.Entities

let itemNameHeader = Styles.header "Name"

let itemTypeHeader = Styles.header "Type"

let itemPriceHeader = Styles.header "Price"

let itemType t = Generic.itemDetailedName t

let itemPrice price = Styles.money price

let itemInteractiveRow (item: Item, price) =
    $"{Generic.itemDetailedName item} ({Styles.money price})"

let itemPrompt = "Which item do you want to order?"

let paymentRejectNotEnoughFunds =
    "Your payment was reject due to insufficient funds." |> Styles.error

let itemNotFound input =
    Styles.error $"There's no \"{input}\" in the menu"

let notEnoughFunds =
    Styles.error
        "The personnel are looking at you very weirdly as your credit card keeps on getting rejected. You don't have enough funds to buy that"

let carSelectionPrompt = "Which car would you like to see?"

let carInteractiveRow (item: Item, price) =
    $"{item.Brand} {item.Name} - {Styles.money price}"

let carPrice (price: Amount) = $"Price: {Styles.money price}"

let confirmCarPurchase = "Would you like to purchase this car?"

let processingPurchase = "Processing your purchase..."

let carDealerGenericFarewell =
    "Feel free to come back anytime. We're always happy to help!"

let carAvailableOutside =
    "Your new car is waiting for you outside the dealership!"
