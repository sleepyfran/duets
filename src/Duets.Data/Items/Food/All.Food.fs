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
    @ Snack.all
    @ Spanish.all
    @ Turkish.all
    @ USA.all
    @ Vietnamese.all
