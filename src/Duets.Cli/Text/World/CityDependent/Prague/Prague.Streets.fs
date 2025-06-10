[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Prague.Streets

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
    | Historic -> historic
    | Glitz -> glitz
    | Industrial -> industrial
    | Luxurious -> luxurious
    | Nature -> nature
    |> fun fn -> fn dayMoment

and private bohemian dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The narrow cobbled street is still damp from the morning dew. Ornate but crumbling Art Nouveau facades line the way, their plaster peeling to reveal the brick beneath. A few early patrons sit outside a quiet 'hospoda' (pub), sipping their first beer of the day. The air is thick with the smell of brewing coffee and yesterday's spilt lager."
          "The street is slowly stirring. Mismatched signs for vintage shops and smoky pubs hang over doorways. The scent of fresh bread wafts from a small 'pekárna' (bakery). A lone artist sketches the crooked rooftops from a corner, capturing the quiet, dishevelled charm of the neighbourhood."
          "Small, quirky galleries and basement bars with heavy wooden doors line the street. The sidewalks are largely empty, save a few people heading to the tram stop. Potted plants on windowsills and graffiti art on a forgotten wall give the street a colourful, lived-in feel." ]
    | Midday
    | Afternoon ->
        [ "The cobbled street hums with a relaxed energy. The patios outside the pubs are now full, with chatter and laughter mixing with the clinking of glasses. Quirky boutiques and second-hand bookshops see a steady stream of browsers. Old, ivy-covered walls seem to soak up the midday sun, lending a timeless quality to the scene."
          "A vibrant mix of students, artists, and old-timers populates the street. People mill between pubs and small, independent shops, their conversations echoing in the narrow space. The street buzzes with an easygoing, unpretentious energy."
          "The street feels like a collection of hidden corners and tucked-away courtyards, often leading to a small beer garden. The hum of conversation, snippets of music from an open window, and the general activity of passers-by fills the air with a pleasant chaos." ]
    | Evening ->
        [ "The cobbled street is now bathed in the warm glow of streetlamps and pub signs. Laughter and loud conversations spill from the open doors of packed 'hospody'. A band can be heard tuning up in a basement club, its muffled sound adding to the anticipation of the night. The old facades seem to recede into the shadows, making the lit doorways all the more inviting."
          "Dim lights filter through the smoky windows of intimate pubs and candlelit restaurants. Artists and locals mingle on the pavement outside, drinks in hand. The street is alive, drawn together by the alluring, convivial atmosphere."
          "The street has come alive. The windows of the pubs glow with warmth and activity. Live music now fills the air, and small groups cluster on street corners, deciding which smoky bar to dive into next. The air is electric with a friendly buzz." ]
    | Night
    | Midnight ->
        [ "The cobbled street is quieter now, lit by the sparse glow of old-fashioned streetlamps. A few pubs remain open, spilling dim light and the sound of quiet conversation onto the pavement. The ornate facades are lost in darkness, and the street feels ancient and intimate. A few last drinkers make their way home, their footsteps echoing on the stones."
          "The street is dimly lit, casting long shadows that stretch across the closed boutiques. The soft glow of warm lights seeps from the few establishments that remain open, often with a small huddle of smokers standing outside the door."
          "Most businesses are shuttered, their metal grilles pulled down for the night. The street has an almost eerie beauty in the quiet. Streetlights cast strange shadows on the cobblestones. Very few linger here as the night goes on and it grows very still." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Sleek glass-fronted office buildings stand beside renovated 19th-century industrial halls. Trams rattle past, their bells clanging in the crisp morning air. The sidewalks are crowded with people in smart business attire, hurrying towards their offices with briefcases and takeaway coffee cups."
          "The street is a canyon of modern steel and historic brickwork, the morning sun reflecting brightly off the glass facades. Pedestrians stride purposefully along the wide pavements, while food kiosks do a brisk trade in pastries and coffee. The air smells of tram brakes and fresh espresso."
          "Imposing modern structures dominate the area, their reflective glass fronts contrasting with the occasional grand, older building. The steady sound of traffic is punctuated by the rhythmic clang of passing trams. Groups of office workers are beginning their daily commutes." ]
    | Midday
    | Afternoon ->
        [ "Modern glass buildings cast sharp shadows on the street, punctuated by the warm brick of converted factories. The sound of trams is a constant backdrop. The sidewalks are busy with people heading to lunch at trendy bistros or grabbing a sandwich to eat by the nearby river."
          "The midday sun glints off the chrome and glass towers. Lunch crowds spill out from offices, filling the modern plazas and food courts. The sounds of chatter in a dozen languages echo around the sleek, corporate architecture."
          "Large, towering structures of glass and steel rise up on both sides of the street. People can be seen bustling between meetings, occasionally lingering at small cafes with outdoor seating. The overall noise is a mix of traffic, tram bells, and animated conversation." ]
    | Evening ->
        [ "The glass towers begin to empty, their windows going dark one by one. The sound of traffic and trams lessens as the evening rush hour subsides. The sidewalks are almost deserted, save for the occasional commuter hurrying home or a cleaning crew starting their shift."
          "The lit towers look grand and imposing against the darkening sky. The steady flow of commuters dwindles, and the streets have quietened significantly. The focus shifts to the stylish wine bars and restaurants that are just beginning to welcome their evening clientele."
          "The street lamps flicker on, reflecting in the puddles left by the street cleaning vehicle. The street feels large and deserted. The tram noises become more infrequent, and only a few people remain, heading for a late dinner or a taxi home." ]
    | Night
    | Midnight ->
        [ "The modern towers stand as dark silhouettes against the city glow, punctuated by the odd light of a late-working office or a maintenance crew. The sound of traffic is minimal. The sidewalks are deserted and lit only by sterile, white streetlights."
          "The skyscrapers cast long, dark shadows across the otherwise deserted street. The office towers are dark, save for a few red aircraft warning lights blinking in unison. A lone security guard can sometimes be seen making their rounds behind the glass."
          "The area feels like a different world compared to the busy working day. There is an eerie silence here, and the scale of the empty buildings under the night sky feels immense and inhuman." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The street is starting to wake with a quiet, expectant energy. Large windows of converted warehouses show off studios cluttered with design prototypes and art supplies, but few people are visible yet. The air smells of industrial history and fresh coffee from a lone hip cafe. A few cyclists glide down the road."
          "The buildings here are a mishmash of repurposed factories and stark modern additions. Large windows offer glimpses into workshops and studios just being opened for the day. The faint, rhythmic sound of a printing press can be heard from an open door."
          "The buildings are varied, their industrial past clearly visible in their high ceilings and large metal-framed windows. The sidewalks are empty save for a couple of people unlocking heavy metal doors, but the promise of a creative buzz is palpable." ]
    | Midday
    | Afternoon ->
        [ "The street pulsates with a creative buzz. Large windows show off busy design agencies and studios where people can be seen collaborating around large tables or shaping materials. Music drifts from a nearby rehearsal space. People meander along the sidewalks, popping into independent galleries or concept stores."
          "The street hums with activity, the energy of creation permeating the area. The windows are now full of busy individuals, from fashion designers draping mannequins to programmers staring intently at screens. Small groups often gather to look at a particular piece of street art."
          "The street feels busy and full of different energies. The shops show off a wide range of local design, and the architecture is a fascinating testament to industrial conversion. The sidewalk can sometimes become clogged with onlookers, and the street is not empty for long." ]
    | Evening ->
        [ "As dusk falls, the studios and offices empty out, but the street's energy transforms rather than fades. The focus shifts to alternative theatres, independent cinemas, and gallery openings. A mix of loud music, chatting, and laughing can be heard from various venues, as a trendy crowd flows along the pavement."
          "The studio windows are now dark, but the ground floors have come to life with bright lights shining from bustling bars and performance spaces. The street hosts a gathering of patrons and creative types drawn to live music and exhibition previews."
          "The street has changed. A new crowd congregates here, drawn by innovative pop-up restaurants and experimental music events. This creates a friendly, stylish throng that lingers for hours, discussing art and ideas over drinks." ]
    | Night
    | Midnight ->
        [ "The street is mostly dark, with little activity coming from the former industrial buildings. Most windows are dark, and it is only possible to vaguely tell what is within from the shadows cast from outside. Very few people are walking in the area, and they are mostly moving quickly through it."
          "The street is quiet, but not deserted, with the occasional flicker of light from an apartment window on an upper floor. A faint bassline thumps from a hidden club. Very few wander down the road, but a constant quiet murmur permeates the place still."
          "The street appears entirely different at night, and most of the creative businesses are shut down. The silence and the sheer scale of the dark buildings can feel lonely and a little intimidating." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Garish neon signs from clubs and casinos stand dormant and out of place in the morning light. Grand historic theatre facades stand shoulder to shoulder with fast-food chains. Street cleaners are hosing down the pavements, washing away the evidence of the previous night's revelry. The streets are mostly deserted."
          "The streets are strangely quiet, and it feels odd in the morning light before the tourist crowds descend. A handful of workers clear out rubbish bins and restock bars. The atmosphere seems quiet and uncharacteristic for an area known for its noise and crowds."
          "The streets are bare, showing off the grand architecture and the tacky storefronts that make this a centre of activity at night. Few people walk through the streets, and the bright signs stand inactive and dull." ]
    | Midday
    | Afternoon ->
        [ "Some of the brighter neon signs have begun to flash, looking garish in the daylight. Grand theatre facades and casual restaurants create a lively, chaotic mix. There is a noticeable increase in activity now, as tourists stroll and plan their evening. A constant mix of chatter, music from souvenir shops, and the spruiking of tour guides fills the air."
          "The street fills slowly as people trickle in from other areas. Small crowds gather to watch street performers or to queue for currency exchange booths. The mood is beginning to feel more lively as people start their afternoon drinking in pavement cafes."
          "The mood is becoming bright and busy as people start to fill the streets. They gather at the shops here and plan their day, navigating through the crowds with maps and phones in hand." ]
    | Evening ->
        [ "Bright neon signs and illuminated billboards vie for your attention, bathing the street in a vibrant, tacky glow. Grand theatre facades stand next to bustling beer halls and noisy clubs. The air buzzes with energy, a constant wall of sound from music, hawkers, and the chatter of a thousand tourists. Crowds fill the street, a river of humanity."
          "The crowds here fill the street from all directions, coming to attend theatres, restaurants, and clubs. The area is awash with noise, light, and colour, a true sensory overload."
          "The streets are extremely crowded, a chaotic mix of tour groups, stag parties, and couples. The bright signs and storefronts call to you, and the area throbs with a vibrant, commercial energy." ]
    | Night
    | Midnight ->
        [ "The neon signs still flash, though some venues have closed for the night. The grand theatres are dark, but the late-night bars and clubs are still going strong. There is less activity now as most shows are over, but a persistent, rowdier crowd remains, moving between venues."
          "The neon signs of closed casinos and cabarets flicker off one by one. The streets feel much more spacious than before. Groups of people are now trying to find taxis or late-night food."
          "The lights seem to have faded, and the main crowds have dissipated. Most venues have either closed or greatly reduced their activity. There is still a bustle of people walking along the sidewalk to get transportation home or seek a quieter after-hours establishment." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The narrow, cobbled lane is damp and gleaming from an overnight cleaning. The worn stone facades of ancient buildings seem to lean in, whispering secrets of centuries past. A baker unlocks his shop, releasing the warm scent of fresh bread into the cool air. The only sounds are the cooing of pigeons and a distant church bell."
          "Morning light struggles to penetrate the alley, illuminating a sliver of the Astronomical Clock's tower at the far end. Delivery workers rattle carts over the uneven stones, their modern task a stark contrast to the medieval surroundings. The air is still and holds a hint of morning mist from the river."
          "The lane is a silent passage through history. House signs—a golden lion, two suns—are visible above arched doorways. The stones underfoot are polished smooth by millions of footsteps. For a moment, with no one else around, it's easy to imagine the city as it was 500 years ago." ]
    | Midday
    | Afternoon ->
        [ "The lane is now thronged with people, their voices and footsteps echoing between the stone walls. Sunlight, where it reaches the ground, creates a sharp patchwork of light and deep shadow. Tour guides hold up umbrellas, their groups pressing forward to see the next historic sight."
          "A cacophony of languages fills the air as tourists from around the world navigate the winding passage. People peer into small shops selling Bohemian crystal, wooden puppets, and amber jewellery, their windows like tiny treasure chests."
          "The sheer press of the crowd makes movement slow. The ancient stones seem to absorb the noise and energy. Above the heads of the masses, the silent, impassive faces of stone saints and gargoyles look down from the rooftops." ]
    | Evening ->
        [ "Gas-style lamps flicker to life, casting a warm, golden glow on the cobblestones and creating long, dancing shadows. The crowds have thinned, replaced by a more romantic atmosphere. The sound of a classical guitarist drifts from a nearby square, his melancholic tune perfectly suiting the twilight hour."
          "The lane feels like a passage back in time. The fading daylight turns the sky above into a deep blue velvet, against which the gothic spires are sharply silhouetted. The air cools, and the smell of goulash and roasting meat wafts from a restaurant in a deep stone cellar."
          "The golden light from the lamps makes the street feel magical and intimate. Couples stroll hand-in-hand, their voices low. Every dark, arched doorway seems to hold a mystery, inviting you to wonder what lies within." ]
    | Night
    | Midnight ->
        [ "The lane is empty and silent, steeped in shadow and history. The moonlight catches the edge of a grotesque gargoyle high on a church roof. Your footsteps are the only sound, echoing the passage of centuries on the worn cobblestones."
          "The air is cold and still. The tall, dark buildings loom over you, their windows like vacant eyes. The lane feels haunted by the ghosts of all who have walked here before—alchemists, kings, and commoners. A sense of profound history and loneliness pervades the space."
          "Deep in the labyrinth of the Old Town, the silence is absolute. The street is a dark canyon, lit only by the faint glow of the distant city sky. It is a place for secrets and solitude, where the modern world feels a million miles away." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Immaculate Art Nouveau facades gleam in the soft morning light. The wide, tree-lined avenue is pristine. High-end boutiques are displayed like museums through gigantic, polished display windows. A few expensive cars glide silently by, but otherwise, the area is surprisingly quiet."
          "The street feels very pristine in the early light, its carefully preserved historic surfaces immaculate. Staff in sharp uniforms can be seen meticulously arranging displays in the windows of Cartier and Dior before the doors open."
          "The early sun glints from the gilded details on the ornate buildings, revealing a clean and perfect street. It's completely empty save for a few delivery personnel making discreet drop-offs at the closed doors of the luxury boutiques." ]
    | Midday
    | Afternoon ->
        [ "Immaculately preserved facades gleam under the sun's light, making the architecture seem even more grand. High-end boutiques showcase their wares behind vast windows. Luxurious cars are a common sight, dropping off and picking up affluent shoppers. Some onlookers browse idly, more in awe of the buildings than the bags."
          "The street is active with well-dressed shoppers moving between stores, their expensive shopping bags a stark contrast to the historic setting. All the storefronts have their goods carefully displayed in large, modern window settings. People gaze up admiringly at the stunning Art Nouveau architecture."
          "Luxury shoppers pass by in expensive clothing, while others admire storefronts and occasionally pause to point out a particular watch or dress to their companions. Cars with diplomatic plates pull up to the curb. The scene here appears both refined and bustling." ]
    | Evening ->
        [ "The street is bathed in elegant, warm light from shop windows and ornate streetlamps. The Art Nouveau architecture looks particularly magical at night. Luxurious cars line up along the curb outside fine-dining restaurants. Small crowds of beautiful people congregate, an air of exclusivity hanging in the air."
          "As dusk settles, the bright shop windows shine like jewel boxes. Small groups loiter on the pavement, clearly there to see and be seen. There's a feeling of opulent calm as people head for late evening cocktails and exclusive social events."
          "As the sun goes down, many well-to-do visitors linger on the street, dressed in expensive evening wear. The area is well lit and everything seems very clean and perfect, creating a bubble of extreme wealth and beauty." ]
    | Night
    | Midnight ->
        [ "The ornate facades are softly illuminated, looking like a film set. High-end boutiques are displayed like museums through their darkened but still lit display windows. Only the occasional luxury vehicle makes its way down the silent street. Very few people are visible here."
          "The stores are closed now, but their displays glow faintly behind security grilles, looking almost like guarded treasures. Only the odd limousine passes by; otherwise, the area has shut down for the night, serene and untouchable."
          "The area now feels deserted. The streets are empty and silent, and everything glows with a sterile, expensive light. The only sign of life is a private security guard patrolling the block." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The buildings here are uniformly grand, with well-tended private gardens and ornate wrought-iron balconies. Expensive cars are parked neatly along the wide, tree-lined street. It's peaceful and quiet here, as the residents of these exclusive apartments are only now beginning their day."
          "Grand Art Nouveau apartment buildings dominate this street, almost as if they were all part of a single, elegant design. It feels calm and tranquil, but very secluded. The only person in sight is a landscaper tending to a perfect hedge."
          "The properties here seem large and impressive, and the plants in the front gardens are so well maintained they could be from a magazine. Few seem to occupy this area beyond gardeners and housekeepers, and the day has an eerily peaceful quietness." ]
    | Midday
    | Afternoon ->
        [ "The buildings here are of a uniformly high quality, with manicured lawns and subtle high-end touches. Cars with diplomatic plates crawl past the large townhouses. It's peaceful and quiet, with very few people visible beyond the odd nanny pushing a pram or a dog walker."
          "The architecture along this road is all very high quality, with every building looking freshly painted and perfectly maintained. Everything has a manicured look to it, with little out of place. The area feels serene and almost unnaturally tidy."
          "The area seems almost unpopulated beyond those performing work to keep the buildings so clean and precise. A light breeze rustles the leaves of the tall, mature trees, the only sound in the quiet air." ]
    | Evening ->
        [ "The buildings here are uniformly high quality, with well-tended gardens and elegant facades. Cars with expensive tags pull up to the grand entrances. There is a small increase in the visible number of people compared to earlier, most of them seeming to arrive from or return to their home."
          "The lighting here is subtle and soft as evening comes on. Warm light glows from behind the curtains of large apartment windows. There is an increase in activity, but it all seems contained and private, as residents return for the evening."
          "There are signs of life here, but it mostly consists of private chauffeurs waiting in cars or staff in uniform. Very few of the residents of this street seem to walk along it; they simply appear and disappear through the grand doorways." ]
    | Night
    | Midnight ->
        [ "The grand apartment buildings stand in stately silence. Most cars have been safely stored away in underground garages. The area is quiet and completely deserted, as most residents are inside for the night, their windows either dark or dimly lit."
          "The area has a subdued, wealthy look. Only a few lit-up windows indicate that residents are active inside. The roads and the sidewalks are entirely empty. The silence is profound."
          "The lighting here feels soft and gentle in the silence and darkness. Nothing seems out of place or out of sync. There is a sense of secure, cloistered loneliness that looms over the perfect street." ]

and private nature dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The buildings that can be seen from the street look small and out of place. Mostly what you see here is the edge of a vast, wild park, with rocky outcrops and dense trees. The street is narrow, seemingly carved into the edge of the wilderness. The sound of birds chirping is surprisingly loud, and a few hikers can be seen heading for the trails."
          "A dense forest presses right up against the edge of the narrow road. The buildings that manage to exist here feel small and unimportant when placed beside such tall trees. The sounds of the forest come to your attention easily, creating a loud chorus."
          "The road here feels like a thin ribbon laid to permit access to the city from the thick woods. Everything feels very calm and undisturbed, and very little besides birds and the occasional deer can be seen from this peaceful road." ]
    | Midday
    | Afternoon ->
        [ "The buildings seem to shrink away from the dominance of nature. The street is a narrow strip of civilization bordering a vast city park. The sound of birds chirping is clear and distinct, and a handful of people hike or stroll on the paths within the wilds nearby."
          "The air smells of damp earth and pine needles. The shadows of the trees grow long, stretching out over the sidewalk. The forest feels full of movement as various birds and small animals rustle in the undergrowth."
          "The forest here looks thick and impenetrable, mostly only revealing tree trunks and a carpet of leaves. Light shines through the canopy and highlights occasional birds that pass overhead. A tram stops at the edge of the park, disgorging more visitors." ]
    | Evening ->
        [ "The buildings that can be seen from the street are small and their windows are beginning to glow. The forest beside the road is growing dark and ominous. The sound of birds is fading as they begin to roost. The last few dog-walkers are quickly returning to the lights of the street."
          "The light here is fading fast, mostly cut off by the dense leaf cover that dominates this area. Sounds from the wildlife grow more prominent now as owls and small insects begin their nocturnal routines."
          "The forest is growing dark with the dying sun and is starting to feel strangely primeval. There are some last stragglers hiking back from the deep brush, their phone torches bobbing in the gloom." ]
    | Night
    | Midnight ->
        [ "The few buildings here are insignificant against the vast, black shape of the forest. The street is narrow and seems to barely hold back the wilderness. The sound of crickets and rustling leaves dominates all else. No humans can be seen anywhere here now."
          "The road here is very dark, barely lit by the distant city glow that manages to penetrate the canopy above. There are very few human sounds here, save for the occasional car using the road as a shortcut."
          "The area is mostly black, the forest's edge identifiable only as a wall of deeper darkness. The sounds of the nocturnal woods now rule this domain, a world away from the city centre." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The broad avenue is lined with grand theatres and galleries, their neo-classical facades bathed in gentle morning light. A few early visitors stroll between them, clutching coffees. Banners announcing new exhibitions and operas flutter softly from ornate lampposts."
          "Morning light streams through the large arched windows of historic concert halls like the Rudolfinum, illuminating posters of upcoming performances. A street musician begins to set up near the National Theatre, tuning a cello for the day's work."
          "The quiet of dawn is broken by the murmur of museum staff unlocking heavy doors and art lovers arriving with anticipation. The scent of coffee and pastries wafts from a nearby 'kavárna' (cafe) popular with the city's intellectuals." ]
    | Midday
    | Afternoon ->
        [ "Tourists and locals wander between cultural landmarks, pausing to admire the statues in the historic squares. Guided tours flow through museum courtyards, their narrators animatedly sharing stories of Dvořák and Mucha."
          "The energy of the cultural district is palpable as midday crowds gather for exhibitions and matinee performances. Musicians play Vivaldi on street corners, adding a classical soundtrack to the lively atmosphere."
          "Artisans display handmade crafts at pop-up stalls near gallery entrances. The sounds of conversation, camera shutters, and distant classical music blend into a vibrant midday symphony against a backdrop of stunning architecture." ]
    | Evening ->
        [ "Historic theatres light up with marquee signs, inviting patrons to evening shows. Outdoor spaces host impromptu jazz performances as twilight settles over the district, the lights of Prague Castle glowing on the hill above."
          "Galleries stay open late for special 'vernisáž' (exhibition openings) with wine receptions. Crowds cluster beneath ornate archways, discussing art and the anticipation for the night’s events."
          "Theatres open their grand doors to eager, well-dressed audiences, and the glow from the chandeliers spills onto the sidewalks. A sense of cultured excitement fills the air." ]
    | Night
    | Midnight ->
        [ "The cultural district is hushed, save for the distant applause from a concert hall as a performance ends. Soft lights highlight stately facades and empty, moonlit courtyards."
          "Museum interiors are dark, but security lights trace the outlines of grand columns and statues. The echo of late-night rehearsals can occasionally be heard from an open window of the conservatory."
          "Few wander the moonlit plazas, where shadows cast dramatic shapes against gallery walls. The cultural heartbeat of the city slows to a gentle, dreaming pace under the stars." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The street awakens with the rumble of delivery trucks lining up at the gates of a sprawling factory complex. A plume of steam emits from a chimney as the first shifts arrive, many by a dedicated tram line."
          "Warehouse doors roll open, revealing rows of crates and machinery bathed in pale morning light. Workers in overalls move purposefully between old brick buildings and modern metal sheds."
          "The scent of oil and metal mingles with the cool dawn air. The pinging of a reversing forklift and the distant clang of metal on metal are the first sounds of the working day." ]
    | Midday
    | Afternoon ->
        [ "The industrial district hums with activity: conveyor belts whirr inside a bottling plant, cranes groan in a scrap metal yard, and machinery rattles inside open workshops. Forklifts weave between shipping containers."
          "Old brick factory buildings glint under the midday sun as workers coordinate loading and unloading tasks. The air is thick with mechanical sounds and the smell of welding."
          "Delivery vans and heavy trucks traverse the wide boulevards, while sparks from a welding torch occasionally flit behind the open shutters of a metal workshop. A freight train rumbles past on an adjacent track." ]
    | Evening ->
        [ "The shift change sees workers streaming from factory gates under amber floodlights, heading for the tram stop. The hum of machinery calms as evening routines begin."
          "Loading docks become quieter, lit only by harsh overhead lamps. The smell of industrial grease and coolant hangs in the cooling air. A security guard begins their patrol along a chain-link fence."
          "Large overhead doors close one by one, and the district takes on a more open, desolate feel under the glow of streetlights. The daytime energy has completely vanished." ]
    | Night
    | Midnight ->
        [ "Most factories are silent now, their machinery dormant beneath stark floodlights. A few night-shift workers move like ghosts through fenced pathways."
          "The industrial area feels deserted except for the glow of security lights and the occasional patrol vehicle. The air holds a metallic chill. The vast, dark shapes of the factories are imposing."
          "Shadows stretch across empty warehouses, and the distant hum of a single generator or a ventilation fan is the only reminder of the day’s bustle in this forgotten corner of the city." ]
