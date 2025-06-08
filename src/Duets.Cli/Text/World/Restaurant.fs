module Duets.Cli.Text.World.Restaurant

open Duets.Common
open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Restaurant _ ->
        [ "The restaurant hums with activity as a vibrant crowd fills every table. The aroma of delicious cuisine wafts through the air, enticing patrons with its mouthwatering allure. Waitstaff expertly navigate the lively dining room, attending to the needs of guests with a blend of efficiency and warmth. It's a lively and bustling atmosphere, with the clinking of cutlery and animated conversations creating a symphony of dining delight."
          "The restaurant is moderately busy, with a comfortable number of diners occupying the well-appointed tables. The gentle buzz of conversation intertwines with the soothing background music, creating a pleasant ambiance. The attentive waitstaff move gracefully between tables, ensuring a seamless dining experience. It's an inviting setting, perfect for intimate gatherings or enjoyable meals with friends and family."
          "The restaurant exudes an elegant and refined atmosphere, with only a few select diners savoring their meals in a spacious dining area. Soft lighting illuminates the tastefully decorated interior, casting a warm glow over the room. The attentive waitstaff provide personalized service, attending to each guest's needs with care and attention to detail. It's a sophisticated haven, ideal for those seeking an exclusive dining experience or a quiet celebration."
          "The restaurant is nearly empty, with only a couple of occupied tables scattered across the room. The subdued lighting and soft background music create an intimate and cozy ambiance. The attentive waitstaff move discreetly between tables, providing impeccable service to the few guests present. It's a perfect spot for a romantic dinner or a quiet business meeting, where privacy and attention to detail are paramount." ]
        |> List.sample
    | RoomType.Kitchen ->
        [ "The kitchen is a whirlwind of controlled chaos. Chefs in crisp whites move with precision, flames flare from stovetops, and the air is thick with the tantalizing aromas of spices and searing ingredients. Stainless steel surfaces gleam under bright lights, reflecting the focused energy of the culinary team."
          "A symphony of sizzling, chopping, and clattering pans fills the kitchen. Steam rises from pots, and the scent of freshly baked bread mingles with roasting meats. Orders are called out, and the team works in harmonious rhythm, transforming raw ingredients into culinary masterpieces."
          "The kitchen is in full swing, a hive of activity. Sauces simmer, vegetables are prepped with expert knife skills, and the pastry chef meticulously plates a delicate dessert. There's an undeniable buzz of creativity and dedication in this high-pressure environment."
          "Between rushes, the kitchen is a space of focused preparation. Surfaces are scrubbed, ingredients are restocked, and the team discusses the upcoming service. Even in quieter moments, there's an undercurrent of readiness for the next wave of orders."]
        |> List.sample
    | _ -> failwith "Room type not supported in restaurant"
