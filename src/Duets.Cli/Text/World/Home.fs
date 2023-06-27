module Duets.Cli.Text.World.Home

open Duets.Agents
open Duets.Entities
open Duets.Simulation

let rec description _ (roomType: RoomType) =
    let dayMoment =
        State.get () |> Queries.Calendar.today |> Calendar.Query.dayMomentOf

    dayMoment
    |> match roomType with
       | RoomType.Bedroom -> bedroomDescription
       | RoomType.Kitchen -> kitchenDescription
       | RoomType.LivingRoom -> livingRoomDescription
       | _ -> failwith "Room type not supported in home"

and private bedroomDescription dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        "The bedroom is softly lit by the morning sun, creating a warm and gentle atmosphere. The curtains are partially drawn, allowing a soft glow to fill the room. The bed is neatly made, ready for a new day."
    | Midday
    | Afternoon ->
        "Sunlight streams into the bedroom, filling it with brightness and energy. The curtains are open wide, welcoming the warm rays of the sun. The walls are adorned with vibrant colors, creating a lively ambiance."
    | Evening ->
        "The bedroom is bathed in soft, warm lighting, creating a cozy and relaxed atmosphere. The curtains are drawn, providing a sense of privacy and tranquility. The walls are painted in soothing tones, creating a peaceful mood."
    | Night
    | Midnight ->
        "The bedroom is dimly lit, with a faint glow from the moonlight seeping through the curtains. The room is quiet and still, creating a serene ambiance."

and private kitchenDescription dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        "The kitchen is softly illuminated by the morning light, giving it a warm and inviting ambiance. The sunlight filters through the windows, casting a gentle glow on the countertops and appliances."
    | Midday
    | Afternoon ->
        "Sunlight streams into the kitchen, filling it with brightness and energy. The windows are open, allowing a pleasant breeze to flow through the space. The countertops are adorned with various ingredients and utensils, showcasing the ongoing culinary activities."
    | Evening ->
        "Soft lighting bathes the kitchen, creating a cozy and relaxed atmosphere. The countertops and dining table are gently illuminated, providing an inviting space to gather and share a meal. The aroma of delicious food wafts through the air, creating an appetizing ambiance."
    | Night
    | Midnight ->
        "The kitchen is dimly lit, with only a faint glow from the moonlight seeping through the windows. The room is quiet and still, evoking a peaceful ambiance. The countertops and appliances sit quietly."

and private livingRoomDescription dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        "The living room is bathed in gentle morning light, creating a bright and welcoming atmosphere. Sunbeams filter through the windows, casting soft patterns on the floor. The room is neatly arranged, with comfortable seating and a coffee table at its center. The walls are adorned with tasteful artwork, adding a touch of elegance to the space. It's a pleasant area to start the day, providing a cozy setting for relaxation or engaging in quiet activities."
    | Midday
    | Afternoon ->
        "Sunlight streams into the living room, filling it with a warm and vibrant glow. The windows offer a view of the outside world, creating a sense of connection to the surrounding environment. The furniture is arranged to facilitate comfortable conversation and interaction. The room feels lively and inviting, a hub for socializing or enjoying leisure activities during the afternoon hours."
    | Evening ->
        "Soft, ambient lighting creates a cozy and intimate atmosphere in the living room. Lamps or overhead lights illuminate the space with a gentle glow. The seating arrangements are arranged to encourage relaxation and coziness. The room feels calm and comfortable, providing a peaceful setting for unwinding, reading, or enjoying quality time with loved ones in the evening."
    | Night
    | Midnight ->
        "The living room is dimly lit, creating a serene and tranquil environment. Soft lighting fixtures or candles cast a subdued glow, offering a soothing ambiance. The room feels quiet and peaceful, providing a space for relaxation or solitary activities during the late hours. The comfortable seating and serene atmosphere make it an inviting spot to reflect, read, or simply enjoy a moment of quiet solitude."
