module Duets.Cli.Text.World.Cafe

open Duets.Common
open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Cafe ->
        [ "The cafe buzzes with activity as a steady stream of customers fills the cozy space. The aroma of freshly brewed coffee permeates the air, blending with the enticing scent of pastries. People gather at the counter, eagerly placing orders and engaging in lively conversations. It's a vibrant and bustling atmosphere, with baristas skillfully crafting drinks amidst the cheerful chatter of a thriving cafe."
          "The cafe is moderately busy, with a pleasant number of patrons enjoying their beverages and treats. There are enough people to create a lively ambiance, but still ample space to find a cozy corner to relax. The soft murmur of conversation mingles with the gentle clinking of cups, creating a warm and inviting atmosphere."
          "The cafe exudes a calm and tranquil atmosphere, with only a few customers scattered around the comfortable seating arrangements. Soft instrumental music plays in the background, enhancing the serene ambiance. Patrons quietly sip their drinks, immersed in their thoughts or engrossed in a good book. It's a peaceful oasis, perfect for a moment of solitude or a quiet chat with a close friend."
          "The cafe is nearly empty, with only a couple of people occupying a table or two. The dim lighting casts a cozy glow over the space, making it feel intimate and secluded. The barista stands attentively behind the counter, ready to serve those seeking respite from the bustling world outside. It's a haven for introverts and anyone seeking a tranquil space to enjoy a cup of coffee in peaceful solitude." ]
        |> List.sample
    | _ -> failwith "Room type not supported in cafe"
