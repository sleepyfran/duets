[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.NewYork.Streets

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
        [ "Towering skyscrapers cast long shadows over avenues bustling with yellow cabs. The air is crisp with the smell of coffee and ambition as professionals in sharp suits march purposefully along the sidewalks."
          "The street is a canyon of steel and glass, echoing with the determined footsteps of the city's workforce. Newsstands and coffee carts do a brisk business, fueling the engine of commerce."
          "Sunlight glints off the high-rise windows, creating a dazzling but impersonal cityscape. The roar of traffic is a constant, powerful hum, the city's mechanical heartbeat." ]
    | Midday
    | Afternoon ->
        [ "The streets are a flurry of activity as office workers spill out for lunch. The aroma of street food from various carts mingles in the air, offering a brief, delicious respite from the corporate grind."
          "The midday sun beats down on the pavement, but the shadows of the colossal buildings offer some relief. The pace is still brisk, a testament to a city that never truly stops for a break."
          "Corporate logos gleam from polished facades. The energy is palpable, a mixture of high-stakes deals, hurried lunches, and the relentless forward momentum of a global financial hub." ]
    | Evening ->
        [ "The river of commuters begins to flow homeward, a sea of tired but satisfied faces. The golden hour light softens the hard edges of the skyscrapers, painting the sky in hues of orange and pink."
          "The traffic thickens as the workday ends, a symphony of car horns and brake lights. Office windows begin to darken, one by one, as the district prepares for a brief slumber."
          "A different crowd begins to emerge, heading for after-work drinks at sleek, modern bars. The energy shifts from corporate hustle to sophisticated leisure." ]
    | Night
    | Midnight ->
        [ "The streets are eerily quiet, the towering buildings now dark silhouettes against the night sky. The only sounds are the hum of ventilation systems and the distant wail of a siren."
          "Streetlights cast long, lonely shadows on the empty sidewalks. The once-bustling avenues are now deserted, creating a sense of immense, silent power."
          "A few windows remain lit, beacons of late-night work or cleaning crews restoring order. The city's commercial heart rests, gathering strength for the dawn." ]

and private coastal dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "A fresh, salty breeze blows in from the water, a refreshing contrast to the urban landscape. Joggers and dog-walkers populate the waterfront promenade, enjoying the calm before the city fully awakens."
          "The morning sun glitters on the water's surface, with the iconic skyline and bridges forming a breathtaking backdrop. The sound of gentle waves provides a soothing soundtrack."
          "Ferries begin their daily routes, gliding across the water. The air is filled with a sense of peace and open space, a rare commodity in this dense city." ]
    | Midday
    | Afternoon ->
        [ "People lounge on benches, enjoying the sun and the spectacular views. The waterfront is a popular spot for a leisurely lunch or a moment of tranquility away from the city's chaos."
          "The sun is high in the sky, warming the piers and pathways. The sound of children's laughter and the cry of gulls fills the air, creating a relaxed, holiday-like atmosphere."
          "The promenade is alive with a mix of locals and tourists, all drawn to the water's edge. The constant movement of boats and ships adds to the dynamic, scenic view." ]
    | Evening ->
        [ "The setting sun paints the sky with vibrant colors, reflecting beautifully on the water. The city lights begin to twinkle across the river, creating a magical, romantic atmosphere."
          "The evening air is cool and pleasant. Couples stroll hand-in-hand along the waterfront, and the mood is peaceful and contemplative as the day winds down."
          "The skyline transforms into a glittering tapestry of lights. The view is mesmerizing, a perfect end to the day, with the gentle sounds of the water in the background." ]
    | Night
    | Midnight ->
        [ "The city's reflection shimmers on the dark water, a dazzling display of urban beauty. The waterfront is quiet, offering a serene and stunning perspective of the city that never sleeps."
          "The distant hum of the city is a constant companion, but here at the water's edge, there is a profound sense of peace. The vastness of the water and the sky is humbling."
          "Under the moonlight, the bridges are majestic silhouettes. It's a place for quiet thoughts and late-night conversations, with the city's grandeur laid out before you." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The cobblestone streets are quiet, still holding the cool of the night. The cast-iron facades of the buildings stand as silent witnesses to the neighborhood's artistic soul, waiting for the day's inspiration to strike."
          "Gallery doors are just beginning to open, and the smell of fresh coffee wafts from chic cafes. There's a palpable sense of potential in the air, a quiet hum of creativity about to be unleashed."
          "The morning light filters through the large loft windows, illuminating studios filled with canvases and sculptures. It's a peaceful, expectant moment before the artistic hustle begins." ]
    | Midday
    | Afternoon ->
        [ "The streets are now a runway for the fashionable and the artistic. High-end boutiques and art galleries attract a stylish crowd, and the sidewalks buzz with sophisticated chatter."
          "Street artists showcase their talents, adding to the vibrant, expressive atmosphere. The neighborhood is a living gallery, with inspiration to be found around every corner."
          "The architecture itself is a work of art, with intricate details and historic charm. The mix of old and new, art and commerce, creates a uniquely dynamic and inspiring environment." ]
    | Evening ->
        [ "The neighborhood transforms as trendy bars and restaurants come to life. The warm glow from their windows spills onto the streets, creating an inviting and chic atmosphere."
          "Art gallery openings are in full swing, with crowds spilling out onto the sidewalks, wine glasses in hand. The air is filled with conversation, laughter, and the exchange of creative ideas."
          "The energy is sophisticated and vibrant. It's a place to see and be seen, a hub of contemporary culture where art, fashion, and nightlife intersect seamlessly." ]
    | Night
    | Midnight ->
        [ "The streets are quieter now, but a stylish energy still lingers. The cobblestones gleam under the streetlights, and the cast-iron buildings are imbued with a timeless, dramatic beauty."
          "The warm light from the remaining open bars creates intimate, inviting scenes. It's a time for quiet conversations and soaking in the neighborhood's unique, artistic ambiance."
          "Shadows play on the historic facades, highlighting their intricate details. The night offers a more personal, contemplative experience of this creative heart of the city." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The neighborhood awakens with a rich tapestry of sounds and smells. The aroma of soul food begins to drift from kitchens, and the distant sound of a gospel choir warms the morning air."
          "The streets are relatively quiet, but there's a deep-rooted sense of community. Locals greet each other on their way to work, and the murals on the walls tell stories of a proud heritage."
          "Historic brownstones stand with quiet dignity. The morning is a time of peaceful preparation, a calm before the vibrant energy of the day fully takes hold." ]
    | Midday
    | Afternoon ->
        [ "The streets are alive with a vibrant mix of people, music, and commerce. Street vendors sell their wares, and the sidewalks are bustling with the energy of a diverse and thriving community."
          "The neighborhood is a feast for the senses. The sound of jazz, the sight of colorful murals, and the taste of authentic cuisine create an immersive cultural experience."
          "There's a palpable rhythm to the neighborhood, a beat that's both historic and contemporary. It's a place of constant motion, conversation, and connection." ]
    | Evening ->
        [ "The sounds of live music begin to spill from the open doors of jazz clubs and bars. The neighborhood's legendary nightlife starts to awaken, drawing in crowds with its promise of authentic entertainment."
          "Restaurants are filled with the sound of laughter and lively conversation. The atmosphere is warm, welcoming, and full of soul, a true reflection of the neighborhood's spirit."
          "People gather on stoops and street corners, the community itself a form of living theater. The evening is a time for socializing, celebrating, and enjoying the rich cultural fabric of the area." ]
    | Night
    | Midnight ->
        [ "The music and energy from the clubs and bars create a vibrant soundtrack for the night. The neighborhood is still very much alive, a beacon of culture and nightlife."
          "The streets are still populated with people moving between venues, their laughter and conversations adding to the lively atmosphere. The sense of community is strong, even late into the night."
          "The night has a rhythm of its own here. It's a place that feels both exciting and familiar, a neighborhood that wears its history and its heart on its sleeve." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The giant electronic billboards are still, their dazzling lights dormant. The streets are surprisingly calm, with only delivery trucks and cleaning crews preparing the stage for the day's spectacle."
          "There's a sense of quiet anticipation in the air. The grand theaters stand silent, their marquees promising future excitement. It's the calm before the storm of tourists and theater-goers."
          "The morning light reveals the bare bones of the entertainment machine. Without the crowds and the neon glare, the scale of the architecture is even more imposing." ]
    | Midday
    | Afternoon ->
        [ "The area begins to swell with tourists, their faces upturned to gaze at the towering billboards, which have now begun to flicker to life. The energy level starts to rise, a low hum that will soon become a roar."
          "Costumed characters and street performers begin to appear, adding to the surreal, carnival-like atmosphere. The sidewalks become a river of people from all corners of the globe."
          "The buzz is undeniable. People queue for matinee shows, and the restaurants start to fill up. It's the beginning of the daily performance, and the district is the main stage." ]
    | Evening ->
        [ "This is the moment the district was made for. A sensory overload of light, sound, and motion. The neon signs and billboards paint the night sky in a riot of color, and the energy is electric."
          "The streets are packed shoulder-to-shoulder with a massive crowd. The roar of the city is at its peak here, a symphony of traffic, music, and the collective chatter of thousands."
          "Theater-goers, dressed in their evening best, rush to their shows. The atmosphere is one of pure excitement and spectacle, a dazzling display of urban energy." ]
    | Night
    | Midnight ->
        [ "The crowds begin to thin as the final curtains fall, but the district's pulse remains strong. Late-night food carts do a roaring trade, and the lights still blaze with relentless energy."
          "The energy shifts from frantic to a more sustained buzz. People linger, soaking in the unique atmosphere, not yet ready for the show to be over."
          "Even in the late hours, this place feels like the center of the world. The city that never sleeps is most awake here, its heart beating with a neon-fueled rhythm." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The morning sun casts a warm glow on the elegant facades of brownstones and historic mansions. The streets are peaceful, with only the sound of birds and the occasional passerby."
          "There's a palpable sense of history in the air, a feeling of stepping back in time. The well-preserved architecture tells stories of a bygone era of grace and elegance."
          "The neighborhood is waking up slowly and gracefully. It's a time for quiet reflection, for appreciating the timeless beauty and tranquility of the area." ]
    | Midday
    | Afternoon ->
        [ "The streets are perfect for a leisurely stroll. The architectural details of the buildings are fascinating, and the leafy, tree-lined sidewalks provide a shady respite from the sun."
          "The neighborhood retains its peaceful, residential character even in the middle of the day. The pace is slower here, a welcome contrast to the frenzy of other parts of the city."
          "Historic plaques and landmarks offer glimpses into the area's rich past. It's a living museum, a place where history is not just remembered but is an integral part of the present." ]
    | Evening ->
        [ "Gaslight-style street lamps flicker on, casting a warm, romantic glow on the cobblestone streets. The neighborhood takes on an even more charming and intimate character."
          "The windows of the brownstones light up, offering cozy glimpses of life inside. The atmosphere is one of quiet domesticity and understated elegance."
          "The evening is peaceful and serene. It's the perfect time for a quiet walk, to admire the architecture and soak in the historic ambiance under the soft light of the street lamps." ]
    | Night
    | Midnight ->
        [ "The streets are quiet and still. The historic buildings are imbued with a sense of mystery and dignity in the darkness, their stories held within their silent walls."
          "The neighborhood is a peaceful sanctuary from the city's relentless energy. The silence is profound, broken only by the rustle of leaves in the wind."
          "Under the night sky, the historic character of the neighborhood is even more pronounced. It's a place of timeless beauty, a quiet corner of the city where the past feels very much alive." ]

and private nonExistent _ =
    [ "This area does not exist in New York." ]
