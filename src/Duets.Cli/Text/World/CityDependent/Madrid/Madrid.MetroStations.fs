[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Madrid.MetroStations

open Duets.Cli.Text.World.Common
open Duets.Entities

let rec description dayMoment descriptor =
    match descriptor with
    | Bohemian -> bohemian
    | BusinessDistrict -> businessDistrict
    | Creative -> creative
    | Coastal -> nonExistent
    | Cultural -> cultural
    | EntertainmentHeart -> entertainmentHeart
    | Glitz -> glitz
    | Historic -> historic
    | Industrial -> industrial
    | Luxurious -> luxurious
    | Nature -> nature
    |> fun fn -> fn dayMoment

and private bohemian dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The metro station hums with quiet creativity: musicians tune their instruments and artists arrange their supplies while early commuters pass by."
          "Colorful murals and posters line the walls, and the scent of fresh coffee drifts from a nearby kiosk."
          "A handful of artists sketch in notebooks, inspired by the unique energy of the morning crowd."
          "The gentle sound of guitar or violin can sometimes be heard echoing through the corridors." ]
    | Midday
    | Afternoon ->
        [ "The station is lively and diverse, with students, artists, and tourists sharing the platforms."
          "Street performers entertain waiting passengers, and the hum of conversation fills the air."
          "Bright murals and creative advertisements add color to the underground space."
          "The energy is vibrant, as people discuss art, music, and upcoming events." ]
    | Evening ->
        [ "The station buzzes with activity as people head to evening shows and gatherings."
          "Live music and impromptu performances attract small crowds, and the atmosphere is relaxed and welcoming."
          "Artists and musicians mingle with commuters, sharing stories and laughter."
          "The sound of applause and music lingers in the air as trains come and go." ]
    | Night
    | Midnight ->
        [ "A few late-night performers entertain the last travelers, their music echoing in the quiet station."
          "The creative spirit remains, even as the crowds thin and the lights dim."
          "Murals and artwork seem to glow under the soft lighting, inspiring those who pass by."
          "The distant sound of a saxophone or guitar drifts through the corridors, a final note before the station sleeps." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is filled with professionals in suits, clutching coffee and briefcases as they rush to work."
          "Announcements echo off the polished tiles, and the air is brisk and purposeful."
          "Vendors sell quick breakfasts to commuters on the go, and the hum of conversation is low and steady."
          "The scent of fresh pastries and espresso lingers near the entrances." ]
    | Midday
    | Afternoon ->
        [ "Lunch crowds move through the station, filling nearby cafes and shops with lively chatter."
          "Business meetings spill over onto the platforms, and the sound of phone calls and footsteps is constant."
          "The atmosphere is busy but efficient, with everyone moving quickly to their next destination."
          "The station feels modern and clean, a reflection of the district above." ]
    | Evening ->
        [ "The pace slows as workers head home, and the station is quieter except for a few after-work groups."
          "The glow of overhead lights reflects off glass and metal, and the sound of laughter drifts from nearby bars."
          "A sense of relief and relaxation settles over the commuters as the workday ends."
          "The station feels spacious and calm, a welcome change from the daytime rush." ]
    | Night
    | Midnight ->
        [ "The station is nearly empty, lit by security lights and the occasional passing train."
          "A lone janitor sweeps the floor, and the silence is broken only by the distant sound of footsteps."
          "The air is cool and still, with only a handful of late-night travelers passing through."
          "The businesslike atmosphere remains, but the energy is subdued and peaceful." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Creative commuters carry sketchbooks and instruments, their faces bright with anticipation for the day ahead."
          "Posters for concerts and exhibitions line the walls, and the scent of fresh paint lingers in the air."
          "A few musicians practice quietly in corners, their melodies blending with the sounds of arriving trains."
          "The station feels like a canvas, waiting to be filled with the day's inspiration." ]
    | Midday
    | Afternoon ->
        [ "The station is alive with artistic energy: groups of friends discuss projects, and laughter echoes down the platforms."
          "Street performers and dancers entertain travelers, drawing small crowds with their talent."
          "The walls are decorated with rotating art displays, and the air buzzes with conversation."
          "The creative spirit is contagious, inspiring even the most hurried commuters." ]
    | Evening ->
        [ "Evening events fill the station with excitement: people in costumes and musicians with cases gather for shows."
          "The station glows with neon lights and the sound of live music drifts from nearby venues."
          "Artists and fans mingle, sharing stories and plans for the night ahead."
          "The air is electric with anticipation, and every corner seems to hold a new surprise." ]
    | Night
    | Midnight ->
        [ "A few artists linger, packing up supplies after a long day of creativity."
          "The station is quiet, but the echoes of music and laughter remain."
          "The walls seem to hold the day's inspiration, and the silence is filled with possibility."
          "The distant sound of a piano or guitar can sometimes be heard, a gentle end to the creative day." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station opens to early visitors heading to museums and theaters, their conversations filled with anticipation."
          "Historic posters and banners decorate the walls, and the air is filled with the promise of discovery."
          "Tour guides gather small groups, ready to explore the city's cultural treasures."
          "The scent of fresh pastries and coffee drifts from nearby cafes, inviting travelers to linger." ]
    | Midday
    | Afternoon ->
        [ "The platforms are busy with tourists and locals heading to exhibitions and performances."
          "Cultural events are advertised on digital screens, and the sound of guided tours echoes through the station."
          "Cafes are filled with lively discussions about art, history, and literature."
          "The energy is cosmopolitan and curious, inspiring travelers to explore more." ]
    | Evening ->
        [ "The station is alive with people heading to evening shows and cultural gatherings."
          "Live music and performances attract crowds, and the air is filled with excitement and applause."
          "The glow of theater marquees and concert posters lights up the corridors."
          "The sense of culture and creativity is strongest now, as the city comes alive for the night." ]
    | Night
    | Midnight ->
        [ "The station quiets as venues close, but a few late-night visitors linger, discussing the day's performances."
          "Streetlights cast a warm glow on posters and murals, and the echoes of music linger in the air."
          "The sense of history and culture remains, even as the city sleeps."
          "The sound of a distant violin or cello can sometimes be heard, a gentle reminder of the day's inspiration." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is quiet, recovering from the previous night's festivities."
          "Cleaning crews tidy up, and a few early risers pass through, evidence of last night's revelry still visible."
          "Neon signs flicker as the city prepares for another day of entertainment."
          "The scent of cleaning products and fresh air fills the station, a sign of renewal." ]
    | Midday
    | Afternoon ->
        [ "Shops, bars, and theaters are open, and the station is filled with the sound of music and laughter."
          "Tourists and locals mingle, and street performers entertain small crowds on the platforms."
          "The energy builds as people make plans for the evening, and the anticipation of nightlife is already in the air."
          "The scent of popcorn and sweet treats drifts from concession stands, enticing travelers to linger." ]
    | Evening ->
        [ "The area comes alive: music pours from open doors, and crowds gather for shows and parties."
          "Bright lights and lively conversation fill the station, and the excitement is contagious."
          "Queues form outside popular venues, and the pulse of Madrid's entertainment scene is unmistakable."
          "The air is thick with anticipation, and every train seems to bring more revelers." ]
    | Night
    | Midnight ->
        [ "The party is in full swing: music, laughter, and the clink of glasses echo down the corridors."
          "Clubs and bars are packed, and the station is a blur of movement and color until the early hours."
          "Even as venues begin to close, groups of friends linger, reluctant to let the night end."
          "The sound of music and laughter drifts from a nearby bar, a reminder that the night is still young." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station sparkles with polished floors and glass, welcoming early commuters with a sense of luxury."
          "Boutiques and high-end shops open their doors, their displays glittering in the morning light."
          "A few luxury cars glide past the entrances, and the air is filled with the subtle scent of expensive perfume."
          "The sound of designer heels and polite conversation fills the platforms." ]
    | Midday
    | Afternoon ->
        [ "The station is busy with well-dressed travelers and tourists admiring the latest fashions and jewelry."
          "Cafes serve gourmet lunches to stylish patrons, and the atmosphere is one of sophistication and exclusivity."
          "Sunlight reflects off glass and gold, and the buzz of commerce is steady and refined."
          "The sound of champagne corks and laughter fills the air, as people celebrate special occasions." ]
    | Evening ->
        [ "Elegant lights illuminate the station, and the windows of boutiques and restaurants glow invitingly."
          "Evening shoppers and diners stroll past, and the sense of luxury is heightened by the soft glow of chandeliers and lanterns."
          "The station feels like a stage, with everyone dressed for an evening out in Madrid's most glamorous district."
          "The air is filled with the scent of fine dining and expensive perfume." ]
    | Night
    | Midnight ->
        [ "The station is quiet, with only a few late-night taxis and the distant hum of the city."
          "Shop windows are dark, but the elegance remains in the architecture and the gentle lighting."
          "A sense of exclusivity lingers, and the station feels both peaceful and opulent."
          "The sound of a lone violinist drifts from a nearby platform, adding a touch of magic to the night air." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Classic tilework and old signage greet early travelers, the station's history evident in every detail."
          "The sound of footsteps echoes through arched corridors, and the air is cool and still."
          "A few locals pause to admire the architecture on their way to work."
          "The scent of fresh bread and coffee drifts from a nearby bakery, inviting travelers to linger." ]
    | Midday
    | Afternoon ->
        [ "The platforms are busy with tourists and locals exploring the station's historic features."
          "Guided tours and school groups move between exhibits, and the sense of history is everywhere."
          "Shaded benches offer a respite from the bustle, and the sound of conversation fills the air."
          "The air is filled with the scent of old paper and polished wood, a reminder of the station's storied past." ]
    | Evening ->
        [ "Golden hour bathes the station in warm light, and the sound of conversation and laughter echoes through the halls."
          "Locals and visitors gather at old cafes, sharing stories and enjoying the atmosphere."
          "The past feels close, and every corner seems to hold a secret waiting to be discovered."
          "The smell of pastries and coffee lingers, enticing people to linger a little longer." ]
    | Night
    | Midnight ->
        [ "The station is quiet and mysterious, with shadows stretching across old tiles and the distant sound of trains."
          "Soft lighting highlights architectural details, and the air is filled with the scent of night-blooming flowers."
          "The sense of history is strongest now, as if the station itself is dreaming."
          "The sound of a lone guitarist drifts from a nearby platform, adding a touch of melancholy to the empty station." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station opens early for workers, the air filled with the sounds of machinery and delivery carts."
          "Workers in uniforms gather at the gates, and the scent of oil and metal is strong."
          "The station is practical and utilitarian, with little decoration but a sense of purpose."
          "The smell of coffee and diesel fuel wafts through the air, a sign of the industrial day ahead." ]
    | Midday
    | Afternoon ->
        [ "The platforms are busy with workers and vehicles, the noise of industry constant and rhythmic."
          "Lunch breaks bring groups of workers to small canteens, their conversations lively but brief."
          "The area feels efficient and focused, with everyone moving quickly between tasks."
          "The sound of machinery and construction adds to the bustling atmosphere." ]
    | Evening ->
        [ "Most businesses close, but a few lights remain in workshops where overtime is being worked."
          "The station is quieter now, with only the occasional train passing by or a group of workers heading home."
          "The smell of metal and oil lingers, and the sense of industry is ever-present."
          "The sound of a lone engine drifts from a nearby platform, a reminder that work continues late into the night." ]
    | Night
    | Midnight ->
        [ "The industrial area is almost deserted, its platforms lit by harsh security lights and the occasional passing train."
          "The silence is broken only by the distant hum of machines and the echo of footsteps on concrete."
          "The station feels stark and utilitarian, with little sign of life until morning returns."
          "The sound of a lone crane drifts from a nearby construction site, adding to the industrial atmosphere." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station glows with polished marble and gold accents, welcoming early travelers with a sense of elegance."
          "A few well-dressed commuters enjoy the peace, and the scent of fresh flowers fills the air."
          "The air is fresh, and the sense of luxury is unmistakable."
          "The sound of soft music drifts from hidden speakers, adding to the refined atmosphere." ]
    | Midday
    | Afternoon ->
        [ "Luxury shops and gourmet cafes are busy with guests and staff, their entrances bustling with activity."
          "The platforms are filled with the sound of polite conversation and the clink of fine china from terrace cafes."
          "The overall feeling is one of comfort, privilege, and quiet sophistication."
          "The sound of champagne corks and laughter fills the air, as people celebrate special occasions." ]
    | Evening ->
        [ "Golden lights illuminate the station, and the platforms are lively with well-dressed travelers and guests arriving for events."
          "Soft music drifts from open cafes, and the air is filled with the scent of gourmet cuisine."
          "The station feels festive yet exclusive, a place to see and be seen."
          "The smell of fine dining and expensive perfume fills the air, as people gather to celebrate the night ahead." ]
    | Night
    | Midnight ->
        [ "The station is peaceful and quiet, with only the occasional traveler or attendant visible."
          "The soft lighting highlights marble and gold details, and the sense of luxury endures even after dark."
          "A feeling of safety and exclusivity lingers, making it a restful end to the day."
          "The sound of a lone violinist drifts from a nearby platform, adding a touch of magic to the night air." ]

and private nature dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is surrounded by parks and gardens, the air fresh and filled with birdsong."
          "Joggers and dog-walkers enjoy the green spaces before their commute, and the scent of grass and flowers is everywhere."
          "The platforms are quiet and green, a peaceful retreat from the city."
          "The smell of freshly cut grass and blooming flowers wafts through the air, enticing travelers to linger." ]
    | Midday
    | Afternoon ->
        [ "Families and friends gather on benches and lawns, picnicking and playing games in the shade."
          "The station is lively with cyclists, strollers, and children chasing each other under the trees."
          "The sound of laughter and the sight of blooming flowers make the area feel vibrant and alive."
          "The smell of barbecues and outdoor cooking wafts through the air, enticing people to stop and sample the local cuisine." ]
    | Evening ->
        [ "The park glows in the golden light of sunset, and couples stroll along winding paths."
          "Birdsong gives way to the gentle hum of insects, and the air cools as the day ends."
          "The platforms are tranquil, a perfect place to unwind and reflect."
          "The smell of food and drink wafts through the air, as people gather to celebrate the night ahead." ]
    | Night
    | Midnight ->
        [ "The green spaces are quiet and shadowy, with only the sound of the breeze in the trees and the distant city beyond."
          "Soft lighting casts gentle pools of light on the platforms, and the scent of night-blooming flowers lingers."
          "The peacefulness is profound, and the sense of nature remains even as the city sleeps."
          "The sound of a lone owl drifts from a nearby tree, adding a touch of mystery to the night air." ]
