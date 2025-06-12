[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Tokyo.MetroStations

open Duets.Cli.Text.World.Common
open Duets.Entities

let rec description dayMoment descriptor =
    match descriptor with
    | EntertainmentHeart -> entertainmentHeart
    | Creative -> creative
    | BusinessDistrict -> businessDistrict
    | Glitz -> glitz
    | Historic -> historic
    | Cultural -> cultural
    | Industrial -> industrial
    | Luxurious -> luxurious
    | _ -> nonExistent
    |> fun fn -> fn dayMoment

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Shibuya Station yawns awake; cleaners scrub tiled floors while advertising screens glow silently above empty gates."
          "The faint aroma of bakery buns floats through the concourse as early trains hiss in and rattle out."
          "Only a trickle of commuters descend escalators; neon still sleeping outside the glass façade." ]
    | Midday
    | Afternoon ->
        [ "Crowds surge toward Yamanote platforms, their footsteps merging into a steady thunder."
          "Jingles announcing departures overlap with pop songs from nearby shops."
          "Tourists clutching shopping bags gawk at endless signboards written in every major language." ]
    | Evening ->
        [ "Platforms vibrate with the bass from street performers outside Hachikō exit."
          "LED panels flash concert adverts while club-goers in vivid outfits cram last carriage spaces."
          "Turnstiles beep in frenetic cadence, matching the pulse of crossing signals above." ]
    | Night
    | Midnight ->
        [ "The final Saikyō Line departs; a hush falls broken only by a distant sax busker."
          "Cleaning crews in orange overalls glide mops across vast marble aisles."
          "Vending machines hum under neon as late taxis queue beyond shuttered ticket offices." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Harajuku’s small platform smells of drip coffee from neighbouring cafés; fashion students doodle designs while waiting."
          "Hand-drawn posters for indie gigs decorate pillar wraps near the exit." ]
    | Midday
    | Afternoon ->
        [ "Groups in eclectic streetwear discuss pop-up galleries between train chimes."
          "Buskers rehearse acoustic sets at the ticket gates until staff politely nudge them on." ]
    | Evening ->
        [ "Live-house patrons swarm the stairwells, comparing wristbands for tonight’s sets."
          "The platform fills with overlapping perfume scents and camera shutters from influencers livestreaming." ]
    | Night
    | Midnight ->
        [ "Only spray-paint tags glint under dim lights as the station dozes; a lone artist sketches carriage interiors."
          "Soft electronic music leaks from headphones of the last passengers heading home." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Shinjuku’s underground maze channels tidal flows of suits tapping smart passes in perfect rhythm."
          "Announcers deliver punctual bulletins over crisp PA systems; smell of espresso permeates platform edges." ]
    | Midday
    | Afternoon ->
        [ "Queues form outside bento kiosks; briefcases rest against polished columns."
          "Meeting reminders echo from phone speakers as commuters stride through ticket barriers without slowing." ]
    | Evening ->
        [ "Departure boards flicker with delays from evening showers; crowds thin yet remain purposeful."
          "Izakaya flyers handed out near exits lure workers seeking post-shift solace." ]
    | Night
    | Midnight ->
        [ "Security shutters close section by section leaving cavernous corridors eerily quiet."
          "The clack of a single pair of heels resounds off granite walls until swallowed by tunnel darkness." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Ginza Station’s marble floors gleam under chandelier-like LEDs; scent diffusers release subtle yuzu." ]
    | Midday
    | Afternoon ->
        [ "Designer shopping bags jostle for space beside leather briefcases on the Hibiya Line."
          "Announcements are delivered by a calm recorded voice in four languages, accompanied by soft harp chimes." ]
    | Evening ->
        [ "Boutique pop-ups line concourses displaying limited-edition sneakers guarded by velvet ropes."
          "Champagne bars near the exit host patrons glancing at watches while trains rush below." ]
    | Night
    | Midnight ->
        [ "Display cases remain lit, showcasing jewellery to empty halls."
          "A lone janitor steers a polishing robot across flawless tiles." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Asakusa Station smells of incense from nearby Sensō-ji; temple bells echo faintly through tiled tunnels." ]
    | Midday
    | Afternoon ->
        [ "Tour groups clutching paper fortunes navigate old-style wooden signage toward Exit 1."
          "Kimono rental ads line staircases scented by fresh taiyaki." ]
    | Evening ->
        [ "Lantern light spills onto the platform as festival drums begin outside."
          "Workers replace omikuji posters with next week’s event schedules." ]
    | Night
    | Midnight ->
        [ "The station quiets; only distant chanting from late temple ceremonies rides the night air." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Museum staff wheel art crates through Sumida-side platforms preparing for exhibits." ]
    | Midday
    | Afternoon ->
        [ "School trips chatter excitedly while performers busk classical shamisen under vaulted ceilings." ]
    | Evening ->
        [ "Post-show crowds discuss ballet critiques queuing for the Hanzōmon Line." ]
    | Night
    | Midnight ->
        [ "Advertising screens switch to minimalist calligraphy as the corridors empty." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Haneda Airport’s underground station hums with tug carts overhead and distant jet engines." ]
    | Midday
    | Afternoon ->
        [ "Announcements cycle in rapid multilingual succession; baggage workers ride elevators behind glass panels." ]
    | Evening ->
        [ "Pilots stride past vending machines stocked with canned coffee, uniforms crisp under fluorescent light." ]
    | Night
    | Midnight ->
        [ "Cleaning robots glide beside luggage conveyors now resting for dawn flights." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Soft piano loops play near hotel transfer counters decorated with orchid displays." ]
    | Midday
    | Afternoon ->
        [ "Concierges in white gloves escort guests to limousine rendez-vous above Marunouchi Line vestibules." ]
    | Evening ->
        [ "Champagne flutes clink in lounge bars overlooking the ticket hall; ambient jazz drifts downward." ]
    | Night
    | Midnight ->
        [ "Velvet ropes guard closed premium boutiques; security personnel greet departing chauffeurs politely." ]
