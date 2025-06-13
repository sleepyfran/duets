[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Madrid.Streets

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
        [ "The narrow streets of Malasaña are stirring. The scent of strong coffee and toasted bread wafts from small cafes where the first patrons are opening laptops. Street art, vibrant and fresh, covers shop shutters that are just beginning to roll up, revealing vintage clothing stores and independent bookshops. A delivery person on a bicycle expertly navigates the cobblestones, a quiet harbinger of the day's gentle buzz."
          "Dawn in Lavapiés casts a soft golden light on the colorful facades and wrought-iron balconies. Chalk art from the previous evening's gathering still decorates the pavement. The air is a mix of last night's incense and this morning's freshly baked pastries. A few artists gather on a small plaza, sketchbooks in hand, quietly observing the city as it wakes."
          "The first rays of sun glint off the glass of a vermouth bottle on an outdoor table and the metalwork of old balconies. A musician quietly tunes a guitar by a doorway, their soft melody the only sound besides the cooing of pigeons. The street feels like a shared secret, a canvas ready for the day's stories."
          "A gentle breeze rustles the leaves of the tenacious plants spilling from window boxes. The only sounds are the clatter of a cafe owner setting out mismatched chairs and the distant, rhythmic sweeping of a street cleaner. The whole neighborhood feels like it's holding its breath, savoring the peace before the creative chaos begins." ]
    | Midday
    | Afternoon ->
        [ "The bohemian street pulses with a relaxed but constant energy. The sun is high, and the terraces of bars are packed with people enjoying cañas and tapas. Groups of friends debate loudly and laugh, their voices echoing in the narrow lane. The air is filled with the scent of sizzling garlic from a nearby kitchen and the murmur of a dozen different conversations."
          "Sunlight illuminates the organized clutter of second-hand book stalls and artisan craft markets. People browse through vinyl records and handmade jewelry. A busker’s accordion provides a cheerful, slightly melancholic soundtrack to the afternoon. Every wall is a collage of posters for concerts, plays, and political rallies."
          "A painter sets up an easel in the shade of a plane tree, trying to capture the vibrant movement of the crowd. Friends share a 'litrona' (large beer bottle) on a stone bench, while a nearby workshop buzzes with the sound of a sewing machine. The sense of community is palpable, a living, breathing thing."
          "The afternoon heat draws out the scent of jasmine from hidden patios. The street shimmers with a creative haze, a place where students, artists, and old-timers coexist in a lively, colorful dance. Even the stray cats seem to move with an artistic nonchalance." ]
    | Evening ->
        [ "As the sun sets, the street transforms. Fairy lights strung between buildings flicker to life, casting a warm glow on the cobblestones. The smell of exotic spices from Senegalese and Indian restaurants mixes with the aroma of grilled pinchos morunos. Musicians claim their corners, their melodies weaving through the laughter spilling from crowded bars."
          "Small, independent theaters and micro-cinemas open their doors. Crowds gather outside, discussing the upcoming show. Every doorway seems to promise a new experience—a poetry slam, a live jazz set, an art exhibition. Couples stroll hand-in-hand, pausing to listen to music or share a cone of roasted chestnuts."
          "The clinking of wine glasses and the rich aroma of vermouth fill the air. A poet performs under a streetlamp to a small, captivated audience. The street feels timeless, a place where generations of rebels and dreamers have gathered to celebrate art and life."
          "As the night deepens, the energy becomes more intimate. Groups huddle in cozy, dimly lit bars, sharing stories. The last of the musicians pack up, their final notes hanging in the cool air as the neighborhood settles into a creative, contended hush." ]
    | Night
    | Midnight ->
        [ "Under the amber glow of old streetlamps, the bohemian street is a quiet maze for night owls. The murals take on a mysterious quality, their figures seeming to shift in the shadows. A few late-night bars remain open, their windows fogged with the warmth of conversation and laughter. The scent of spilled beer and old wood drifts from a doorway."
          "The street is mostly empty, save for a couple sharing a final cigarette on a doorstep and a food delivery cyclist gliding silently by. The air is cool and still, broken only by the distant thump of music from a club several streets away. The sense of creative potential still lingers, as if the street itself is dreaming."
          "A gentle breeze carries the murmur of a rooftop conversation, and the ivy-covered walls seem to whisper secrets in the dark. The street feels enchanted, a place where a new idea or a forgotten memory could appear at any moment."
          "As midnight passes, the last lights in the apartments above flicker out. The street surrenders to a deep silence, watched over by its silent, painted walls, witnesses to the day's vibrant, chaotic, beautiful life." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The glass titans of the AZCA complex catch the morning sun, casting long, sharp shadows across the wide avenues. A river of professionals in sharp suits flows from the Nuevos Ministerios station, each person armed with a briefcase and a takeaway coffee cup. The air hums with the determined energy of a city getting to work."
          "The roar of traffic on the Paseo de la Castellana is the district's pulse. The rhythmic clatter of heels on pavement and the ding of elevator bells creates a symphony of commerce. The smell is of clean asphalt, expensive cologne, and exhaust fumes."
          "Sunlight glints off the iconic Torre Picasso, its white facade a beacon in the morning sky. Food carts on street corners do a brisk trade in coffee and bocadillos for commuters in a hurry. The atmosphere is one of ambition and urgency."
          "Corporate flags snap in the breeze atop towering office buildings. Doormen in crisp uniforms nod silently as employees stream into gleaming lobbies. The district feels like a well-oiled machine, just beginning its powerful daily cycle." ]
    | Midday
    | Afternoon ->
        [ "At one o'clock, the district exhales. The lunch rush floods the sleek restaurants and cafes at the base of the skyscrapers. Terraces are filled with animated conversations about markets and meetings, the sound of Spanish mixing with English and French."
          "The sidewalks are a blur of navy blue suits and stylish business attire. The constant ringing of phones and the murmur of conference calls drifts from open ground-floor windows. The district is at the peak of its power, a hive of transactions and decisions."
          "Taxis and ride-share vehicles form a constant, weaving ballet, dropping off and picking up clients. The distant sound of construction from a new tower being erected is a reminder of the district's perpetual growth and reinvention."
          "Sunlight reflects intensely off the glass facades, making the plazas feel like urban ovens. People seek shade under the sparse, modern trees, checking their phones and finalizing afternoon schedules. The energy is focused, sharp, and relentless." ]
    | Evening ->
        [ "As dusk settles, the tide of workers recedes, leaving the wide streets feeling spacious and calm. The energy shifts from work to leisure as groups of colleagues gather for after-work cañas at chic, modern bars with minimalist decor."
          "Streetlights flicker on, illuminating mostly empty plazas. The glass towers begin to glow from within, their lighted windows creating a mosaic against the darkening sky. The roar of traffic on the Castellana softens to a steady hum."
          "A golden light washes over the KIO Towers, the famous 'Gate of Europe', as the sun sets behind them. The district grows quiet, its purpose for the day fulfilled, now a landscape of silent, imposing geometry."
          "The sound of laughter and clinking glasses drifts from a rooftop bar atop a luxury hotel, an exclusive pocket of life high above the emptying streets. It's a place to celebrate a closed deal and watch the city lights spread out below." ]
    | Night
    | Midnight ->
        [ "The business district becomes a ghost town of glass and steel. The vast, empty streets are lit only by the cold, white glare of security lights and the branding on the towers. The silence is profound, broken only by the hum of ventilation systems."
          "A lone security guard makes their rounds in a brightly lit, marble lobby. The only movement is the occasional late-night taxi speeding down the Castellana or a cleaning crew's van parked on the curb. The area feels sterile and imposing."
          "The reflections of the lit-up towers shimmer on the dark, silent surfaces of ornamental pools. The district feels like a futuristic cityscape from a film, devoid of human life but humming with latent electronic power."
          "By midnight, the district is asleep. The towering structures stand as silent monuments to the day's commerce, waiting for the sun to rise and the river of people to return and bring them back to life." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The morning sun illuminates vibrant murals on the brick walls of converted warehouses near Matadero. The air smells of fresh coffee and spray paint. Designers and artists arrive on single-speed bikes, disappearing into studios with large, industrial windows."
          "The day begins not with a rush, but with a creative hum. The faint sound of a band doing a soundcheck leaks from a basement rehearsal space, its bassline a gentle heartbeat for the street. There's a palpable sense of potential in the quiet air."
          "A few early risers sip coffee outside a minimalist cafe, MacBooks already open. A gallery owner carefully adjusts the lighting on a new installation, visible through the floor-to-ceiling glass front. The atmosphere is one of focused, quiet preparation."
          "Large, unmarked doors open to reveal cavernous workshops where set designers and sculptors are starting their day. The street is a mix of old industrial grit and new creative polish, a testament to Madrid's evolving identity." ]
    | Midday
    | Afternoon ->
        [ "The street pulses with collaborative energy. The doors of workshops and studios are thrown open to let in the air and light, revealing glimpses of photographers at work and dancers in rehearsal. The sound is a medley of power tools, modern music, and impassioned discussion."
          "Groups of artists and tech startup employees gather at food trucks for lunch, their conversations a fast-paced exchange of ideas and industry jargon. The sidewalks are busy with people carrying portfolios, camera equipment, and rolls of fabric."
          "A fashion shoot is taking place against the backdrop of a graffiti-covered wall, the photographer directing a model while curious onlookers watch from a distance. The street itself has become a flexible, dynamic stage."
          "The air is thick with the smell of sawdust from a furniture maker's shop and the sharp scent of turpentine from a painter's studio. The energy is less about commerce and more about creation; the district feels alive with the process of making." ]
    | Evening ->
        [ "As evening arrives, the focus shifts from creation to presentation. A gallery opening spills out onto the sidewalk, the crowd a stylish mix of artists and patrons, glasses of wine in hand. The sound of clinking glasses and intellectual debate fills the air."
          "The glow of neon signs from independent theaters and performance spaces illuminates the street. People line up for experimental plays or film screenings, their faces lit with anticipation. The creative community gathers to share and celebrate its work."
          "Music and conversation drift from the open doors of trendy bars that were quiet design offices just hours before. The energy remains high, fueled by a shared passion for the new and the next."
          "A digital art installation projects shifting patterns onto the side of a building, captivating passersby. The street is a living exhibition, a place where art isn't just in the galleries, but all around." ]
    | Night
    | Midnight ->
        [ "Most studios are now dark, but a few windows still glow with the light of a desk lamp, signs of a creator working late to meet a deadline or chase an idea. The street is quiet, the creative hum subsided to a low murmur."
          "The last groups of friends leave a late-night concert, their laughter echoing in the industrial silence. The vibrant murals are now just muted shapes in the darkness, their energy resting for the morning."
          "The creative spirit lingers in the air. The silence is punctuated by the distant, rhythmic beat from an underground electronic music club, a reminder that some forms of creation only thrive after dark."
          "A lone musician sits on a loading dock, quietly strumming a guitar, the soft melody a private soundtrack for the sleeping street. The district feels like a mind at rest, full of dreams and ideas waiting for the dawn." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The majestic facades of the Prado and Thyssen-Bornemisza museums bask in the soft morning light. The wide Paseo del Prado is serene, with only the sound of street sweepers and the rustle of leaves in the grand trees. The air smells clean and green from the nearby Botanical Garden."
          "Early-bird art lovers and students are already waiting, sitting on the stone benches, guidebooks open. The atmosphere is one of quiet, reverent anticipation for the treasures within the museum walls."
          "Tour guides gather their small groups near the Neptune Fountain, their flags furled, ready to begin a day of historical and artistic exploration. The promise of discovery hangs in the air."
          "The vertical garden of the CaixaForum is dewy and vibrant in the morning light, a living masterpiece contrasting with the classical stone architecture around it. The scent of coffee drifts from the elegant museum cafes as they prepare to open." ]
    | Midday
    | Afternoon ->
        [ "The Paseo del Arte is a bustling river of people. Tourists from every corner of the world mingle with locals, moving between the 'big three' museums. The air is a polyglot buzz of different languages and excited chatter."
          "The sidewalks are crowded, and long queues snake from the entrances of the Prado and Reina Sofía. Street musicians playing classical guitar or violin add a live soundtrack to the cultural pilgrimage. The shade of the leafy boulevard is a welcome refuge from the strong Spanish sun."
          "Cafes with outdoor seating are filled with visitors taking a break, their tables cluttered with museum maps, tickets, and glasses of cold beer. The conversation is of Goya, Picasso, and Velázquez."
          "School groups in matching t-shirts sit on the museum steps, listening intently to their teachers. The atmosphere is one of education and inspiration, a place where centuries of human creativity are on full display." ]
    | Evening ->
        [ "The golden hour light bathes the grand buildings in a warm, ethereal glow, making the stone facades of the Palacio de Cibeles and the Bank of Spain look even more magnificent. The crowds begin to thin as the museums announce their closing times."
          "The energy shifts from tourism to local leisure. Well-dressed Madrileños head towards the Círculo de Bellas Artes for an evening exhibition or a drink on its famous rooftop bar. The atmosphere becomes more sophisticated and relaxed."
          "The applause from an early evening concert at the Auditorio Nacional can be faintly heard. The facades of the historic buildings are artfully illuminated, and the air is filled with a sense of refined cultural enjoyment."
          "The scent of tapas and wine begins to drift from the stylish bars in the nearby Barrio de las Letras, drawing in those who have spent the day nourishing their minds and are now ready to nourish their bodies." ]
    | Night
    | Midnight ->
        [ "The grand cultural boulevard is quiet and majestic under the night sky. The imposing museum buildings are dark and silent, their artistic treasures locked away. The ornate streetlights cast a warm, romantic glow on the empty sidewalks and grand fountains."
          "The Neptune and Cibeles fountains are beautifully illuminated, their cascading water shimmering in the light. The street feels like an open-air museum of architecture, peaceful and profound."
          "The echoes of the day's crowds have vanished, replaced by the gentle hum of the city. A sense of history and monumental culture remains, heavy and comforting in the stillness."
          "A lone taxi glides down the wide, empty Paseo. The district is at rest, a sleeping giant of art and history, its powerful presence felt even in the deep silence of the night." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Madrid's Gran Vía is in recovery mode. Street cleaning crews jet-wash the sidewalks, erasing the evidence of last night's revelry. The air smells of soap and damp pavement, a stark contrast to the nightly perfume of fun and festivities."
          "The grand theaters are silent, their vibrant neon signs switched off. A few early commuters hurry past, heads down, while delivery trucks unload goods for the massive flagship stores. The street feels like a stage after the show has ended."
          "The metallic shutters of bars and souvenir shops are still down, many covered in graffiti. The only signs of life are the cafes serving coffee and churros to tourists suffering from jet lag and the last of the night's party-goers heading home."
          "The iconic Schweppes sign looks over a surprisingly tranquil scene. For a few short hours, the city's busiest artery is calm, gathering its strength for the relentless onslaught of another day and night of entertainment." ]
    | Midday
    | Afternoon ->
        [ "The pulse of Gran Vía returns. The street is a dense, flowing river of people—shoppers laden with bags, tourists taking photos of the art deco architecture, and locals using it as a chaotic shortcut. The sound is a constant, high-energy roar of traffic, chatter, and music."
          "Street performers—human statues, cartoon characters, and musicians—claim their pitches, drawing small crowds. The smell of roasted nuts and popcorn from street vendor carts mixes with the exhaust fumes and a hundred different perfumes."
          "The energy builds as people pour out of the metro stations. Queues begin to form at the box offices for the evening's musicals—The Lion King, Aladdin. The anticipation of the night's entertainment is already a tangible force."
          "The sun beats down on the pavement, but the energy is unstoppable. People crowd into the air-conditioned mega-stores like Primark or Zara, or seek refuge in one of the many fast-food chains and bars that line the street." ]
    | Evening ->
        [ "Gran Vía ignites. As dusk falls, the massive neon signs and digital billboards blaze to life, bathing the street in a spectacular, colorful glow. Music pours from the open doors of bars and the lobbies of theaters welcoming their audiences."
          "The street is electric with excitement. Well-dressed couples head to see a show, groups of friends start their night out with tapas and drinks, and the sidewalks become even more crowded. The air buzzes with laughter and lively conversation."
          "Queues outside the theaters are long and animated. The pulse of Madrid's nightlife is strong and unmistakable here. It's a place of spectacle, a sensory overload of light, sound, and movement."
          "The aroma of street food gives way to the smells of international cuisine from the many restaurants. The street is a feast for all senses, the vibrant, beating heart of Madrid's promise of a good time." ]
    | Night
    | Midnight ->
        [ "The party is in full swing. The theaters let out, flooding the street with post-show crowds, who then surge into the surrounding bars and clubs in the Malasaña and Chueca neighborhoods. The sound of clinking glasses, reggaeton, and pop music is everywhere."
          "The street is a blur of taxi headlights, flashing signs, and moving people. Groups of friends laugh and shout, making plans for the next stop on their epic night out. The energy is joyous and unrestrained."
          "Even as some venues begin to wind down after midnight, the flow of people continues. The night is far from over; for many in Madrid, it's just getting started. The street is a testament to the city's legendary stamina for celebration."
          "Laughter echoes down the side streets. A group of friends shares a final slice of pizza on a curb before heading home. Gran Vía never truly sleeps; it just takes short, powerful naps." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "On Calle de Serrano, the morning is a picture of polished perfection. The storefronts of Chanel, Prada, and Louis Vuitton gleam, their displays immaculate even before the doors open. The air is still and smells of expensive floral perfume and clean, wet pavement from the street cleaners."
          "The street is quiet and elegant. The only sounds are the discreet click of expensive heels on the sidewalk and the whisper of electric luxury cars gliding by. Doormen in pristine uniforms stand sentry outside high-end jewelry stores."
          "Wealthy residents walk their perfectly coiffed dogs, stopping for an artisanal coffee at a chic, minimalist cafe. The atmosphere is one of understated, confident wealth."
          "The first wave of dedicated shoppers arrives as the boutiques open their doors precisely on time. There's no rush, no chaos—just the serene, orderly beginning of a day of high-end commerce." ]
    | Midday
    | Afternoon ->
        [ "The Barrio de Salamanca buzzes with a refined, steady energy. Well-dressed shoppers, both locals and tourists, move from one luxury brand to the next. The sidewalks are busy but never feel crowded or chaotic."
          "Elegant cafes and restaurants on Calle de Jorge Juan set up their exclusive terraces. The sound is of polite conversation in Spanish and English, the clink of silverware on fine china, and the occasional pop of a champagne cork."
          "Sunlight reflects off immaculate glass storefronts and the polished chrome of luxury vehicles double-parked with impunity. The atmosphere is one of sophistication and unapologetic indulgence."
          "Personal shoppers expertly navigate their clients through the stores, while society figures lunch at exclusive, reservation-only restaurants. This is where Madrid's elite come to see and be seen." ]
    | Evening ->
        [ "As evening falls, the street glows with a soft, expensive light. The windows of the boutiques are artfully illuminated, turning them into jewel boxes. The lanterns of high-end restaurants cast a warm, inviting glow onto the pavement."
          "The evening crowd is impeccably dressed, heading to gourmet dinners or private cocktail parties. The sense of luxury is heightened, more about elegant leisure than daytime shopping."
          "The street feels like a runway. The air is filled with the scent of fine dining—truffle, fine wine, and delicate desserts—mixed with the world's most exclusive fragrances."
          "A quiet, confident hum pervades the area. There's no loud music or boisterous laughter, just the murmur of sophisticated enjoyment and the quiet purr of a departing limousine." ]
    | Night
    | Midnight ->
        [ "The glamorous street grows quiet and peaceful. The shops are dark, their precious contents secured behind gates, but their elegant facades maintain the district's opulent character. The only traffic is the occasional taxi carrying diners home."
          "The streetlights cast a soft, flattering glow on the beautiful architecture of the 19th-century apartment buildings. The area feels safe, serene, and deeply exclusive."
          "A sense of peaceful opulence lingers. The silence is broken only by the distant chime of a clock or the soft footsteps of a couple strolling home after a late dinner."
          "The district sleeps under a blanket of quiet wealth. It feels like a protected enclave, resting and readying itself for another day of effortless glamour." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The morning sun casts long shadows across the cobblestones of the Plaza Mayor. The air is cool and still, holding the scent of old stone and the faint, sweet aroma of churros from a nearby cafe preparing to open. The sound of church bells from a distant parish echoes through the narrow alleys."
          "The centuries-old buildings of Madrid de los Austrias glow in the soft light. Their red-tiled roofs and wrought-iron balconies tell stories of Hapsburg Spain. A few locals cross the empty plaza on their way to work, their footsteps the only sound."
          "Delivery vans carefully navigate the winding streets, bringing fresh produce to the historic mesones and taverns. The Mercado de San Miguel is quiet, its glass walls reflecting the awakening city."
          "The smell of freshly baked bread and strong coffee begins to drift from traditional bakeries that have served the neighborhood for generations. The past feels incredibly present, a tangible weight in the morning air." ]
    | Midday
    | Afternoon ->
        [ "The historic heart of Madrid is alive with tourists and locals. The Plaza Mayor buzzes with activity, its outdoor cafes filled with people enjoying the sun. The sound is a mixture of tour guides explaining history, street artists performing, and the general hubbub of a crowd."
          "The narrow streets are a hive of exploration. Visitors wander in and out of shops selling traditional crafts like fans and espadrilles. The scent of sizzling calamari for the famous bocadillos de calamares is irresistible and hangs heavy in the air."
          "Shaded, hidden courtyards offer a quiet respite from the sun and the crowds. The sense of history is everywhere, in the worn stone steps, the ancient wooden doors, and the coats of arms carved above doorways."
          "The sound of a lone classical guitarist often drifts from an archway, their beautiful melodies adding another layer to the historic atmosphere. It feels as if you could turn a corner and step back in time." ]
    | Evening ->
        [ "The 'golden hour' bathes the sandstone of the Royal Palace and the Almudena Cathedral in a spectacular, warm light. The streets are filled with the sound of relaxed conversation and laughter as the day's heat subsides."
          "Locals and visitors alike gather in the old taverns, a tradition known as the tapeo. The sound of clinking glasses of vermouth and the chatter of friends fills the historic bars. The air smells of olive oil, cured ham, and red wine."
          "The past feels tantalizingly close. As the streetlights flicker on, casting long shadows, every corner seems to hold a secret—a legend of a ghost, a story of a king, a whisper of a revolution."
          "The energy is convivial and deeply rooted in tradition. This is not the Madrid of flashing lights, but of shared stories, timeless flavors, and a deep connection to the city's soul." ]
    | Night
    | Midnight ->
        [ "The historic district becomes a mysterious labyrinth of dark, silent streets. The cobblestones gleam under the old-fashioned streetlights, and the only sound is the echo of your own footsteps. The air is cool and carries the scent of night-blooming jasmine from hidden patios."
          "The Plaza Mayor is vast and empty, its arcades filled with deep shadows. The painted frescoes on the Casa de la Panadería are barely visible, like sleeping ghosts. The sense of history is at its most potent now, in the profound silence."
          "The district feels like a city dreaming of its own past. You can almost hear the faint echoes of horse-drawn carriages and royal processions. The air is thick with unspoken stories."
          "A lone cat darts across a silent alleyway. The great squares and narrow lanes are returned to the ghosts and legends that inhabit them, waiting for the sun to bring the living back." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "In an industrial part on the city's edge, the day starts early and loud. The air fills with the roar of diesel engines as delivery trucks and forklifts begin their work. The scent is a sharp mix of metal, oil, and exhaust."
          "Large, corrugated metal gates slide open, revealing vast warehouses and workshops. Workers in high-visibility vests and steel-toed boots gather at entrances, sharing a quick coffee from a thermos before clocking in. The atmosphere is purely functional."
          "The street is a utilitarian landscape of concrete, asphalt, and functional, unadorned buildings. There is no decoration, only the logos of logistics companies and manufacturing plants. The purpose here is clear: to make and to move."
          "The rhythmic clang of machinery and the beeping of reversing trucks form the industrial morning anthem. The air hums with a raw, powerful energy, far from the polished center of Madrid." ]
    | Midday
    | Afternoon ->
        [ "The street is a hive of constant, organized activity. Trucks are loaded and unloaded in a noisy, efficient ballet. The air shimmers with heat rising from the asphalt, and the noise of machinery is relentless."
          "Lunch breaks are short and practical. Workers gather in small, no-frills canteens or at food trucks parked on the periphery, their conversations loud and fast over the industrial din. The food is hearty and simple."
          "The area feels efficient and focused. Everyone moves with purpose, whether driving a forklift, overseeing a production line, or signing off on a delivery. There is little time for leisure."
          "The sun beats down on wide, treeless roads. The sound of welding and grinding adds to the symphony of industry, a sign of the constant production happening behind the plain facades." ]
    | Evening ->
        [ "As the main shifts end, the cacophony subsides. A wave of workers heads out, their cars and motorcycles flooding the access roads for a brief period. The street grows significantly quieter."
          "A few lights remain on in offices and workshops where a night shift is beginning or overtime is being worked. The smell of metal and oil lingers in the cooling air. The sense of industry is ever-present, even in the relative quiet."
          "The occasional articulated lorry rumbles past, its headlights cutting through the dusk as it heads out onto the M-40 motorway for a long-haul journey. The district is winding down, but it never completely stops."
          "The setting sun casts long, stark shadows from cranes and communication towers. The industrial landscape takes on a skeletal, almost sculptural quality in the fading light." ]
    | Night
    | Midnight ->
        [ "The industrial area is almost completely deserted, a stark and lonely place. The streets are lit by the harsh, orange glare of security lights, creating pools of light in a sea of darkness. The silence is vast and deep."
          "The only sounds are the distant hum of a generator, the wind whistling around large metal buildings, and the rumble of a freight train on the nearby tracks. The human element is gone."
          "The street feels utilitarian and anonymous. It's a landscape built for machines, not people. The scale of the warehouses and silos is even more imposing in the darkness."
          "A lone security car makes its slow, steady rounds, its headlights sweeping across silent loading bays. The district is a place of pure function, waiting in standby mode for the morning to bring back its noise and purpose." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The wide, tree-lined avenues of the El Retiro neighborhood are serene and immaculate in the morning sun. The grand, turn-of-the-century apartment buildings with their ornate facades and clean-swept entrances exude a calm, established wealth. The air smells of blooming flowers from private gardens and freshly cut grass."
          "A few early joggers in expensive athletic wear glide along the sidewalks. Doormen in formal attire polish the brass handles of massive wooden doors. The only sound is birdsong from the nearby park and the soft purr of a diplomatic car."
          "The atmosphere is one of profound peace and refinement. This isn't the flashy luxury of a shopping street, but the quiet confidence of old money and residential elegance."
          "Residents enjoy a quiet breakfast on balconies overlooking the street, hidden behind lush green foliage. The day begins slowly, gracefully, and without any sense of urgency." ]
    | Midday
    | Afternoon ->
        [ "The street remains calm even at midday. Luxury cars—Bentleys, Porsches—are parked neatly along the curb. Nannies push expensive strollers towards the gates of El Retiro Park. The sound is of polite greetings and the distant laughter of children playing."
          "The entrances to five-star hotels like the Mandarin Oriental Ritz are discreetly busy, with bellhops assisting well-heeled guests. The clink of fine china can be heard from the exclusive hotel terraces, where afternoon tea is being served."
          "The feeling is one of comfort, privilege, and absolute discretion. The wealth here is not on display; it is simply the air one breathes. The sun filters through the dense canopy of the trees, dappling the wide, clean sidewalks."
          "Children in smart school uniforms are picked up by their parents or drivers. The afternoon is a picture of orderly, privileged domestic life, a world away from the city's central chaos." ]
    | Evening ->
        [ "Golden light from elegant, old-fashioned streetlamps illuminates the facades of the stately buildings. The street becomes a stage for quiet, sophisticated evening activity. Well-dressed residents head out for dinner at exclusive, hidden-gem restaurants or to the opera."
          "Soft piano music drifts from the open window of a ground-floor apartment or the lobby of a luxury hotel. The air is filled with the subtle scent of gourmet cuisine and expensive evening perfumes."
          "The street feels festive yet intensely private. It is a place to be, not to be seen. The atmosphere is one of effortless elegance and secure tranquility."
          "Chauffeurs wait patiently by their gleaming cars. The sound of happy, but subdued, conversation comes from the private clubs and embassy receptions that are common in the area." ]
    | Night
    | Midnight ->
        [ "The grand avenue is profoundly peaceful and quiet. The only movement is the occasional taxi gliding silently to a stop or a doorman wishing a resident a good night. The majority of the ornate windows are dark."
          "The streetlights cast a gentle, romantic glow on the marble entryways and stone carvings of the buildings. The sense of luxury and security endures, even in the deep of night. It feels like the safest, most serene place in the city."
          "A feeling of exclusivity and calm hangs in the air. The silence is deep, cushioned by the wealth and history of the neighborhood. It is a restful, peaceful end to the day."
          "The distant, muffled sound of a classical music station from an open window is the only thing that breaks the silence. The street sleeps soundly, wrapped in a blanket of quiet opulence." ]

and private nature dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Madrid's El Retiro Park awakens to a chorus of birdsong. The rising sun sends golden shafts of light through the misty air, dappling the winding paths. The air is cool and smells richly of damp earth, dew-covered grass, and pine needles."
          "The park is the domain of early-risers. Joggers follow their usual routes, their rhythmic footsteps a soft percussion. Dog-walkers gather in designated areas, their pets joyfully chasing balls on the wide lawns. The only traffic is the gentle whir of the park's maintenance vehicles."
          "The water of the great lake (Estanque Grande) is a perfect mirror, reflecting the grand monument to Alfonso XII. A few dedicated rowers are already out, their oars dipping silently into the glassy surface. The atmosphere is one of pure tranquility."
          "Gardeners are at work in the Rosaleda, the rose garden, their gentle snipping and tending adding to the peaceful, diligent start of the day. The park feels like the city's green, beating lung, inhaling the morning freshness." ]
    | Midday
    | Afternoon ->
        [ "The park is alive and bustling with a happy, relaxed energy. Families spread out picnic blankets on the grass, their laughter echoing under the trees. The sound of children playing fills the air, mixing with the music from a nearby busker."
          "The main paths are busy with strollers, cyclists, and couples walking hand-in-hand. The lake is now filled with blue and white rowboats, clumsily navigated by laughing tourists and locals. The sun is warm, and the shade of the horse-chestnut trees is a precious commodity."
          "Light streams through the glass walls of the Palacio de Cristal, illuminating the art installation inside. Outside, people lounge on the steps, feeding the ducks and turtles in the small lake. The vibe is leisurely and joyful."
          "The scent of popcorn and waffles drifts from small kiosks near the main entrances. The park is a vibrant, democratic space, a green oasis offering a joyful escape from the urban intensity just beyond its gates." ]
    | Evening ->
        [ "The golden hour bathes the park in a magical, warm light. The long shadows of the trees stretch across the lawns. The frantic energy of the afternoon mellows into a tranquil, romantic hum. Couples find secluded benches, and groups of friends practice yoga or tai chi on the grass."
          "The birds begin their evening chorus as the sun sets, a beautiful, natural symphony. The air cools, and the scent of night-blooming jasmine begins to emerge from the Cecilio Rodríguez Gardens. The pace slows, and a sense of peace descends."
          "The last of the rowboats are returned, and the lake's surface grows still again. The streetlights along the paths flicker on, creating a soft, inviting glow. The park becomes a perfect place for a quiet stroll and reflection."
          "The distant sound of city traffic serves as a reminder of the world outside, but within the park's confines, the atmosphere is serene and restorative, a final, deep breath at the end of the day." ]
    | Night
    | Midnight ->
        [ "The gates of El Retiro are closed, and the vast park is returned to darkness and silence. The only light comes from the moon and the ambient glow of the city beyond its borders. The paths are shadowy and mysterious."
          "The only sounds are the rustling of leaves in the wind, the hoot of an owl, and the gentle lapping of water against the edge of the lake. The park feels wild and ancient, its daytime identity shed."
          "The grand statues and fountains are dark silhouettes against the night sky. The Palacio de Cristal is a ghostly, skeletal structure in the moonlight. The sense of nature is profound and a little intimidating."
          "The park is a secret world at night, inhabited only by its nocturnal creatures and the silent, watchful trees. It is the city's sleeping heart, gathering its energy to offer peace and beauty once more with the coming of the dawn." ]
