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
        [ "Neon signs over Shibuya Crossing lie dormant, their colored bulbs waiting for nightfall. The wide intersection is a river of early commuters, each step echoing on still-damp pavement. Street cleaners gather last night's flyers, their sweeping brushes the only sound in the hush. Delivery scooters weave through narrow back-alleys carrying bakery boxes, while the giant video screens above the square glow faintly against the pale sky. Cafés pull up shutters and the scent of fresh pour-overs mingles with exhaust fumes. Only the distant hum of an early train hints at the chaos to come, and the city feels like it is holding its breath before the daily surge."
          "The first rays of sunlight reflect off glass towers, illuminating the empty plaza where a few stragglers from the night before finish their canned coffee. The air is crisp, tinged with the faint aroma of grilled yakitori from a late-night stall closing up. A lone jogger crosses the intersection, dodging delivery bikes as the city slowly stirs to life."
          "A lone salaryman stands at the edge of Hachikō Square, sipping vending machine coffee as the city’s pulse begins to quicken. The only sounds are the distant rumble of a subway and the rhythmic swish of sweeping brooms. The neon is muted, but anticipation is palpable in the cool morning air." ]
    | Midday
    | Afternoon ->
        [ "Scramble Crossing erupts in a kaleidoscope of umbrellas, tote bags, and camera flashes. The roar of conversation, crosswalk beeps, and overlapping jingles fills the canyon of glass buildings. Pop-music snippets escape fashion stores that pump out air-conditioning onto the pavement, while queue lines snake outside crane-game arcades. Tourists practice selfie angles beneath towering billboards of J-pop idols, and the scent of sweet crepes and fried chicken drifts from the yokocho alleys."
          "Lunch crowds spill from office towers, weaving between shoppers and schoolkids in uniform. Side streets are alive with the clatter of chopsticks in ramen shops, and the pulse of music from basement clubs. The city feels electric, every surface reflecting light and sound in a dizzying dance."
          "A parade of street fashion and selfie sticks moves through the plaza, while delivery drones buzz overhead. The air is thick with the aroma of takoyaki and the laughter of friends meeting under the gaze of the Hachikō statue. Every moment feels like a scene from a movie, vibrant and fleeting." ]
    | Evening ->
        [ "Shibuya is ablaze with color. Animated adverts ripple across skyscraper faces; hundreds of glowing shopfronts compete for attention. The scent of yakitori and sweet crepes drifts from yokocho alleys while buskers set up amplifiers near the Hachikō statue. Office workers, shoppers, and club-goers merge into one restless river, flowing between bars and live houses. The air vibrates with anticipation, the city’s energy peaking as the sky darkens."
          "The crossing glows under a sea of neon, each sign a beacon for a different adventure. Friends gather on street corners, debating which izakaya or karaoke bar to visit. The city feels infinite, every alley promising a new story as the night unfolds."
          "A wave of music and laughter washes over Center Gai as crowds spill out of arcades and fashion stores. The aroma of grilled skewers and sweet sake mixes with the cool night air. Every window is alive with light, and the city pulses with endless possibility." ]
    | Night
    | Midnight ->
        [ "Even past midnight the crossing pulses—smaller crowds but louder laughter. Taxis idle under neon reflections that dance across wet asphalt. Convenience-store lights paint the streets cyan and magenta, and a lone skateboard rattles down Center Gai. Arcade shutters rattle closed, yet distant bass lines from basement clubs promise the night is far from over."
          "The air is thick with the scent of spilled drinks and fried snacks. A few last revelers linger outside 24-hour ramen shops, trading stories as the city finally begins to slow. Somewhere, a busker plays a melancholy tune, his voice echoing through the emptying streets as Tokyo dreams on."
          "Shibuya’s neon is mirrored in puddles as the last trains depart. A group of friends, arms linked, sing pop songs as they weave past shuttered arcades. The city’s heartbeat slows, but never truly sleeps, its energy lingering in the electric air." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Cat Street is calm, lined with ivy-clad townhouses converted into studios. Designers sip canned coffee on stoops scrolling through tablets, while sunlight filters through narrow gaps between low-rise boutiques decorated with hand-drawn signage. Spray-paint still dries on a new mural while a photographer tests lighting against pastel storefronts. The air is filled with the quiet energy of ideas waiting to take shape."
          "A gentle breeze stirs the banners above indie cafés, and the only sounds are the soft whir of a bicycle and the distant clatter of a skateboard. The street feels like a blank canvas, ready for the day’s inspiration to unfold."
          "A lone illustrator sketches in a sunbeam outside a closed thrift shop, her pencil scratching the only sound in the hush. The scent of fresh bread drifts from a hidden bakery, and the city’s creative spirit is just beginning to wake." ]
    | Midday
    | Afternoon ->
        [ "Pop-up galleries throw their doors open, showcasing indie fashion drops and vinyl releases. Bicycles with woven baskets lean against brick walls, and students from nearby art colleges sketch architecture details. The air is thick with espresso and aerosol paint, while shoppers drift from thrift racks to hidden cafés playing lo-fi beats, trading recommendations for the next underground exhibition."
          "The street is alive with color and sound—laughter from open studio windows, the click of camera shutters, and the hum of creative energy. Every corner promises a surprise, from a new mural to an impromptu street performance."
          "A group of friends debates the merits of vintage film cameras outside a record store, while a DJ tests speakers for a rooftop set. The afternoon sun glints off bicycle bells and the world feels full of possibility." ]
    | Evening ->
        [ "String lights criss-cross the lane, illuminating outdoor racks of vintage jeans. Street photographers capture candid portraits against glowing shop windows, while small live houses test sound levels, bass vibrating manhole covers as ramen stalls assemble their counters. Friends compare film photos under hanging lanterns, deciding which opening party to crash next."
          "The street takes on a magical quality, each window glowing with possibility. The air is fragrant with the scent of fresh ramen and roasting coffee, and the city’s creative spirit feels most alive as night falls."
          "A muralist packs up brushes as a jazz trio tunes up in a nearby café. The laughter of young artists drifts through the night, and the city feels like one big, open studio." ]
    | Night
    | Midnight ->
        [ "Most boutiques are shuttered, their hand-painted signs soft under sodium lamps. Inside loft studios, silhouettes move behind paper blinds, rushing deadlines. A guitarist practices in a rehearsal space; the melody spills onto the silent alley before a train rattles overhead. Cat Street feels like a secret corridor, lit by vending machines and the occasional cyclist heading home."
          "The hush is broken only by the distant thump of a late-night party and the soft glow of a vending machine. The creative pulse of the city never truly sleeps, always ready to spark anew with the sunrise."
          "A lone painter washes brushes in a tiny apartment above the street, her window open to the cool night. The city’s creative dreams linger in the shadows, waiting for morning’s light." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Shinjuku’s skyscrapers pierce low clouds, glass facades catching the first pink light. Streams of suits queue for drip coffee at corner stands, their footsteps echoing on polished marble. Elevators whoosh behind marble lobbies while delivery drones hum between rooftops. Digital billboards flash stock updates above the silent courtyard of the Tokyo Metropolitan Government Building, and the city feels poised for another day of relentless ambition."
          "The air is crisp, tinged with the aroma of fresh pastries and the faint ozone of morning rain. Office towers loom overhead, casting long shadows over the bustling sidewalks below."
          "A steady stream of briefcases and umbrellas moves past the gleaming entrance of a tech incubator. The city’s business heart beats quietly but insistently, the promise of deals and deadlines in every step." ]
    | Midday
    | Afternoon ->
        [ "Lunch hour floods the streets with office workers vying for curry and bento specials; neon OPEN signs flicker atop long ramen lines. Couriers on folding bikes dart between taxis, their insulated backpacks marked by tech-company logos. Electronic jingles from convenience stores blend with the echo of distant construction piling, and the energy is palpable as deals are made over quick lunches."
          "The plazas fill with clusters of colleagues, their laughter mingling with the hum of traffic and the distant chime of a tram. The city’s pulse beats fastest here, every moment measured and purposeful."
          "A line forms outside a convenience store as salarymen check messages on their phones, chopsticks in hand. The air is thick with the scent of curry and the hum of ambition." ]
    | Evening ->
        [ "The golden hour paints the skyscrapers bronze. Suits loosen ties, drifting toward izakayas advertising happy-hour highballs. LED ribbons around observation decks ignite, inviting tourists for after-dusk skyline views. The streets glow with taillights as commuter buses line up like clockwork, and the air is thick with the promise of release after a long day."
          "The city’s business heart softens, its steel and glass facades reflecting the warm glow of sunset and the laughter of workers finally at ease."
          "A group of colleagues gathers at a yakitori stand, laughter echoing between glass towers. The city’s energy shifts from urgency to celebration as night falls." ]
    | Night
    | Midnight ->
        [ "Office towers fall dark floor by floor, leaving only cleaning crews and the glow of copy machines. A handful of taxis cruise through wide avenues, flanked by silent monoliths of glass and steel. The clack of heels echoes beneath the elevated walkways before settling into stillness, and the city’s relentless pace finally slows to a hush."
          "The business district is transformed, its energy spent, waiting for the next sunrise to spark it back to life."
          "A lone security guard makes rounds past silent fountains, the only movement in a landscape of glass and shadow. The pulse of the city is a whisper, resting for tomorrow’s rush." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Ginza’s luxury storefronts glimmer even before opening; security guards polish brass handles, and delivery trucks unload velvet-draped mannequins while suited concierges practice bows. The street smells faintly of perfume wafting from automated diffusers outside flagships, and the silence is broken only by the soft click of designer heels on spotless pavement."
          "The city’s most glamorous avenue feels like a stage before the show, every detail meticulously prepared for the day’s parade of elegance."
          "A florist arranges perfect bouquets behind a frosted window, while a lone jogger in designer sweats glides past art deco façades. The morning air is cool and scented with luxury." ]
    | Midday
    | Afternoon ->
        [ "Designer banners flutter above spotless sidewalks; shoppers drift between boutiques clutching silver-embossed bags. A parade of high-end cars moves slowly, reflecting mirrored façades of department stores. Café terraces serve artisanal matcha to influencers filming haul videos, and the air is filled with the soft murmur of polite conversation and the clink of fine china."
          "The sunlight dances on polished chrome and glass, and every window promises a glimpse of luxury and aspiration."
          "A pair of tourists marvel at a robotic window display, while a magazine shoot unfolds on a marble staircase. The city’s elegance is effortless and ever-present." ]
    | Evening ->
        [ "LED chandeliers inside storefronts rival the setting sun. Window displays cycle through choreographed light shows, and patrons queue for omakase sushi priced like jewelry. Street violins perform classical pieces echoing off marble, and Ginza feels like an open-air gallery of glass and chrome."
          "The city’s elite gather for cocktails on rooftop terraces, their laughter mingling with the distant hum of traffic below. The night air is perfumed with the scent of blooming lilies and expensive cologne."
          "A luxury car pulls up to a velvet-roped entrance, its headlights reflected in the glassy towers above. The city glows with promise and sophistication as the evening deepens." ]
    | Night
    | Midnight ->
        [ "Most boutiques are shuttered yet illuminated, showcasing watches beneath spotlighted velvet. A lone limousine idles while chauffeurs chat quietly under building awnings. Street sweepers glide over granite tiles, leaving no trace of the day’s bustle."
          "The avenue is serene, its opulence undimmed by the hour, and the city’s dreams shimmer in the reflections of moonlight on glass."
          "A stray cat pads silently past a jewelry display, its emerald collar glinting in the lamplight. The city’s luxury is eternal, glowing long after the crowds have gone." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Morning sun bathes Sensō-ji’s pagoda in warm gold. Incense smoke curls into crisp air while vendors set out ningyō-yaki cakes. Rickshaw pullers stretch near Kaminari-mon gate waiting for first customers, and the rhythmic clang of temple bells mingles with distant river traffic. The ancient stones feel cool underfoot, and the city’s history breathes in the quiet."
          "Cherry blossoms drift onto the temple steps, and the only sounds are the soft murmur of prayers and the flutter of pigeons in the courtyard."
          "A local priest sweeps fallen petals from the shrine path, pausing to greet a passing jogger. The morning is peaceful, steeped in centuries of tradition." ]
    | Midday
    | Afternoon ->
        [ "Nakamise Street throngs with pilgrims and tourists buying paper charms and yukata. Vermilion lanterns flutter above stalls selling freshly fried senbei, and camera shutters fire endlessly. Taiko drummers rehearse near the main hall, their beats reverberating through cedar beams, while the scent of sweet bean paste and soy glaze fills the air."
          "Guides lead groups beneath ancient gates, telling stories of samurai and merchants as the city’s past mingles with the present in every step."
          "A group of schoolchildren giggles as they try on festival masks, their teacher snapping photos beneath a row of fluttering banners. The air is alive with history and joy." ]
    | Evening ->
        [ "The temple grounds glow under lantern light, casting long shadows across stone paths. Couples in rental kimono pose for twilight photos against deep-red pillars, and street food aromas—soy glaze, sweet bean paste—intensify in the cooler air. The sound of distant festival drums lingers, and the city’s spirit feels timeless in the gathering dusk."
          "The air is filled with the gentle laughter of families and the rustle of silk, as history and modernity embrace beneath the stars."
          "A storyteller in Edo-era costume spins tales for a small crowd, lanterns flickering as dusk settles. The city’s heritage feels close, vibrant, and alive." ]
    | Night
    | Midnight ->
        [ "The gates stand closed, but the incense burners still smolder faintly. Asakusa’s shops roll down metal shutters painted with Edo-era scenes, and cicadas give way to the distant buzz of late trains over Sumida River. The ancient stones are cool and silent, holding the secrets of centuries in their shadows."
          "The temple grounds are peaceful, watched over by stone lanterns and the soft glow of moonlight on tiled roofs."
          "A lone cat slips between the pillars, its silhouette framed by the last glow of lanterns. The city’s history whispers in the quiet, waiting for dawn." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Small theaters along Sumida River advertise matinee kabuki; staff sweep entrances of fallen cherry blossoms. Museum workers wheel crates through side doors ahead of a new ukiyo-e exhibition, and elderly locals practice shamisen on park benches facing SkyTree. The city’s artistic heart beats quietly in the soft morning light."
          "The riverbank is still, save for the occasional jogger and the distant laughter of schoolchildren on a field trip. The promise of creativity lingers in the air."
          "A troupe of dancers rehearses beneath a cherry tree, their movements mirrored in the calm water. The city’s culture awakens gently, full of anticipation." ]
    | Midday
    | Afternoon ->
        [ "Students sketch riverboats from observation decks, jazz riffs drift from gallery cafés, and poster stands boast Taiko festivals and flower-arrangement workshops. Tour groups in colored caps follow flags past bronze statues of storytellers, and the air is alive with the excitement of discovery."
          "The city’s cultural tapestry is on full display—every corner offers a new performance, a new story, a new inspiration."
          "A poet reads haiku to a small audience in a riverside park, while a food stall serves matcha ice cream to a line of eager children. The afternoon is vibrant with art and life." ]
    | Evening ->
        [ "Paper lanterns illuminate outdoor stages preparing for rakugo comedy. Boat restaurants launch, their lantern reflections rippling across the water, while street performers spin kimonos among tourists recording on smartphones. The city feels festive, every sound and sight a celebration of creativity."
          "The river shimmers with light, and the laughter of audiences drifts through the air, blending with the music of the city’s soul."
          "A jazz quartet sets up on a floating platform, their melodies mingling with the scent of grilled fish from a nearby yatai. The night is alive with culture and joy." ]
    | Night
    | Midnight ->
        [ "A hush blankets the river walk; banners flap gently in the breeze. Museum facades glow subtly, highlighting calligraphy engraved on stone, and only the muffled announcement of the last cruise echoes across the embankment. The city’s creative energy settles into a peaceful quiet, ready to awaken again with the dawn."
          "The night is soft, filled with the promise of new stories waiting to be told."
          "A lone saxophonist plays beneath a bridge, his notes drifting over the water. The city’s creativity lingers, gentle and unhurried." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Baggage carts whirr across Haneda’s tarmac; the smell of jet fuel lingers under pale light. Maintenance crew in fluorescent vests inspect conveyor belts inside vast service tunnels, and distant take-offs rumble like thunder beyond frosted windows. The airport is a hive of activity, every movement choreographed to the minute."
          "The city’s industrial engine hums quietly, its gears turning as the rest of Tokyo wakes up."
          "A line of taxis idles outside a warehouse as dockworkers unload crates, their voices muffled by the roar of engines. The morning is brisk, purposeful, and full of motion." ]
    | Midday
    | Afternoon ->
        [ "Arriving passengers swarm moving walkways while delivery robots tug freight crates. Announcements ring out in multiple languages beneath high arched ceilings, and laser-guided vehicles load catering trucks in neat choreography. The air is filled with the scent of jet fuel and the promise of new journeys."
          "The industrial district is alive with motion, every process running with precision and purpose."
          "A mechanic wipes sweat from his brow as forklifts zip by, the clang of metal and the hiss of hydraulics creating a symphony of work. The sun glints off endless rows of shipping containers." ]
    | Evening ->
        [ "Golden sunlight pours through panoramic glass walls facing the runway. Travelers queue at kiosks for late flights, fluorescent vest crews swap shifts, and cargo bays flash warning lights as containers roll toward waiting jets. The day’s work winds down, but the city’s machinery never truly stops."
          "The hum of engines and the glow of runway lights create a sense of endless possibility, as Tokyo connects with the world beyond."
          "A supervisor checks manifests under the orange glow of sodium lamps, while the distant roar of a departing jet signals the end of another shift. The evening is restless and industrious." ]
    | Night
    | Midnight ->
        [ "Runway lights twinkle like constellations; a final departure roars into the black sky. Cleaning bots glide silently across empty lounges, and the terminal hums, half-asleep yet never truly silent. The city’s industrial heart beats on, even as the world outside sleeps."
          "The stillness is punctuated only by the distant echo of a jet, a reminder that the work of the city never truly ends."
          "A lone vending machine glows at the edge of the tarmac, its hum joining the chorus of distant engines. The night is quiet but never empty." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Valet staff polish chrome handles of five-star hotel entrances along Chuo-dori. Window cleaners ascend glass towers in motorized gondolas before crowds gather, and the aroma of freshly baked melon-pan drifts from patisserie counters inside marble lobbies. The city’s luxury is understated but ever-present, every detail attended to with care."
          "The morning sun glints off gilded signage, and the quiet is broken only by the soft footsteps of early guests and the gentle hum of a grand piano tuning up for the day."
          "A bellhop arranges fresh orchids in the lobby, pausing to admire the sunrise over a rooftop garden. The city’s elegance is quiet, but unmistakable." ]
    | Midday
    | Afternoon ->
        [ "Lobby string quartets rehearse softly while concierge desks arrange helicopter transfers. Guests in designer suits toast with sparkling sake overlooking upscale shopping arcades, and luxury sedans line curbs, engines idling in near silence. The air is cool and perfumed, every moment choreographed for comfort and elegance."
          "The city’s most exclusive addresses are alive with quiet activity, each guest treated like royalty."
          "A chef in a tall hat inspects a tray of wagyu beef as a sommelier polishes crystal glasses. The afternoon is a symphony of service and refinement." ]
    | Evening ->
        [ "Chandeliers sparkle above gourmet restaurants unveiling seasonal kaiseki menus. Bellhops guide patrons to rooftop spas glowing aquamarine against city lights, and perfumed air-conditioning wafts from boutiques displaying limited-edition timepieces. The city’s luxury is on full display, every experience curated for delight."
          "The night air shimmers with anticipation, and the city feels like a jewel box, waiting to be opened."
          "A limousine glides to a stop at a private entrance, its door opened by a white-gloved attendant. The evening is rich with promise and quiet grandeur." ]
    | Night
    | Midnight ->
        [ "Doormen chat beneath heat lamps as last-minute taxis arrive. Lobby lighting dims to a golden hush; the grand piano plays to an empty bar. Ginza’s streets gleam spotless, reflecting the moon between silent towers. The city’s luxury never sleeps, always ready to welcome the next guest."
          "The quiet elegance of the night is a reminder that in Tokyo, even the smallest details are a work of art."
          "A concierge arranges tomorrow’s breakfast menu as the city outside glows silver and gold. The night is peaceful, wrapped in comfort and style." ]
