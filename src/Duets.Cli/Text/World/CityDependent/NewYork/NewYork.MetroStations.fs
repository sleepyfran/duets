[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.NewYork.MetroStations

open Duets.Entities

let rec description dayMoment descriptor =
    match descriptor with
    | BusinessDistrict -> businessDistrict
    | Coastal -> coastal
    | Creative -> creative
    | Cultural -> cultural
    | EntertainmentHeart -> entertainmentHeart
    | Historic -> historic
    | Bohemian -> nonExistent
    | Glitz -> nonExistent
    | Industrial -> nonExistent
    | Luxurious -> nonExistent
    | Nature -> nonExistent
    |> fun fn -> fn dayMoment

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is a blur of motion, a river of suits and briefcases flowing towards the city's commercial heart. The air is filled with a sense of urgency and the metallic screech of arriving trains."
          "Polished steel and cool fluorescent lights define the station's aesthetic. It's a space of pure function, designed for the efficient movement of thousands of commuters."
          "The announcements echo through the cavernous space, a constant, authoritative voice directing the morning rush. The energy is focused and intense, a prelude to a day of high-stakes business." ]
    | Midday
    | Afternoon ->
        [ "The flow of people is less frantic but still constant. The station is a crossroads for lunch meetings, midday appointments, and the ceaseless activity of the business world."
          "The sharp, clean lines of the station reflect the corporate world above. There's little time for lingering; it's a place of transit, a means to an end in a busy workday."
          "The crowd is a mix of employees, messengers, and clients, all moving with a sense of purpose. The station is a vital artery in the city's economic circulatory system." ]
    | Evening ->
        [ "The evening commute is a mirror image of the morning rush, but with an undercurrent of relief. The day's work is done, and the faces in the crowd are turned towards home."
          "The station's energy begins to subside as the last of the commuters depart. The once-crowded platforms start to feel spacious, echoing with the footsteps of the remaining travelers."
          "The bright lights of the station seem to soften as the evening progresses. The relentless pace of the day gives way to a more subdued, end-of-day rhythm." ]
    | Night
    | Midnight ->
        [ "The station is vast, quiet, and almost empty. The silence is broken only by the hum of the escalators and the distant rumble of a late-night train."
          "Under the stark lighting, the station feels like a sterile, futuristic landscape. It's a lonely place, a hollowed-out version of its daytime self."
          "The few remaining travelers are solitary figures in a large, impersonal space. The station rests, a silent monument to the commerce and ambition it serves during the day." ]

and private coastal dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "A faint, salty scent hangs in the air, a pleasant reminder of the nearby water. The station is relatively quiet, used by early morning joggers and commuters heading into the city."
          "The sound of a distant foghorn sometimes drifts down to the platforms. There's a sense of openness and tranquility here that's rare for a city subway station."
          "The artwork in the station often features maritime themes, with mosaics of waves and sea creatures. It's a subtle nod to the station's unique, coastal location." ]
    | Midday
    | Afternoon ->
        [ "The station sees a mix of commuters and those seeking a waterfront escape. The vibe is more relaxed than in other parts of the city's transit system."
          "On sunny days, the light streams into the station's entrance, creating a bright and airy atmosphere. It feels like a gateway to leisure and recreation."
          "The platforms are a mix of business attire and casual wear, a reflection of the dual nature of the area. It's a place where the city's hustle meets the water's calm." ]
    | Evening ->
        [ "The station is a starting point for evening strolls along the waterfront. The crowd is often made up of couples and friends looking to enjoy the sunset and the skyline views."
          "The energy is calm and pleasant. The station serves as a peaceful transition from the workday to a relaxing evening by the water."
          "As the city lights begin to sparkle across the water, the station becomes a gateway to a romantic and scenic part of the city. The mood is tranquil and beautiful." ]
    | Night
    | Midnight ->
        [ "The station is quiet and peaceful. The distant lights of the skyline, visible from the station's entrance, create a stunning backdrop."
          "The sound of the water is more pronounced at night, a gentle, rhythmic presence. The station is a serene and contemplative space in the late hours."
          "It's a place for a quiet journey home after a peaceful evening. The station's calm atmosphere is a perfect end to a day spent near the water." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is quiet, the platforms populated by a few stylishly dressed individuals. There's an air of artistic contemplation, a calm before the creative storm of the day."
          "Posters for art exhibitions and indie film screenings adorn the walls. The station itself feels like a prelude to the creative experiences that await above ground."
          "The morning light catches the unique architectural details of the station, which often incorporates artistic elements into its design. It's a space that values aesthetics as much as function." ]
    | Midday
    | Afternoon ->
        [ "The station buzzes with a sophisticated energy. The platforms are filled with a mix of artists, designers, and shoppers, all contributing to the neighborhood's chic, creative vibe."
          "The sound of a talented busker often echoes through the station, their music providing a fitting soundtrack for the artistic neighborhood. The crowd is appreciative and often stops to listen."
          "The station is a hub of inspiration. The conversations are about art, fashion, and design, and the energy is one of constant, stylish motion." ]
    | Evening ->
        [ "The station becomes a gateway to the neighborhood's trendy nightlife. The platforms are filled with people heading to gallery openings, chic bars, and exclusive restaurants."
          "The fashion on display is a spectacle in itself. The station is a runway of contemporary styles, a reflection of the neighborhood's status as a center of creative culture."
          "The energy is vibrant and sophisticated. The station is the first stop for a night of art, culture, and high-end socializing." ]
    | Night
    | Midnight ->
        [ "The station is quieter, but the artistic vibe lingers. The remaining travelers are often coming from late-night gallery events or intimate bar conversations."
          "The station's design feels more pronounced in the quiet of the night. The play of light and shadow on the architectural and artistic elements creates a dramatic atmosphere."
          "It's a place for a stylish journey home. The station retains its chic character even in the late hours, a final taste of the neighborhood's creative spirit." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is a reflection of the neighborhood's rich heritage. Murals depicting historical figures and cultural events adorn the walls, telling a story to the morning commuters."
          "A sense of community is palpable, even in the morning rush. People greet each other with a familiar nod, and the station feels like a true neighborhood hub."
          "The air is filled with a mix of languages and the distant sound of music. The station is a microcosm of the diverse and vibrant culture of the area." ]
    | Midday
    | Afternoon ->
        [ "The station is bustling with life. It's a crossroads for locals, tourists, and students, all drawn to the neighborhood's cultural attractions."
          "The sound of street performers, often playing jazz or soul music, drifts down to the platforms. The station is alive with the rhythm and energy of the neighborhood."
          "Posters for community events, concerts, and local markets are everywhere. The station is an information hub, a finger on the pulse of the neighborhood's cultural life." ]
    | Evening ->
        [ "The station is a gateway to the neighborhood's legendary nightlife. People flock to the area for its world-class music venues, theaters, and restaurants."
          "The energy is infectious. The platforms are filled with anticipation and excitement, a crowd eager to experience the authentic cultural offerings of the neighborhood."
          "The station is a vibrant, lively space, echoing with the sounds of laughter, conversation, and the promise of a memorable night out." ]
    | Night
    | Midnight ->
        [ "The station's energy endures late into the night. The rhythm of the neighborhood is still felt in the steady flow of people coming from clubs and late-night eateries."
          "The music and spirit of the neighborhood are carried into the station by the night owls. It's a place that feels alive and soulful, even in the early hours of the morning."
          "The station is a final, vibrant chapter to a night spent immersed in culture. It's a testament to a neighborhood with a deep history and an enduring, lively spirit." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is eerily quiet, a stark contrast to the spectacle it will become. The only activity is the quiet work of cleaning crews, preparing the station for the coming onslaught."
          "The bright, flashy advertisements for Broadway shows seem out of place in the morning calm. The station feels like a stage waiting for its actors and audience."
          "A few tired-looking travelers, perhaps coming from an all-night journey, are the only occupants. The station is in a state of suspended animation, holding its breath before the daily performance begins." ]
    | Midday
    | Afternoon ->
        [ "The station transforms into a chaotic whirlwind of activity. A torrent of tourists, armed with maps and cameras, floods the platforms, their excitement and confusion palpable."
          "The sound is a cacophony of announcements, street performers, and the chatter of a dozen different languages. The station is a sensory overload, a true reflection of the district above."
          "Navigating the station is a challenge in itself. It's a crowded, confusing, but undeniably exciting hub, the epicenter of the city's tourism machine." ]
    | Evening ->
        [ "The station reaches its peak frenzy. The platforms are packed with theater-goers, a sea of people rushing to make their 8 PM curtain call. The energy is electric and slightly frantic."
          "The atmosphere is one of pure, unadulterated excitement. The station is the main artery feeding the heart of the entertainment world, and its pulse is racing."
          "Dressed in their evening best, the crowd is a spectacle in itself. The station is a dazzling, chaotic, and unforgettable part of the Broadway experience." ]
    | Night
    | Midnight ->
        [ "The post-theater rush is a wave of humanity flowing back into the station. The energy is still high, but now it's mixed with the satisfaction of a show well seen."
          "The crowds begin to thin, but the station remains active with those heading to late-night bars or grabbing a final slice of pizza. The district's energy dies down slowly, reluctantly."
          "Even late at night, the station retains a buzz. It's a testament to the relentless energy of the entertainment heart of the city, a place that truly never sleeps." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station itself is a historical landmark, with vintage tiles, ornate ironwork, and period details. The morning light highlights its quiet, understated beauty."
          "The platforms are peaceful and uncrowded. The station feels like a secret, a step back in time away from the modern rush of the city."
          "The commuters are few, and the atmosphere is one of calm and dignity. The station reflects the historic and residential character of the neighborhood it serves." ]
    | Midday
    | Afternoon ->
        [ "The station is a point of interest for history buffs and tourists on walking tours. People often pause to admire the station's preserved architectural features."
          "The pace is noticeably slower and more relaxed here. The station is a tranquil oasis in the city's bustling transit system."
          "The station's historic charm provides a unique and pleasant travel experience. It's a reminder of a different era of public transportation, one of elegance and craftsmanship." ]
    | Evening ->
        [ "The station's soft, warm lighting creates a romantic and nostalgic atmosphere. It's a beautiful starting point for an evening stroll through the historic streets above."
          "The crowd is small and consists mostly of local residents heading home. The station feels like a familiar, welcoming part of the neighborhood."
          "The evening is a quiet, peaceful time in the station. The historic details are imbued with a warm glow, making the space feel intimate and charming." ]
    | Night
    | Midnight ->
        [ "The station is almost silent, its historic character feeling more profound in the quiet of the night. The ornate details are cast in shadow, creating a sense of mystery and timelessness."
          "It feels like a place full of stories, a silent witness to the comings and goings of generations. The station is a peaceful, contemplative space in the late hours."
          "The journey home through this station is a quiet, reflective one. It's a final, beautiful glimpse of the neighborhood's historic charm before the day ends." ]

and private nonExistent _ =
    [ "This station does not exist in New York." ]
