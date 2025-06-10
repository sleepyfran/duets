[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Prague.MetroStations

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
        [ "The station is functional and a little grimy, with peeling posters for underground gigs on the walls. The air smells faintly of stale beer and yesterday's cigarette smoke. A few early commuters, students and artists with tired eyes, wait patiently for the first train."
          "The platform is dimly lit, the walls lined with standard-issue tiles. A lone figure with a guitar case sits on a bench, not playing, just waiting. The atmosphere is subdued, a quiet prelude to the neighbourhood's livelier rhythm."
          "The station itself is a concrete bunker, but the people give it its character. A mix of students and artistic types makes its way through the underground corridor, their footsteps echoing in the morning quiet." ]
    | Midday
    | Afternoon ->
        [ "The station now echoes with the chatter of students and locals. The sounds are lively, making the utilitarian space feel energetic. Flyers for pub quizzes and local bands are plastered over every available surface, adding a chaotic layer of colour."
          "The station is filled with a low murmur of conversation as artists and free-spirited locals mingle with regular commuters. The walls feature a few scrawled tags and witty graffiti pieces that seem to appear and disappear overnight."
          "Small groups pass you by, dressed in second-hand clothes, some carrying instrument cases or tote bags from independent bookshops. The station is noticeably busier now, a thoroughfare for the city's alternative scene." ]
    | Evening ->
        [ "The platform is filled with an eclectic mix of people, many in band t-shirts or punk attire, heading out for the night. The smell of frying onions from a nearby kebab stand mixes with the scent of ozone from the tracks."
          "The station is crowded as people head to and from the many smoky pubs and live music venues in the district. The atmosphere feels charged with anticipation for the night ahead."
          "The station is filled with a constant but easy-to-tolerate chatter as people make their way to various pubs and shows nearby. The crowd is a sea of creative and non-conformist styles." ]
    | Night
    | Midnight ->
        [ "The station is eerily quiet now, save for the hum of the escalators and the automated announcement, 'Ukončete prosím výstup a nástup, dveře se zavírají'. Most travellers have long since left for the last tram."
          "The station feels almost deserted now, with most of the nearby pubs closed for the night. There's an empty sense of finality here as the last train rumbles into the distance."
          "The station is mostly empty. A few night-shift workers and stragglers wait for the last connection. The colourful posters on the walls seem to mock the late-night quiet." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is a functional space of concrete and coloured tiles, a product of a different era. Well-dressed professionals stride purposefully through, a river of dark suits flowing up the long, fast escalators towards the glass towers above."
          "The station's design emphasizes efficiency, with clear Cyrillic-influenced signage and wide walkways. Commuters briskly navigate their way from the rumbling trains, a sense of hurried purpose in the air."
          "The station is filling rapidly as the morning rush begins. Suits and briefcases dominate as people hurry to begin their day in the surrounding corporate headquarters, a stark contrast to the station's socialist-era architecture." ]
    | Midday
    | Afternoon ->
        [ "The station is consistently busy with people heading to and from lunch appointments. The sounds of brief conversations and announcements bounce off the tiled walls, creating a constant, low hum of activity."
          "The station is filled with workers grabbing a quick bite from the kiosks in the concourse or making their way to meetings. The flow of traffic up and down the escalators is non-stop."
          "The station echoes with the clatter of footsteps on the stone floors and snippets of phone calls about deadlines and meetings. People quickly navigate towards their required platforms, rarely making eye contact." ]
    | Evening ->
        [ "The platform has become a little more relaxed than during the morning's rush. A clear flow of tired commuters heads for the trains leading away from the offices. There's a subtle sense of collective relief."
          "The station is becoming less populated as the sun goes down. Workers make their way steadily toward the trains as the workday winds down. A mix of tired faces and loosened ties pervades the station's mood."
          "The traffic here is considerably less now. Those who remain seem to have completed their working day and are beginning their long commutes home. They do not linger." ]
    | Night
    | Midnight ->
        [ "The vast station feels deserted now, with only the occasional cleaning crew member present. It’s mostly quiet except for the deep rumble of the odd departing train in the tunnel."
          "The station feels large, dark, and uninviting as the final patrons leave for the night. The long, silent escalators seem to lead into an abyss."
          "The station's lighting dims to a minimal setting, and the echoing platforms feel enormous and vaguely oppressive with so few people on them. The sounds of the city above are completely muted here." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is stark and utilitarian, often a concrete shell with minimal decoration. The first commuters are often seen with laptops or designer tote bags. There's a quiet, focused atmosphere here, the calm before the creative storm."
          "The station platform is fairly small and quiet as designers and programmers make their way out of it. It feels like a functional gateway to the trendy, post-industrial neighborhood above."
          "The platform is sparsely populated, with only the occasional creative heading through to their studio or co-working space. It feels relaxed, easy, and very personal." ]
    | Midday
    | Afternoon ->
        [ "The station now buzzes with a quiet energy, as local creators and tech workers meet before heading to a nearby gallery or concept cafe. It's a place that has the feeling of a modern, networked community."
          "The station is a parade of minimalist fashion and stylishly unconventional looks. It now feels like a hub for the area's design and tech industries, with conversations about projects and funding happening on the platforms."
          "Many people pass you by dressed in stylish, often monochrome clothes, carrying laptops or architectural drawings. There are many unique forms of creative style visible, creating a subtle visual spectacle." ]
    | Evening ->
        [ "The station is very active with small crowds of people heading to nearby gallery openings and hip, post-industrial bars. The mix of styles is impressive and understatedly cool."
          "The station becomes quite lively in the evening as crowds descend into it, heading toward the many galleries or alternative theatre spaces dotted nearby. It's a creative experience in and of itself."
          "The station hums with an energy as it is flooded with stylish travellers on their way to an interesting array of happenings. It feels both vibrant and appealingly exclusive." ]
    | Night
    | Midnight ->
        [ "The platform is quiet, save for the occasional late commuter and the echo of announcements. Most have moved on to the district's late-night venues. It feels quiet and empty."
          "The station is very dim now, with only safety lights on. An odd patron waits on a particularly late train. There is an empty and slightly desolate feeling in the concrete space."
          "The station feels empty and forlorn. The posters for art exhibitions that once animated the location are now just paper in the dim light. The feeling of loneliness seems complete here." ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station has a grand, slightly cavernous feel, with high ceilings and mosaics on the walls. Early visitors and museum staff move quietly through the large interchange."
          "The air is still and cool. The platform is quiet, with only a few people waiting for the first trains of theday, heading to work in the surrounding theatres and galleries."
          "Sunlight streams through the glass entrance hall above, illuminating dust motes dancing in the air. The station feels like a gateway to culture, a quiet starting point for a day of exploration." ]
    | Midday
    | Afternoon ->
        [ "The station is now bustling with a diverse crowd of tourists, students, and locals. The sound of different languages fills the air, creating a vibrant, cosmopolitan atmosphere in the busy interchange."
          "Groups of schoolchildren, led by their teachers, add a lively energy to the station as they navigate from one line to another. The walls are adorned with posters for operas and classical concerts."
          "The station is a hub of activity, a crossroads where people intersect on their way to the National Museum or the State Opera. It feels educational and slightly chaotic." ]
    | Evening ->
        [ "The crowds have thinned, but a new, more elegant group of people arrives. Well-dressed couples and families head towards the nearby theatres, and a sense of calm descends upon the station."
          "The station is quieter, with travellers looking thoughtful, perhaps anticipating the performance they are about to see. It feels like a place of cultured expectation."
          "As the nearby museums close, the station becomes a place of arrival for the evening's cultural offerings. The energy of the day gives way to a sophisticated, peaceful stillness." ]
    | Night
    | Midnight ->
        [ "The station is nearly deserted, its grand architecture now feeling somewhat imposing in the silence. The only sound is the distant rumble of the last trains carrying the last of the theatre-goers home."
          "The platforms are empty, and the posters for grand performances are just silent images on the walls. The station sleeps, holding the cultural echoes of the day within its silent walls."
          "A profound quiet has settled over the station. It feels like a monument in its own right, a silent witness to the comings and goings of countless people." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is unnervingly silent, the air still thick with the faint, sweet smell of spilled beer and fast food from the night before. Only cleaners and early workers wander the empty, labyrinthine platforms."
          "The large space here is completely empty of activity and feels strange considering how loud and crowded it usually gets. The closed grilles of the underground shops make it feel like a ghost town."
          "The platforms and corridors seem almost ghostly, having been scrubbed clean and ready to begin serving customers once again. There is a strange, hollow emptiness to it all." ]
    | Midday
    | Afternoon ->
        [ "The station fills up with curious tourists making their way toward the many restaurants and shops in the area. The smell of hot dogs and roasting sausages from the street-level stalls begins to drift down."
          "The station is slowly getting busier with anticipation. Groups now form as people discuss where to go and what to see. The excitement of the tourists seems real and contagious."
          "There's an interesting buzz in the air, and the various groups seem increasingly loud and more animated in preparation for their evening. There are many different styles on display, from tour groups to stag parties." ]
    | Evening ->
        [ "The station pulsates with energy as patrons rush to and from the streets above. A steady, loud chattering is everywhere. The garish light from the surrounding casinos and clubs filters down, giving the station a lurid glow."
          "The station throbs with a constant flow of energy, the mood extremely chaotic as people dressed for a night out mix with bewildered tourists. There is a palpable sense of frantic energy here."
          "The station is packed and almost feels as if it is bursting with visitors from every walk of life. The atmosphere is electric and full of a loud, commercial eagerness. It feels completely full of raw energy." ]
    | Night
    | Midnight ->
        [ "The station is notably quieter as people slowly trickle out after the main clubs have emptied. A subdued air of tired, drunken satisfaction lingers here. Some people are floating along the platforms, others are stumbling."
          "The station seems much more spacious than it did earlier as most of the crowds have left. The steady but dwindling sound of departing trains echoes through the now-emptier halls."
          "The station has a slower pace as late-night revellers make their journeys back home. It seems much quieter now. Those that remain are likely very, very tired." ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is quiet, its clean stone surfaces reflecting the soft morning light. The few individuals who pass through are mostly staff from the luxury boutiques above, heading to work."
          "The station is large, clean, and mostly empty. Everything looks well-maintained, but it's just a public space. The sense of luxury is absent, waiting for its patrons to arrive."
          "The station is extremely subdued, with soft lighting and a sense of waiting to be filled. Staff work silently and efficiently, but there are no early-morning high-end shoppers here." ]
    | Midday
    | Afternoon ->
        [ "The station sees a steady flow of affluent shoppers, often identifiable by their expensive handbags and tailored clothes. They move with a purpose, using the metro as a simple convenience."
          "The platform has an air of detached elegance. Visitors in high-end fashion stand apart from the regular commuters. Most do not linger, moving quickly towards the exit for Pařížská street."
          "The station appears busy, but a certain class of passenger is dressed in upscale garments that set them clearly apart. They bring an air of the luxury world above ground into this public space." ]
    | Evening ->
        [ "The station has an air of elegance as well-dressed visitors head to the area's exclusive restaurants. High-end clientele form small groups on the platform, their quiet conversations a contrast to the usual station noise."
          "The platform seems like a brief, functional stop for VIPs. It is not very full, and most travellers seem to be heading towards a specific, expensive destination. There's a sense of importance being conveyed."
          "The lighting here seems warmer. A high-end crowd dominates the platform, many lingering on their phones, arranging the rest of their exclusive evening." ]
    | Night
    | Midnight ->
        [ "The station is deserted, with the lights dimmed and only security personnel still patrolling. The contrast with the imagined luxury above is stark, leaving a distinctly lonely feel."
          "The platforms seem unnaturally vast with the limited night-time lights cast across them. The place is totally devoid of its daytime glamour, revealing its simple, functional core."
          "The platforms have a strangely austere tone. It seems that everything is perfectly styled and waiting for visitors to arrive the next day, but for now, it's just an empty, silent station." ]

and private historic dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station, a mix of modernism and unique artistic flourishes, is quiet. As you ride the long escalator up, you feel a shift in time, emerging into the crisp air of a cobbled square that has seen centuries of history."
          "The platform is largely empty. The station's unique, medieval-themed wall panels seem to hint at the world above. There is a sense of being in a time machine, about to step out into the past."
          "The station is silent, a modern cavern beneath an ancient city. The first few commuters are workers for the tourist shops and cafes, unlocking doors on streets that were laid out in the Middle Ages." ]
    | Midday
    | Afternoon ->
        [ "The station is now a major chokepoint, disgorging a constant stream of tourists into the historic lanes above. The sound of dozens of languages mixes with the rumble of the trains."
          "The platforms are crowded with tour groups, their guides trying to keep them together. The station serves as a stark, modern portal to the fairy-tale architecture of the Old Town or Lesser Quarter."
          "The station is a scene of organized chaos. People with cameras and maps pour out of the trains and are immediately swept up into the human current flowing towards the most famous sights." ]
    | Evening ->
        [ "The flow of people reverses as tired tourists descend back into the metro. A new, smaller crowd arrives, heading for dinner in ancient cellars or to see the city's landmarks illuminated at night."
          "The station's atmosphere becomes more relaxed. The frantic energy of the day is replaced by a more romantic, contemplative mood. The journey down the escalator feels like a return to the modern world."
          "The platforms are less crowded now. The station's lighting seems to echo the gas-lamp style lights of the historic streets above, creating a warm, inviting feel." ]
    | Night
    | Midnight ->
        [ "The station is almost completely empty. Riding the escalator up to a silent, moonlit square feels profoundly cinematic. The modern station is a stark, silent contrast to the ancient city sleeping above."
          "The last train departs, its rumble fading into the deep silence. The station feels vast and hollow, a forgotten cavern beneath the weight of so much history."
          "The station is closed, its metal gates pulled shut. It rests, a modern heart beating quietly beneath a city of ghosts and legends." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is stark and functional, built with concrete and steel. The air smells of diesel and damp metal, and the distant clang of a freight train can be heard."
          "Workers in overalls and heavy boots form the bulk of the commuters. There's a no-nonsense sense of purpose in the air, a place waking up to a day of hard work."
          "The platform is crowded with people heading to their shifts in the surrounding factories and warehouses. The conversations are practical and direct, the atmosphere is one of focused energy." ]
    | Midday
    | Afternoon ->
        [ "The station sees a steady flow of workers coming and going for their breaks. The noise from a nearby manufacturing plant is a constant backdrop to the station's activity."
          "The station is utilitarian and no-frills. There are no decorative elements, only functional signage and durable benches. It's a place of transit, not of leisure."
          "The air is thick with the smells of manufacturing. The station serves its purpose efficiently, a vital link in the chain of the city's production." ]
    | Evening ->
        [ "The station is filled with tired workers heading home after a long day. The energy is subdued, a stark contrast to the morning's rush. Faces are smudged with grease and fatigue."
          "The noise from the industrial zone begins to subside as the evening shift ends. The station becomes quieter, the flow of people slowing to a trickle."
          "As darkness falls, the station's bright fluorescent lights seem harsh in the growing quiet. The last of the workers board their trains, leaving the station to the night." ]
    | Night
    | Midnight ->
        [ "The station is almost completely empty, save for a few security guards and maintenance workers. The silence is punctuated by the hum of the ventilation systems."
          "The air is cool and still, the industrial smells of the day having dissipated. The station feels vast and desolate in the quiet of the night, surrounded by dark, silent factories."
          "Under the stark lighting, the station's raw, functional design is more apparent than ever. It's a skeleton of a place, waiting for the lifeblood of the workforce to return with the dawn." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station has impressively high ceilings and clean, polished stone floors. Descending the exceptionally long escalator feels like a journey in itself. Very few people use the platform this early."
          "The station is mostly empty, maintained to an unusually high standard. It serves an affluent residential area, and its few morning travellers are calm and unhurried."
          "The platform is unusually quiet in the early light. Everything has been cleaned so efficiently that it looks almost like a private terminal. It is clearly different from the busier downtown lines." ]
    | Midday
    | Afternoon ->
        [ "The station feels serene, with subdued lighting and well-maintained benches, most of which are empty. It's not as busy or crowded as other metro hubs, serving as a quiet transit point for the local residents."
          "The area is usually quiet even at midday. The few people travelling through seem relaxed and are often seen with expensive-looking coats or small dogs."
          "The platforms here see limited traffic, with only an odd few people here or there, clearly relaxed and comfortable as if waiting in a private lounge. Nobody seems to be in a rush." ]
    | Evening ->
        [ "The platform now has a few more visitors, waiting patiently for trains, often dressed in formal or sophisticated attire for an evening out. The atmosphere remains very relaxed."
          "The station sees a slow increase of foot traffic, mostly well-to-do locals returning home from offices or heading to one of the district's fine dining restaurants. Everything feels comfortable and refined."
          "The platforms see more movement than earlier in the day. Passengers often carry shopping bags from high-end stores, a sign of their day's activities before returning to their quiet, elegant neighbourhood." ]
    | Night
    | Midnight ->
        [ "The station is silent and dimly lit. The exceptionally long, motionless escalator is an imposing sight in the quiet. The station seems to be sleeping along with the affluent neighbourhood above."
          "The platform here is almost unpopulated as the day nears its end. The quiet of the place now feels strangely alien compared to the bustling hubs elsewhere in the city."
          "The lighting is kept very soft, and nothing seems to move. The station appears to have closed down entirely, a quiet and empty space deep beneath the ground." ]

and private nature dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station has an earthy smell, with damp stone walls and exits that open directly towards a large park. Many of the first passengers are hikers and dog walkers, equipped with rucksacks and waterproof gear."
          "The platform is largely empty with the exception of a few keen explorers already on their way to the trails. They mostly wear outdoorsy clothing and hiking boots."
          "The area is silent, calm, and mostly empty. There is a sense of an impending journey into the wilderness, even in this underground space. Few people here will be staying indoors today." ]
    | Midday
    | Afternoon ->
        [ "The station smells like freshly cut grass and damp earth from the boots of returning walkers. The crowds appear relaxed, often chatting about their routes and the beauty of the nearby forest."
          "The platform is now quite busy as many begin to make their return from walks through the vast urban wilderness. The crowd appears both enthusiastic and exhausted all at once."
          "The station feels much more active as people slowly trickle back from long hours spent in the wilds. They seem content and satisfied but look forward to resting up at home." ]
    | Evening ->
        [ "The platform contains groups of tired travelers now heading home, some of them covered in mud. They talk quietly as they wait for the train back to the city center."
          "The station has the scent of the forest clinging to it as travellers return from hikes. Their conversations reveal a deep appreciation for the natural world just outside the city."
          "The foot traffic has begun to significantly drop off here as the last hikers return from their explorations. Many linger, comparing what experiences they had that day." ]
    | Night
    | Midnight ->
        [ "The station is very dark and eerily quiet. The sound of crickets from the park entrance seems to echo in the deserted concourse. It feels as if you have travelled too far from civilization."
          "The station has now been fully vacated by anyone seeking a journey out or in. Most light and noise has been reduced to a minimum. The station feels entirely reclaimed by the quiet of the night."
          "The station feels quiet and unlit. It's mostly empty apart from insects that shelter in its small crannies. The feeling of the nearby, dark forest looms large in this deserted station." ]
