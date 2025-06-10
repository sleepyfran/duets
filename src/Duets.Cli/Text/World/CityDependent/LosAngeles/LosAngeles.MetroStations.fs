[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.LosAngeles.MetroStations

open Duets.Cli.Text.World.Common
open Duets.Entities

let rec description dayMoment descriptor =
    match descriptor with
    | Bohemian -> bohemian
    | BusinessDistrict -> businessDistrict
    | Creative -> creative
    | Coastal -> coastal
    | Cultural -> cultural
    | EntertainmentHeart -> entertainmentHeart
    | Glitz -> glitz
    | Historic -> nonExistent
    | Industrial -> industrial
    | Luxurious -> luxurious
    | Nature -> nature
    |> fun fn -> fn dayMoment

and private bohemian dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The metro station here is small and unconventional, with vibrant murals covering the walls. The air smells faintly of incense and the lingering aroma of morning coffee. A few early commuters, often seen with instruments or art portfolios, wait patiently."
          "The platform is dimly lit with repurposed lighting fixtures, and the walls are covered with small, colorful tiles and murals. A single busker strums quietly in a corner, waiting for his audience."
          "The station itself seems like it was carved from some pre existing space in a building and it barely seems fit for purpose. A mix of travelers and artistic types wanders through the small underground corridor and makes its way on the narrow stairs." ]
    | Midday
    | Afternoon ->
        [ "The station echoes with the chatter of conversations, the sounds of the crowd are lively and the overall effect makes the station seem full of energetic activity. Flyers for art exhibitions and local gigs line the walls and their colors add to the visual spectacle."
          "The station is filled with the low murmur of conversations from artists and free-spirited locals mingling among the more traditional travellers. The walls feature an ever-evolving series of graffiti art pieces that never stay still."
          "Small groups pass you by on all sides, dressed in casual clothes, with musical instruments or with small canvases or artwork portfolios. The station is now noticeably more busy than it was just hours ago. " ]
    | Evening ->
        [ "The dim platform is filled with an eclectic mix of people, many wearing band t-shirts and unique styles. The smell of spices and food from nearby eateries mixes with the scent of ozone."
          "The station is crowded as commuters head to and from the many live venues dotted throughout this district, and the overall atmosphere feels very exciting and energized. There's a faint background noise of music and a clear expectation of excitement."
          "The station is filled with a constant but easy to tolerate chatter and background music as more travellers and patrons make their way to various events and shows nearby. The crowd mostly looks like artistic types, making their way in various directions. " ]
    | Night
    | Midnight ->
        [ "The station is eerily quiet now, save the soft whir of the escalators and the occasional announcement. Most travellers have long since left and it feels as if only the ghosts of late travellers remain here."
          "The station feels almost deserted now with most businesses closed for the night and those who remained having already boarded trains. There's an empty sense of finality here."
          "The station is mostly empty now and few people remain waiting for the night train. An occasional worker appears every now and again but mostly they disappear as soon as they can." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is a gleaming space of polished steel and bright fluorescent lights. Well-dressed professionals stride purposefully through, clutching coffee cups and briefcases, there's a sense of rushing efficiency here."
          "The station's design emphasizes functionality, with clear signage and wide walkways. Commuters briskly navigate their way to and from the many trains that make the journeys so popular during these working hours."
          "The large station feels as if it's readying itself for the day and people are arriving in ever increasing numbers. Suits and briefcases dominate as people rush to begin their various careers and roles." ]
    | Midday
    | Afternoon ->
        [ "The station is consistently busy with people heading to or from their various appointments and breaks.  The sounds of brief conversations and announcements bounce off the high glass surfaces all around."
          "The station is filled with workers heading to grab quick lunches or making their way to more distant appointments for their respective work schedules. The flow of traffic is non-stop."
          "The vast station echoes with the clatter of footsteps on the polished tile and brief phone calls about office tasks being completed or needing completion. People quickly navigate their way to their required platforms or trains." ]
    | Evening ->
        [ "The platform has become a little more relaxed than during the morning's rush hour and a clear flow can be seen leading most of the travellers directly to the trains leading home. There's a subtle calm here."
          "The station is becoming less populated as the sun goes down. Workers make their way steadily toward trains as the working day slows to a close.  A mix of tired faces and clear relief pervades the station's mood."
          "The traffic here is considerably less now than in earlier parts of the day, and many who remain seem to have completed their working day and are beginning their commutes home.  They do not linger." ]
    | Night
    | Midnight ->
        [ "The vast station feels deserted now, with only the occasional worker lingering here or there. It’s mostly quiet except for the rumble of the odd departing train."
          "The station feels large, mostly dark and uninviting as the final patrons leave for the night, or arrive on the very last train. It feels somewhat hostile to lingering"
          "The station's lighting dims to a minimal setting, and the echoing platforms feel enormous with so few people now on them. The sounds feel very distant as most noise ceases to persist. " ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is brightly lit with the bare minimum of decoration, focusing instead on utilitarian architecture. The commuters are often seen carrying creative supplies or wearing unique clothing. There's a slightly expectant atmosphere here."
          "The station platform is fairly small and quiet as the artist communities make their way out of it. Many can be seen with sketch books or art supplies under their arms or tucked in various backpacks. It feels like it's preparing for its patrons arrival."
          "The platform is usually sparsely populated, with only the occasional creative heading through to begin their day and make their commute. It feels relaxed, easy and very personal. " ]
    | Midday
    | Afternoon ->
        [ "The station is constantly changing due to the use of a huge whiteboard being displayed near the entryway for patrons to write, draw or otherwise modify. It creates an ever-shifting mood and a busy, dynamic feel to it."
          "The station now buzzes with energy, as local creators and artists meet before embarking on creative projects or classes or exhibitions throughout the nearby area. It's a place that has the feeling of friendly community."
          "Many people pass you by dressed in stylish clothes and carry art supplies. There are many unique forms of creative style and attire visible and it creates a visual spectacle here in the station. " ]
    | Evening ->
        [ "The station is very active with small crowds of people heading to nearby bars and galleries.  The mix of styles and designs seems extremely impressive."
          "The station becomes quite loud in the evening as crowds of people descend into it, heading toward the many galleries or exhibitions dotted nearby. It's a very creative experience in and of itself."
          "The station hums with an energy as it is flooded with stylish and interesting travellers all on their way to an interesting array of happenings within the zone.  It feels both vibrant and appealing." ]
    | Night
    | Midnight ->
        [ "The platform is quiet, save the occasional late commuter and the echo of announcements. Most travellers have long gone home. It feels quiet and empty."
          "The station is very dim now with only emergency lights and an odd patron waiting on a particularly late train. There is an empty and ominous feeling in the space now."
          "The station feels empty and forlorn and many of the displays and announcements that once animated the location are now silent. The feeling of loneliness seems complete here." ]

and private coastal dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is breezy, with a salty tang in the air and the distant sound of waves reaching the platform. Many commuters wear light clothing or carrying beach gear on their way for a day out."
          "The station feels calm and peaceful and mostly populated with people who seem ready to enjoy the day at the coast. Most people seem upbeat and lighthearted, it sets the mood perfectly."
          "The area seems almost empty beyond a few keen enthusiasts making an early journey out to the water. There's a sense of quiet anticipation to the whole atmosphere." ]
    | Midday
    | Afternoon ->
        [ "The station is very loud with the voices of vacationers all on their way to enjoy a trip to the shore. The air smells of suntan lotion and snacks. Everyone seems ready for a trip to the water."
          "The station is extremely busy now as holiday makers pass in their bright colorful swimwear and many wear sunglasses on top of their head. The sounds of their laughter seem easy and infectious."
          "Large groups crowd the various platforms as they all prepare for their respective journeys to the coast and beaches beyond. It feels extremely loud and active at this moment." ]
    | Evening ->
        [ "The station is populated by sunburned travellers slowly shuffling home, and the distinct sounds of children quietly complaining fill the platform."
          "The platform now contains tired travellers all coming back from a full day at the coast. It feels less excited now and much more subdued. Some look eager to get home now and some linger for late snacks."
          "The station fills up with people now on their way home, many looking content with their trip to the coast but none seeming interested in lingering. It feels quiet, slow and somewhat tired now. " ]
    | Night
    | Midnight ->
        [ "The station is mostly deserted. The occasional night worker can be seen cleaning up the previous days mess, the feeling of empty sadness permeates it here."
          "The platform here is totally abandoned by travellers, leaving just a few night cleaners and workers doing small maintenance tasks in this huge empty structure. It feels incredibly lonely and vast here."
          "The station feels incredibly large when seen now as most of the lighting has been shut off. Few remain here, either having been sent to work the closing shifts or because they were particularly late getting home. " ]

and private cultural dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station has a grand, almost reverent feel, with high ceilings and informational placards on the walls. Early visitors and museum staff move quietly through the space."
          "The air is still and cool, with the faint scent of old paper and polished wood. The platform is quiet, with only a few people waiting for the first trains of the day."
          "Sunlight streams through large windows, illuminating dust motes dancing in the air. The station feels like a gateway to knowledge, a quiet starting point for a day of exploration." ]
    | Midday
    | Afternoon ->
        [ "The station is now bustling with a diverse crowd of tourists, students, and locals. The sound of different languages fills the air, creating a vibrant, cosmopolitan atmosphere."
          "Groups of schoolchildren, led by their teachers, add a lively energy to the station. The walls are adorned with posters for current and upcoming exhibitions."
          "The station is a hub of activity, a crossroads where people from all walks of life intersect on their way to the nearby cultural institutions. It feels educational and lively." ]
    | Evening ->
        [ "The crowds have thinned, and a sense of calm descends upon the station. The lighting is softer now, creating a more intimate and contemplative mood."
          "The station is quieter, with the remaining travelers looking thoughtful, perhaps reflecting on the art or history they've just experienced. It feels like a place of quiet reflection."
          "As the nearby museums and galleries close, the station becomes a place of departure. The energy of the day gives way to a peaceful, end-of-day stillness." ]
    | Night
    | Midnight ->
        [ "The station is nearly deserted, its grand architecture now feeling somewhat imposing in the silence. The only sound is the distant rumble of the last trains."
          "The platforms are empty, and the informational displays are turned off. The station sleeps, holding the stories and histories of the day within its silent walls."
          "A profound quiet has settled over the station. It feels like a historical monument in its own right, a silent witness to the comings and goings of countless people." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is unnervingly silent as the echoes of last night's shows are left to hang in the air. Only the very early workers and commuters wander the empty platforms."
          "The large space here is completely empty of activity and feels a little weird and strange considering how loud it usually gets. It is difficult to believe it is in the same place"
          "The platforms and the corridors seem almost ghostly when seen this quiet as they have been swept and scrubbed ready to begin serving customers once again today. There is a strange emptiness to it." ]
    | Midday
    | Afternoon ->
        [ "The station feels increasingly full as it fills up with curious theatre-goers making their way toward the many restaurants and cafes around the area.  Many people are wearing their party clothes early."
          "The station feels slowly getting busier with anticipation and groups now forming as people discuss what acts and shows they're about to see this evening. The excitement seems real and contagious."
          "There's an interesting buzz in the air, and the various groups seem increasingly loud and more animated in preparation for the various shows or acts they are heading out to. There are many different styles on display." ]
    | Evening ->
        [ "The station pulsates with energy as patrons rush to shows and performances, and there is a steady loud chattering going on everywhere. The neon lighting feels all encompassing here as patrons descend into the area."
          "The station throbs with a constant flow of energy and the mood here feels extremely exciting as people dressed in their finery are seen making their way to various performances dotted throughout the district. There is anticipation in the air here"
          "The station is packed and almost feels as if it is bursting with all the various visitors from every walk of life. The atmosphere is electric and full of anticipation and eagerness.  It feels completely full of energy. " ]
    | Night
    | Midnight ->
        [ "The station is notably quieter as people slowly trickle out after various shows. A subdued air of tired satisfaction lingers here. Some have the quiet but distinct buzz of success in their gait and they almost seem to be floating along the various platforms."
          "The station seems much more spacious than it did earlier in the day as most of the day trippers have left or settled down into restaurants. The steady but dwindling sound of departures echo out to a large degree."
          "The station has a slower pace as late comers slowly leave to make their journeys back home. It seems much more quiet now. Those that remain are likely very tired. " ]

and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is quiet, with immaculate polished surfaces reflecting the soft morning light. Very few individuals linger, they seem mostly staff getting ready for the day."
          "The station is large, clean and mostly empty. Everything looks sleek and high-end even though only staff can be seen passing you by. There are very few early travelers on the platform."
          "The station is extremely subdued, with soft lighting, and has the sense of waiting to be filled. Nothing seems out of place here, everything looks carefully placed. Staff work silently and efficiently." ]
    | Midday
    | Afternoon ->
        [ "The station sees a steady flow of affluent shoppers, often wearing expensive garments or carrying designer bags. Everything about the experience here feels deliberately opulent and exclusive."
          "The platform has an air of exclusivity about it, with visitors wearing high-end fashion accessories. Most do not linger here, moving only from place to place with their expensive attire. "
          "The station appears busy, but everyone is dressed in upscale garments that set them clearly apart from other metro travelers. All the platforms look very stylish and expensive." ]
    | Evening ->
        [ "The station has an air of elegance as visitors with money mingle with patrons at the many surrounding areas. High end clientele form small groups on the platform, discussing private meetings."
          "The platform seems as though it were created for VIPs. It is not very full and most travelers seem to know where they are going. There's a sense of importance that is being conveyed."
          "The lighting here seems warmer and more exclusive. A high end crowd dominates here and many linger on their phones in various exclusive gatherings as though this is a second home to them." ]
    | Night
    | Midnight ->
        [ "The station is deserted, with the lights dimmed to a minimal setting and only security personnel still patrolling. It has a distinctly lonely feel"
          "The platforms seem unnaturally vast with the limited night time lights cast across it. There are so few people visible that it feels as though the place is totally devoid of travellers, beyond night time employees."
          "The platforms have a strangely austere tone and it almost seems that everything there is perfectly styled and waiting for visitors to arrive in the next day but few remain to experience its careful designs." ]

and private industrial dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station is stark and functional, built with concrete and steel. The air smells of diesel and metal, and the sound of heavy machinery can be heard in the distance."
          "Workers in high-visibility jackets and steel-toed boots form the bulk of the commuters. There's a sense of purpose and industry in the air, a place waking up to a day of hard work."
          "The platform is crowded with people heading to their shifts. The conversations are practical and direct, the atmosphere is one of focused energy." ]
    | Midday
    | Afternoon ->
        [ "The station is a constant flow of workers coming and going for their breaks. The noise from the nearby industrial zone is a constant backdrop to the station's activity."
          "The station is utilitarian and no-frills. There are no decorative elements, only functional signage and durable benches. It's a place of transit, not of leisure."
          "The air is thick with the smells of manufacturing. The station serves its purpose efficiently, a vital link in the chain of production." ]
    | Evening ->
        [ "The station is filled with tired workers heading home after a long day. The energy is subdued, a stark contrast to the morning's rush."
          "The noise from the industrial zone begins to subside as the evening shift ends. The station becomes quieter, the flow of people slowing to a trickle."
          "As darkness falls, the station's bright fluorescent lights seem harsh in the growing quiet. The last of the workers board their trains, leaving the station to the night." ]
    | Night
    | Midnight ->
        [ "The station is almost completely empty, save for a few security guards and maintenance workers. The silence is punctuated by the hum of the ventilation systems."
          "The air is cool and still, the industrial smells of the day having dissipated. The station feels vast and desolate in the quiet of the night."
          "Under the stark lighting, the station's raw, functional design is more apparent than ever. It's a skeleton of a place, waiting for the lifeblood of the workforce to return with the dawn." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station has high vaulted ceilings and polished marble floors that make the small area feel unexpectedly large and welcoming. Very few use the platform here this early, beyond staff making last minute adjustments"
          "The station is mostly empty in the mornings beyond a few uniformed staff working to maintain the area to an unusually high standard. The area looks very refined and new. There seems to be very few early morning travellers."
          "The platform seems unusually quiet in the early light and everything has been cleaned so efficiently that it looks almost like a hotel or reception lounge. It is clearly different to the other subway lines." ]
    | Midday
    | Afternoon ->
        [ "The station feels serene, with subdued lighting and comfortable seating areas, most of which are empty as there's nobody really waiting here for travel. It's not as busy or as crowded as other metro areas"
          "The area is usually quiet even at midday with very few people going through this route as other more efficient lines make better choices for their daily travel. Most can be seen idling with expensive smart devices."
          "The platforms here see limited travellers, with only an odd few here or there clearly looking relaxed and comfortable as if waiting in a private suite, but nobody lingers." ]
    | Evening ->
        [ "The platform now has a few visitors waiting patiently for trains and dressed in formal or sophisticated attire. Nothing seems too frantic here and the atmosphere seems very relaxed."
          "The station sees a slow increase of foot traffic, mostly well-to-do locals returning home from various venues or businesses. Everything feels comfortable and refined even when visitors are moving through."
          "The platforms see more movement than in the earlier parts of the day, and many carry shopping bags that are from designer or well-known stores, or they're being waited on by an attendant, before travelling further. " ]
    | Night
    | Midnight ->
        [ "The station is silent and dimly lit, the usual sounds and sights are replaced by only very quiet humming sounds and the odd announcement. All seems to be closed here."
          "The platform here is almost unpopulated as the day nears its end. The quiet of the place now feels strangely alien compared to the busier times of the day."
          "The lighting is kept very soft and nothing really seems to move beyond maintenance staff checking the location. It appears to have closed down entirely here now. " ]

and private nature dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The station has an earthy smell with damp stone walls and natural wood details giving it a warm organic feel. Many hikers and outdoor enthusiasts wait with rucksacks, poles and waterproof gear."
          "The platform is largely empty with the exception of the odd keen explorer already well on their way to their respective chosen trails. They mostly wear outdoorsy clothing or hiking gear."
          "The area is silent, calm and mostly empty. There is a sense of an exploration happening even in the emptiness here and few remain that will likely stay indoors this fine day." ]
    | Midday
    | Afternoon ->
        [ "The station smells like freshly cut grass and damp earth. The crowds appear relaxed, often chatting about the trials and tribulations of long nature walks. The overall mood is light and friendly. "
          "The platform is now quite busy as many begin to make their return from walks through the many wildernesses in the vicinity. The crowd appears both enthusiastic and exhausted all at once."
          "The station feels much more active as many people begin to slowly trickle back from long hours spent in the wilds nearby. They seem content and satisfied but look forward to resting up." ]
    | Evening ->
        [ "The platform contains a group of tired travelers, now heading home, many of them covered in mud.  They talk quietly as they wait for the platform announcements."
          "The station has the scent of nature clinging to it as travellers return from hikes and wilderness trails, and their conversations reveal a deep interest in the world outside the confines of the station."
          "The traffic has begun to significantly drop off here as hikers return from their explorations in the nearby woodlands. Many linger, comparing what experiences they had that day." ]
    | Night
    | Midnight ->
        [ "The station is very dark and eerily quiet now, even the sounds of insects seems somehow more distinct,  It feels almost like you have travelled too far from civilization"
          "The station has now been fully vacated by anyone seeking a journey out or in. Most light and noise has been reduced to a minimum now. The station feels entirely deserted. "
          "The station feels quiet, unlit and is mostly empty apart from insects that now reside in its many small crannies. The feeling of natural reclamation looms large in this deserted station. " ]
