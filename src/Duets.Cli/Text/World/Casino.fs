module Duets.Cli.Text.World.Casino

open Duets.Common
open Duets.Entities

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Bar ->
        [ "The casino bar is a lively spot where guests unwind with a drink in hand. Bartenders skillfully mix cocktails, and the atmosphere is abuzz with conversations and laughter. The bar's stools are filled with patrons sharing stories and toasting to their good fortune. It's a great place to take a break from gaming and enjoy a drink with friends."
          "You've found your way to the casino bar, a vibrant space where guests sip on signature cocktails and exchange tales of their casino adventures. The bartenders are masters of their craft, and the drinks menu is extensive. Laughter and clinking glasses fill the air as patrons relax and celebrate their wins. It's a lively oasis in the heart of the casino."
          "The casino bar is a popular gathering spot for casino-goers, with an impressive selection of drinks and a lively atmosphere. The bartenders are known for their mixology skills, and guests enjoy a wide range of libations. Laughter and cheers punctuate the conversations as patrons raise their glasses to good times and good fortune. It's a place where the party never stops."
          "As you enter the casino bar, the lively ambiance immediately captures your attention. Bartenders craft exquisite cocktails, and patrons share stories and celebrate their wins. The atmosphere is charged with excitement, making it the perfect place to recharge and socialize. It's a hub of energy and camaraderie in the heart of the casino." ]
        |> List.sample
    | RoomType.Lobby ->
        [ "The casino lobby welcomes you with a grand entrance adorned with sparkling chandeliers and plush carpeting. Guests gather at the reception desk, checking in and out of their luxurious accommodations. The sound of laughter and clinking glasses emanates from the nearby bar, setting the tone for an evening of entertainment. It's the perfect starting point for your casino adventure."
          "You find yourself in the elegant casino lobby, where guests in formal attire mingle and chat. The concierge desk is busy attending to guests' needs, and a large marble statue stands as a centerpiece. The soft hum of conversation and the occasional clink of champagne glasses create an air of sophistication and anticipation. It's a place where you can start your casino experience in style."
          "The casino lobby exudes opulence, with polished marble floors and tastefully decorated walls. Guests arrive in style, and the concierge desk is always at their service. The inviting ambiance hints at the excitement that awaits on the casino floor. You can hear the lively chatter and the pleasant notes of live piano music from the adjacent bar. It's a glamorous prelude to the thrill of the games."
          "As you step into the casino lobby, you're greeted by the sight of well-dressed guests mingling in an atmosphere of luxury. The concierge stands ready to assist, and the scent of fine cigars wafts from the nearby bar. The soft piano music in the background adds to the refined ambiance. It's a place where the world of high-stakes entertainment begins, promising a memorable experience." ]
        |> List.sample
    | RoomType.CasinoFloor ->
        [ "The casino floor is a dazzling spectacle of lights and sounds, with rows of slot machines blinking in a hypnotic rhythm. The clinking of coins and the occasional burst of cheers from nearby tables fill the air with excitement. Gamblers try their luck at various games, from poker to blackjack, while elegantly dressed croupiers manage the action with skill. The atmosphere is electric, making it the heart of the casino's entertainment."
          "The casino floor stretches out before you, bathed in a colorful glow from neon signs and decorative lights. Players crowd around the roulette wheel and card tables, hoping for a stroke of luck. The constant shuffle of cards and the roll of dice add to the energetic ambiance. It's a place where fortunes can change in an instant, and the thrill of risk is palpable."
          "Amidst the glitz and glamour of the casino floor, gamblers test their strategies and luck at various games. The constant murmur of conversation, the clinking of chips, and the whirl of the roulette wheel create an exhilarating atmosphere. Patrons in elegant attire move from table to table, hoping to hit the jackpot. It's a bustling hub of entertainment, where every spin and deal holds the promise of excitement."
          "The casino floor is alive with activity as patrons try their luck at games of chance. The soft, ambient music sets the mood as gamblers place bets and anticipate the next card drawn. The colorful decor and opulent surroundings create an immersive experience, drawing visitors into a world of glamour and risk. It's a place where fortunes are won and lost, and the thrill of the game never fades." ]
        |> List.sample
    | _ -> failwith "Room type not supported in casino"
