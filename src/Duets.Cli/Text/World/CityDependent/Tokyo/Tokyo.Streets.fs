[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Tokyo.Streets

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
        [ "Neon signs over Shibuya Crossing lie dormant, their coloured bulbs waiting for nightfall. Early commuters hurry over the wide intersection while street cleaners gather last night's flyers.";
          "Delivery scooters weave through narrow back-alleys carrying bakery boxes. The giant video screens above the square glow faintly against the pale sky.";
          "Cafés pull up shutters and the scent of fresh pour-overs mingles with exhaust fumes. Only the distant hum of an early train hints at the chaos to come." ]
    | Midday
    | Afternoon ->
        [ "Scramble Crossing erupts in a kaleidoscope of umbrellas, tote bags and camera flashes. Pop-music snippets escape fashion stores that pump out air-conditioning onto the pavement.";
          "Queue lines snake outside crane-game arcades. Tourists practise selfie angles beneath towering billboards of J-pop idols.";
          "A steady roar of conversation, crosswalk beeps and overlapping jingles fills the canyon of glass buildings." ]
    | Evening ->
        [ "Shibuya is ablaze with colour. Animated adverts ripple across skyscraper faces; hundreds of glowing shopfronts compete for attention.";
          "The scent of yakitori and sweet crepes drifts from yokocho alleys while buskers set up amplifiers near the Hachikō statue.";
          "Office workers, shoppers and club-goers merge into one restless river, flowing between bars and live houses." ]
    | Night
    | Midnight ->
        [ "Even past midnight the crossing pulses — smaller crowds but louder laughter. Taxis idle under neon reflections that dance across wet asphalt.";
          "Convenience-store lights paint the streets cyan and magenta. A lone skateboard rattles down Center Gai.";
          "Arcade shutters rattle closed, yet distant bass lines from basement clubs promise the night is far from over." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Cat Street is calm, lined with ivy-clad townhouses converted into studios. Designers sip canned coffee on stoops scrolling through tablets.";
          "Sunlight filters through narrow gaps between low-rise boutiques decorated with hand-drawn signage.";
          "Spray-paint still dries on a new mural while a photographer tests lighting against pastel storefronts." ]
    | Midday
    | Afternoon ->
        [ "Pop-up galleries throw their doors open, showcasing indie fashion drops and vinyl releases. Bicycles with woven baskets lean against brick walls.";
          "Students from nearby art colleges sketch architecture details; the air is thick with espresso and aerosol paint.";
          "Shoppers drift from thrift racks to hidden cafés playing lo-fi beats, trading recommendations for the next underground exhibition." ]
    | Evening ->
        [ "String lights criss-cross the lane, illuminating outdoor racks of vintage jeans. Street photographers capture candid portraits against glowing shop windows.";
          "Small live houses test sound levels, bass vibrating manhole covers while ramen stalls assemble their counters.";
          "Friends compare film photos under hanging lanterns deciding which opening party to crash next." ]
    | Night
    | Midnight ->
        [ "Most boutiques are shuttered, their hand-painted signs soft under sodium lamps. Inside loft studios, silhouettes move behind paper blinds, rushing deadlines.";
          "A guitarist practises in a rehearsal space; the melody spills onto the silent alley before a train rattles overhead.";
          "Cat Street feels like a secret corridor, lit by vending machines and the occasional cyclist heading home." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Shinjuku’s skyscrapers pierce low clouds, glass facades catching the first pink light. Streams of suits queue for drip coffee at corner stands.";
          "Elevators whoosh behind marble lobbies while delivery drones hum between rooftops.";
          "Digital billboards flash stock updates above the silent courtyard of the Tokyo Metropolitan Government Building." ]
    | Midday
    | Afternoon ->
        [ "Lunch hour floods the streets with office workers vying for curry and bento specials; neon OPEN signs flicker atop long ramen lines.";
          "Couriers on folding bikes dart between taxis, their insulated backpacks marked by tech-company logos.";
          "Electronic jingles from convenience stores blend with the echo of distant construction piling." ]
    | Evening ->
        [ "The golden hour paints the skyscrapers bronze. Suits loosen ties, drift toward izakayas advertising happy-hour highballs.";
          "LED ribbons around observation decks ignite, inviting tourists for after-dusk skyline views.";
          "The streets glow with taillights as commuter buses line up like clockwork." ]
    | Night
    | Midnight ->
        [ "Office towers fall dark floor by floor, leaving only cleaning crews and the glow of copy machines.";
          "A handful of taxis cruise through wide avenues, flanked by silent monoliths of glass and steel.";
          "The clack of heels echoes beneath the elevated walkways before settling into stillness." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Ginza’s luxury storefronts glimmer even before opening; security guards polish brass handles.";
          "Delivery trucks unload velvet-draped mannequins while suited concierges practise bows.";
          "The street smells faintly of perfume wafting from automated diffusers outside flagships." ]
    | Midday
    | Afternoon ->
        [ "Designer banners flutter above spotless sidewalks; shoppers drift between boutiques clutching silver-embossed bags.";
          "A parade of high-end cars moves slowly, reflecting mirrored façades of department stores.";
          "Café terraces serve artisanal matcha to influencers filming haul videos." ]
    | Evening ->
        [ "LED chandeliers inside storefronts rival the setting sun. Window displays cycle through choreographed light shows.";
          "Patrons queue for omakase sushi priced like jewellery. Street violins perform classical pieces echoing off marble.";
          "Ginza feels like an open-air gallery of glass and chrome." ]
    | Night
    | Midnight ->
        [ "Most boutiques are shuttered yet illuminated, showcasing watches beneath spotlighted velvet.";
          "A lone limousine idles while chauffeurs chat quietly under building awnings.";
          "Street sweepers glide over granite tiles leaving no trace of the day’s bustle." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Morning sun bathes Sensō-ji’s pagoda in warm gold. Incense smoke curls into crisp air while vendors set out ningyō-yaki cakes.";
          "Rickshaw pullers stretch near Kaminari-mon gate waiting for first customers.";
          "The rhythmic clang of temple bells mingles with distant river traffic." ]
    | Midday
    | Afternoon ->
        [ "Nakamise Street throngs with pilgrims and tourists buying paper charms and yukata.";
          "Vermilion lanterns flutter above stalls selling freshly fried senbei; camera shutters fire endlessly.";
          "Taiko drummers rehearse near the main hall, their beats reverberating through cedar beams." ]
    | Evening ->
        [ "The temple grounds glow under lantern light, casting long shadows across stone paths.";
          "Couples in rental kimono pose for twilight photos against deep-red pillars.";
          "Street food aromas — soy glaze, sweet bean paste — intensify in the cooler air." ]
    | Night
    | Midnight ->
        [ "The gates stand closed, but the incense burners still smoulder faintly.";
          "Asakusa’s shops roll down metal shutters painted with Edo-era scenes.";
          "Cicadas give way to the distant buzz of late trains over Sumida River." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Small theaters along Sumida River advertise matinee kabuki; staff sweep entrances of fallen cherry blossoms.";
          "Museum workers wheel crates through side doors ahead of a new ukiyo-e exhibition.";
          "Elderly locals practise shamisen on park benches facing SkyTree." ]
    | Midday
    | Afternoon ->
        [ "Students sketch riverboats from observation decks; jazz riffs drift from gallery cafés.";
          "Poster stands boast Taiko festivals and flower-arrangement workshops.";
          "Tour groups in coloured caps follow flags past bronze statues of storytellers." ]
    | Evening ->
        [ "Paper lanterns illuminate outdoor stages preparing for rakugo comedy.";
          "Boat restaurants launch, their lantern reflections rippling across the water.";
          "Street performers spin kimonos among tourists recording on smartphones." ]
    | Night
    | Midnight ->
        [ "A hush blankets the river walk; banners flap gently in the breeze.";
          "Museum façades glow subtly, highlighting calligraphy engraved on stone.";
          "Only the muffled announcement of the last cruise echoes across the embankment." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Baggage carts whirr across Haneda’s tarmac; the smell of jet fuel lingers under pale light.";
          "Maintenance crew in fluorescent vests inspect conveyor belts inside vast service tunnels.";
          "Distant take-offs rumble like thunder beyond frosted windows." ]
    | Midday
    | Afternoon ->
        [ "Arriving passengers swarm moving walkways while delivery robots tug freight crates.";
          "Announcements ring out in multiple languages beneath high arched ceilings.";
          "Laser-guided vehicles load catering trucks in neat choreography." ]
    | Evening ->
        [ "Golden sunlight pours through panoramic glass walls facing the runway.";
          "Travellers queue at kiosks for late flights; fluorescent vest crews swap shifts.";
          "Cargo bays flash warning lights as containers roll toward waiting jets." ]
    | Night
    | Midnight ->
        [ "Runway lights twinkle like constellations; a final departure roars into the black sky.";
          "Cleaning bots glide silently across empty lounges.";
          "The terminal hums, half-asleep yet never truly silent." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Valet staff polish chrome handles of five-star hotel entrances along Chuo-dori.";
          "Window cleaners ascend glass towers in motorised gondolas before crowds gather.";
          "The aroma of freshly baked melon-pan drifts from patisserie counters inside marble lobbies." ]
    | Midday
    | Afternoon ->
        [ "Lobby string quartets rehearse softly while concierge desks arrange helicopter transfers.";
          "Guests in designer suits toast with sparkling sake overlooking upscale shopping arcades.";
          "Luxury sedans line curbs, engines idling in near silence." ]
    | Evening ->
        [ "Chandeliers sparkle above gourmet restaurants unveiling seasonal kaiseki menus.";
          "Bellhops guide patrons to rooftop spas glowing aquamarine against city lights.";
          "Perfumed air-conditioning wafts from boutiques displaying limited-edition timepieces." ]
    | Night
    | Midnight ->
        [ "Doormen chat beneath heat lamps as last-minute taxis arrive.";
          "Lobby lighting dims to a golden hush; the grand piano plays to an empty bar.";
          "Ginza’s streets gleam spotless, reflecting the moon between silent towers." ]
