module rec Duets.Cli.Text.World.Hotel

open Duets.Agents
open Duets.Entities
open Duets.Simulation

type private HotelQuality =
    | Low
    | Average
    | High

let description (place: Place) (roomType: RoomType) =
    let dayMoment =
        State.get () |> Queries.Calendar.today |> Calendar.Query.dayMomentOf

    (dayMoment, place.Quality)
    ||> match roomType with
        | RoomType.Bedroom -> hotelRoomDescription
        | RoomType.Lobby -> lobbyDescription
        | _ -> failwith "Room type not supported in hotel"

let private hotelQuality quality =
    match quality with
    | _ when quality < 50<quality> -> Low
    | _ when quality <= 80<quality> -> Average
    | _ -> High

let private hotelRoomDescription dayMoment quality =
    match dayMoment, hotelQuality quality with
    | (EarlyMorning | Morning), Low ->
        "The hotel room is dimly lit by the weak morning sun, casting a somber atmosphere. The blinds are poorly adjusted, barely letting any light in. The beds are roughly made, not particularly inviting."
    | (EarlyMorning | Morning), Average ->
        "The hotel room is moderately illuminated by the morning sun, creating a satisfactory ambiance. The blinds are half-open, allowing a decent amount of light. The beds are decently prepared, ready for the day."
    | (EarlyMorning | Morning), High ->
        "The hotel room is gently illuminated by the early morning sun, casting a warm and soothing ambiance. The blinds are partly opened, letting a tender glow permeate the room. The beds are immaculately prepared, anticipating the bustle of a new day."
    | (Midday | Afternoon), Low ->
        "Harsh sunlight floods the run-down hotel room, creating an unpleasant atmosphere. The blinds are fully retracted, causing a glare. The room's décor is outdated, producing a less than appealing environment."
    | (Midday | Afternoon), Average ->
        "Adequate sunlight enters the hotel room, creating a decent atmosphere. The blinds are fully retracted, allowing a good amount of light. The room's décor is satisfactory, providing a comfortable environment."
    | (Midday | Afternoon), High ->
        "Bright sunlight pours into the luxurious hotel room, engulfing it with vivacity and enthusiasm. The blinds are fully retracted, inviting the radiant sunlight in. The room's décor gleams, producing a spirited environment."
    | (Evening), Low ->
        "The cheap hotel room is barely lit, making the atmosphere less than welcoming. The blinds are closed, but do little to provide privacy. The room's colors are dull and uninviting."
    | (Evening), Average ->
        "The average hotel room is adequately lit, creating a comfortable atmosphere. The blinds are closed, providing a fair level of privacy. The room's colors are neutral, adding to the overall sense of tranquility."
    | (Evening), High ->
        "The luxury hotel room is immersed in a mellow, warm light, fostering a calm and relaxing atmosphere. The blinds are closed, offering a sense of seclusion and tranquillity. The room’s colors are subdued, evoking a sense of quiet relaxation."
    | (Night | Midnight), Low ->
        "The shoddy hotel room is poorly lit, with the barely visible moonlight creeping through the broken blinds. The room is eerily quiet and seems neglected."
    | (Night | Midnight), Average ->
        "The standard hotel room is sufficiently lit by the moonlight filtering through the blinds. The room is calm, offering a decent night's rest."
    | (Night | Midnight), High ->
        "The luxurious hotel room is faintly illuminated, with the silvery glow of moonlight filtering through the blinds. The room is tranquil and motionless, establishing a serene atmosphere."

let private lobbyDescription dayMoment quality =
    match dayMoment, hotelQuality quality with
    | (EarlyMorning | Morning | Midday | Afternoon | Evening), Low ->
        "The hotel lobby is poorly lit and in a state of disarray, with old, worn-out furniture scattered around. The receptionist, although present, seems disinterested and provides less than satisfactory service."
    | (EarlyMorning | Morning | Midday | Afternoon | Evening), Average ->
        "The hotel lobby is sufficiently illuminated, providing a fair welcome. The furniture is a bit outdated but clean and functional. The receptionist is present and provides a decent level of customer service."
    | (EarlyMorning | Morning | Midday | Afternoon | Evening), High ->
        "The hotel lobby is beautifully illuminated, exuding a sense of luxury and elegance. The furniture is top-notch and arranged in a way that provides a welcoming atmosphere. The receptionist is not only present but also attentive and provides excellent customer service."
    | (Night | Midnight), Low ->
        "The hotel lobby is poorly lit and appears neglected. The furniture is worn-out and there is no receptionist in sight."
    | (Night | Midnight), Average ->
        "The hotel lobby is decently lit, providing an adequate atmosphere. The furniture shows some signs of use, but it's generally clean and functional. The receptionist is not present at this late hour."
    | (Night | Midnight), High ->
        "The hotel lobby is subtly illuminated, creating a tranquil and luxurious atmosphere. The furniture is high-end, well-maintained and provides a sense of comfort despite the late hour. Although there is no receptionist at this hour, there are clear signs of professional service."
