module Duets.Data.Items.Food.Index

open Duets.Entities
open Duets.Data.Items.Food

let all: PurchasableItem list =
    Breakfast.all
    @ Czech.all
    @ Japanese.all
    @ Italian.all
    @ French.all
    @ Mexican.all
    @ USA.all
    @ Vietnamese.all
