module Duets.Data.Items.Food.Index

open Duets.Entities
open Duets.Data.Items.Food

let all: PurchasableItem list =
    Breakfast.all
    @ Canadian.all
    @ Chilean.all
    @ Czech.all
    @ German.all
    @ Japanese.all
    @ Italian.all
    @ French.all
    @ Korean.all
    @ Mexican.all
    @ Snack.all
    @ Spanish.all
    @ Turkish.all
    @ USA.all
    @ Vietnamese.all
