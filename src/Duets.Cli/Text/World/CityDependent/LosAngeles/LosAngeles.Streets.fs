[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.LosAngeles.Streets

open Duets.Entities

let rec description dayMoment descriptor =
    match descriptor with
    | Bohemian -> bohemian
    | BusinessDistrict -> businessDistrict
    | Creative -> creative
    | Coastal -> coastal
    | EntertainmentHeart -> entertainmentHeart
    | Glitz -> glitz
    | Luxurious -> luxurious
    | Nature -> nature
    |> fun fn -> fn dayMoment

and private bohemian dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The street is lined with small, colorful storefronts. Murals depicting abstract art adorn some walls while others sport peeling paint revealing a brick facade. Quirky boutiques and small cafes intermingle haphazardly, their open doorways spilling out onto the sidewalk. A few early-risers linger over coffee on the patios outside. Old vines curl up the buildings walls, almost as if they were trying to take the structures back to nature."
          "The street is awakening. Mismatched awnings shade the entrances of vintage shops and independent bookstores. The air is filled with the scent of brewing coffee and freshly baked bread wafting from open cafe doors. A lone artist sets up an easel on the sidewalk, preparing to capture the scene."
          "Small, quirky art galleries and co-working spaces with large windows line the street, many with hand-painted signs. The sidewalks are largely empty, save a few people on their way to get breakfast from the open coffee shops. Potted plants and discarded instruments give the streets a colorful and interesting look." ]
    | Midday
    | Afternoon ->
        [ "The street is lined with small, colorful storefronts. Murals depicting abstract art adorn some walls while others sport peeling paint revealing a brick facade. Quirky boutiques and small cafes intermingle haphazardly, their open doorways spilling out onto the sidewalk. A moderate crowd browses the storefronts and sips beverages on the patios. Old vines curl up the buildings walls, almost as if they were trying to take the structures back to nature."
          "A vibrant mix of art studios, thrift stores, and vegetarian eateries occupy the colorful buildings here. People mill between them in small groups, often examining quirky items. The street hums with an easygoing energy."
          "The street seems filled with hidden corners and tucked-away courtyards, often adorned with outdoor art installations. The buzz of conversations, snippets of music, and the general activity of passers-by fills the street with a pleasant chaos. Worn benches offer places for the locals to relax in the warm sun." ]
    | Evening ->
        [ "The street is lined with small, colorful storefronts. Murals depicting abstract art adorn some walls while others sport peeling paint revealing a brick facade. Quirky boutiques and small cafes intermingle haphazardly, their open doorways spilling out onto the sidewalk. The cafes are filling up with people as live music spills out of one venue. Old vines curl up the buildings walls, almost as if they were trying to take the structures back to nature."
          "Dim lights filter through the windows of intimate pubs and candlelit restaurants that are hidden within the bohemian storefronts.  A few artists work within the bright windows of their open ateliers, as more people join in, drawn by the alluring atmosphere."
          "The street has now become alive, and the windows reveal dimly lit galleries filled with intriguing pieces. The street has a different feel now with fewer storefronts and instead live music filling the air with its exciting buzz, and small groups cluster on street corners." ]
    | Night
    | Midnight ->
        [ "The street is lined with small, colorful storefronts. Murals depicting abstract art adorn some walls while others sport peeling paint revealing a brick facade.  Quirky boutiques stand with closed and locked doors while the cafes that stay open spill dim light out into the street. A few linger on the patios outside, laughing quietly. Old vines curl up the buildings walls, almost as if they were trying to take the structures back to nature."
          "The street is dimly lit by the occasional streetlight, casting shadows that stretch across the closed boutiques. The soft glow of warm lights seeps from the few establishments that remain open, often with small crowds standing outside or moving to other venues nearby."
          "Many businesses are shuttered, but the street has an almost hidden appeal when viewed at night. Streetlights cast strange shadows and the various decorations placed along the street show differently than during the daytime. Very few linger here as the night goes on and it grows very still." ]

and private businessDistrict dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Towering glass skyscrapers cast long shadows down upon the street, punctuated by smaller brick structures housing various offices. The buildings are all of impressive scale with large mirrored windows. The sound of car traffic is consistent and low here. The sidewalks are crowded with people hurrying to and from."
          "The street is a canyon of steel and glass, as the sun is just now beginning to reflect in them. Pedestrians in suits and briefcases stride purposefully along the wide sidewalks. Newsstands and coffee carts do a brisk business. The air smells of exhaust and strong coffee."
          "Large, imposing structures dominate this area with reflective glass fronts and smooth, austere exteriors. The steady sounds of traffic is interrupted only by the occasional loud delivery truck. Groups of office workers are beginning their daily commutes to their respective buildings." ]
    | Midday
    | Afternoon ->
        [ "Towering glass skyscrapers cast long shadows down upon the street, punctuated by smaller brick structures housing various offices. The buildings are all of impressive scale with large mirrored windows. The sound of car traffic is consistent and low here. The sidewalks are busy but not as crowded as in the morning, and people stroll through them rather than rush."
          "The midday sun reflects strongly on the surfaces of the buildings that cast long shadows down the side of the road. Lunch crowds gather around food carts or spill out onto the sidewalks, the sounds of chatter echoing around them."
          "Large, towering structures of chrome and glass rise up from both sides of the street. People can be seen bustling between meetings, occasionally lingering at small snack bars set up on the sidewalk. The overall noise is quite loud from the sounds of car horns and excited conversation." ]
    | Evening ->
        [ "Towering glass skyscrapers cast long shadows down upon the street, punctuated by smaller brick structures housing various offices. The buildings are all of impressive scale with large mirrored windows. The sound of car traffic has become noticeably less here. The sidewalks are almost deserted, only punctuated by the occasional commuter hurrying home."
          "The towers are lit up in the approaching evening light, making them look both grand and imposing. The steady flow of commuters dwindles down the street now and the sidewalks have quietened significantly."
          "The street lamps start to flicker on as it approaches nighttime.  The street feels large and deserted. The car noises slowly become more infrequent and only the odd few remain heading to parking structures and private cars." ]
    | Night
    | Midnight ->
        [ "Towering glass skyscrapers cast long shadows down upon the street, punctuated by smaller brick structures housing various offices. The buildings are all of impressive scale with large mirrored windows. The sound of car traffic is minimal. The sidewalks are deserted and only lit by street lamps."
          "The skyscrapers cast long, dark shadows across the otherwise deserted street. The office towers are dark, save for a few maintenance lights. A lonely security guard can sometimes be seen within."
          "The area feels like a completely different place now when compared to the busy working day it is normally occupied by.  There is an eerie feel here in the silence and darkness and nothing really moves in it." ]

and private creative dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The street is starting to come alive with an energetic buzz. Large windows show off studios cluttered with art supplies, but very few occupants can be seen in the buildings as they are likely just opening up for the day. The buildings are clearly being converted to accommodate their present-day needs, making the architecture interesting and unusual. Occasional music practice can be faintly heard. A handful of people can be seen walking or cycling down the road."
          "The buildings here are a mishmash of renovated warehouses and new construction.  Large windows offer glimpses into workshops and studios being set up. The rhythmic sounds of drills, saws, and shaping machines can be heard from the outside, and the air feels energetic."
          "The buildings here are varied and seem to be converted to accommodate a vast range of industries. The sidewalks are completely empty with only a couple of people unlocking and getting ready to set up for the working day, but the promise of more bustle is implied from the look of it." ]
    | Midday
    | Afternoon ->
        [ "The street pulsates with an energetic buzz. Large windows show off studios cluttered with art supplies, and several creators can be seen within, painting or shaping clay. The buildings are clearly being converted to accommodate their present-day needs, making the architecture interesting and unusual. Music can often be heard being performed within. People meander through the sidewalks and admire the various artworks and storefronts."
          "The street hums with activity, the buzz of artistic creation permeating the area. The windows now all look full of busy individuals pursuing different skills and vocations, from sewing to sculpting. Small groups of people often form to look at specific shop fronts, or they enter them and disappear from view."
          "The street here feels busy and full of different energy and activity. The shops show off a wide range of different talents, and the architecture is interesting, showing off how much the businesses must have transformed the spaces around them. The sidewalk can sometimes become clogged with onlookers and the street is not empty for long." ]
    | Evening ->
        [ "The street pulsates with an energetic buzz. Large windows show off studios cluttered with art supplies, but mostly the lights are off and nobody is visible. The buildings are clearly being converted to accommodate their present-day needs, making the architecture interesting and unusual. A mix of loud live music, chatting, and laughing can be heard from various openings, and the crowd flows down the side of the road."
          "As evening descends the studio windows have now come to life with bright lights shining from them.  The street now hosts the gathering of patrons and creative types drawn to live musical performances and impromptu happenings."
          " The street has changed here and has come to life with new people now congregating on the street.  They come for new and innovative products sold by small scale artisans, and this causes a nice friendly throng that lingers for hours." ]
    | Night
    | Midnight ->
        [ "The street is mostly dark and very little music or activity is coming from the various buildings lining the side of the road. Most of the windows have the lights off and it is only possible to vaguely tell what is within the studio or store from the shadows cast from outside.  Very few are walking in the area and they are mostly moving quickly through it."
          "The street is quiet, but not deserted, with the occasional flicker of light within open windows. The signs outside indicate artistic studios and craft stores. Very few wander down the road or explore within, but a constant quiet murmur permeates the place still."
          "The street here appears entirely different at nighttime, and most of the businesses here are shut down for the day. Very few linger here as the darkness and silence feel lonely." ]

and private coastal dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Palm trees sway in the breeze beside a sandy pathway beside the road, casting dancing shadows across the pavement. Multi-colored storefronts selling beachwear or cheap treats populate the side of the road and their interiors are plainly visible from the outside. The nearby sound of waves is almost constant. A few people walk past in light clothing, or they are opening their stores."
          "The salt air mixes with the sweet smell of sunscreen, and the sound of waves are very loud here. Stores are opening up here, pulling out the tables to outside seating and unpacking new beach toys."
          "Bright colors and sun-faded buildings can be seen along the beach front. Very few wander down the streets, often alone on their way to start their shifts. The day seems very peaceful and quiet at this time." ]
    | Midday
    | Afternoon ->
        [ "Palm trees sway in the breeze beside a sandy pathway beside the road, casting dancing shadows across the pavement. Multi-colored storefronts selling beachwear or cheap treats populate the side of the road and their interiors are plainly visible from the outside. The nearby sound of waves is almost constant. A good crowd of beachgoers and tourists strolls past on both sides of the street."
          "The sun beats down on the street, casting sharp shadows on the bright storefronts. People emerge from the beach carrying colorful floats and towels. Ice cream stands have long queues of thirsty customers."
          "Beach goers in swimwear pass you by on all sides on their way to and from the water front.  The sun is hot and glares on the painted signs as people pause to find the next bit of cool shade. A constant loud chatter can be heard at the stores." ]
    | Evening ->
        [ "Palm trees sway in the breeze beside a sandy pathway beside the road, casting dancing shadows across the pavement. Multi-colored storefronts selling beachwear or cheap treats populate the side of the road and their interiors are plainly visible from the outside. The nearby sound of waves is almost constant. The street sees less action now, only occasional stragglers remain."
          "The air has grown a little cooler as the sun goes down, but people are reluctant to move inside, and still linger to enjoy the scenery. The sound of happy customers still echoes in the small side street."
          "The light has grown more diffuse as it goes down, casting shadows all around. There is a sudden shift of pace as most people begin to retire from the street and the side roads leading to the main beach become empty." ]
    | Night
    | Midnight ->
        [ "Palm trees sway in the breeze beside a sandy pathway beside the road, casting dancing shadows across the pavement. Multi-colored storefronts selling beachwear or cheap treats populate the side of the road, now all closed and darkened. The nearby sound of waves is almost constant. Almost nobody is here."
          "The street is very dimly lit by the warm glows of the various food vendors still open for business.  The sound of the waves is still a constant, almost overpowering all else here, even the very quiet conversation nearby."
          "The street here is very quiet and many shopfronts and small businesses are now locked up for the evening.  Only a few wander down the street as all those working have likely gone home and the place is now deserted." ]

and private entertainmentHeart dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Bright neon signs stand dormant, waiting to begin flashing as the sun sets. Grand theater facades stand shoulder to shoulder with casual restaurants, creating an eclectic and lively atmosphere.  There is a slight hum in the air, coming mostly from car traffic rather than crowds. The streets are mostly deserted as people rush to get to work."
          "The streets are mostly empty, and it feels strange in the morning light before the theaters start to wake up.  A handful of workers clear out the previous days rubbish. The overall atmosphere seems very quiet and uncharacteristic when compared to what this area is normally like."
          "The streets are bare, only showing off the various storefronts that make this street a cultural experience at night.   Few people walk through the streets and the neon signs still stand inactive." ]
    | Midday
    | Afternoon ->
        [ "Bright neon signs stand mostly still, but some are starting to flash in the fading sunlight. Grand theater facades stand shoulder to shoulder with casual restaurants, creating an eclectic and lively atmosphere.  There is a noticeable increase in activity now, as tourists stroll and plan for the evening shows. A constant mix of chat and small snippets of music comes from the various open establishments. People begin filling the side walks and planning for their evening entertainment."
          "The street fills slowly as people trickle in from other areas, planning their evening activity.  Small crowds gather at some of the louder bars and restaurants, waiting to begin the festivities early. "
          "The mood is beginning to feel a little more lively and bright as people start to fill the streets.  They gather at the shops here and plan their day, making sure they will be able to make it to all the various happenings." ]
    | Evening ->
        [ "Bright neon signs vie for your attention, their flashing lights illuminating the street with an intense vibrancy. Grand theater facades stand shoulder to shoulder with casual restaurants, creating an eclectic and lively atmosphere. The air buzzes with energy, a constant chatter and music spills out from the numerous establishments. Crowds fill the sides of the road and eagerly spill into the theaters."
          "The crowds here fill the street from all directions, coming to attend various theaters, restaurants and clubs.   The area is awash with noise and color."
          "The streets are extremely crowded here and everyone is heading towards a specific event.  The bright signs and storefronts call to you and the area throbs with sound and vibrant activity." ]
    | Night
    | Midnight ->
        [ "Bright neon signs still compete for your attention, although many are now turned off to save energy or closed down for the evening. Grand theater facades stand shoulder to shoulder with casual restaurants, creating an eclectic and lively atmosphere. There is less activity now as most shows are over, although several restaurants stay open to the late hours.  The streets now see a much smaller but persistent crowd that are likely on their way home."
          "The neon signs slowly flicker and turn off in groups. Many theaters close for the night, causing the streets to feel much more spacious than before. Some people are now beginning their homeward travels."
          "The lights seem to have faded and the crowds have dissipated now, and most venues have either closed or greatly reduced their activity. There is still a bustle of people walking along the side walk to get transportation home or seek a more quiet after hours establishment." ]


and private glitz dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "Immaculately polished surfaces gleam beneath the soft morning light and the architecture seems unusually modern and large. High-end boutiques are displayed like museums through their gigantic display windows. Luxurious cars occasionally move through the streets but otherwise there is little activity, and the area is surprisingly silent. A few staff members can be seen prepping storefronts for business."
          "The street feels very pristine in the early light, its carefully designed surfaces immaculate. Security cameras keep watch from multiple discreet locations and only small groups of staff wander along to unlock store doors."
          "The early sun glints from all surfaces and reveals a clean and perfect street, completely empty save a few delivery personnel dropping off packages at the closed doors of luxury boutiques." ]
    | Midday
    | Afternoon ->
        [ "Immaculately polished surfaces gleam beneath the sun's light, making the architecture seem even more grand. High-end boutiques are displayed like museums through their gigantic display windows. Luxurious cars are a common sight, ferrying in and out affluent shoppers. Some onlookers browse idly or gaze admiringly at the storefronts."
          "The street is very active, with high end shoppers wandering between stores and high-priced vehicles. It feels exclusive and upscale. All the storefronts have their goods carefully displayed in large and modern window displays. People gaze up admiringly from the side."
          "Luxury shoppers pass you by in expensive clothing, while others admire storefronts and occasionally pause to point out particular styles they like to those nearby. Cars pull in and pull out along the side of the street to quickly collect affluent patrons. The scene here appears both refined and bustling." ]
    | Evening ->
        [ "Immaculately polished surfaces gleam beneath the street lights and the architecture seems unusually modern and large. High-end boutiques are displayed like museums through their gigantic display windows. Luxurious cars line up along the curb, and there are small crowds congregating in certain areas. There is an air of exclusivity that doesn't prevent people from milling about."
          "As dusk settles, the bright street lights shine down on expensive window displays. Small crowds loiter about the outside, clearly there to be seen and admired. There's a feeling of exclusivity in the air as groups form for late evening cocktails and social events."
          "As the sun goes down, many well-to-do visitors linger on the street, dressed in expensive clothing. The area is well lit and everything seems very clean and new, creating an area that would appear extremely appealing." ]
    | Night
    | Midnight ->
        [ "Immaculately polished surfaces gleam faintly beneath the street lights and the architecture seems unusually modern and large. High-end boutiques are displayed like museums through their darkened display windows. Only the occasional luxury vehicle makes its way down the street.  Very few are visible here as they have likely gone home."
          "The stores are closed now but they glow faintly as their displays are left on over night, and it looks almost as if they are guarded. Only the odd luxury vehicle passes you by and otherwise the area seems to have shut down."
          "The area now feels very deserted as all but a few members of staff have gone home for the night. The streets are all empty and silent and everything glows with a sterile light." ]

and private luxurious dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The buildings here are uniformly high quality with well manicured lawns, neatly arranged shrubberies and subtle high-end touches. Cars with expensive tags crawl past the large townhouses lining the street. It's peaceful and quiet here, as the residents are only now coming outside."
          "Large townhouses dominate this street, almost as though they were built out of the same material. It feels calm and tranquil, but very secluded, only the odd landscaper is out."
          "The properties here seem large and impressive and the plants in the front garden are so well maintained they could be from a magazine. Few seem to occupy this area beyond gardeners and housekeepers and the day has an eerily peaceful quietness." ]
    | Midday
    | Afternoon ->
        [ "The buildings here are uniformly high quality with well manicured lawns, neatly arranged shrubberies and subtle high-end touches. Cars with expensive tags crawl past the large townhouses lining the street. It's peaceful and quiet here, with very few people visible beyond the odd gardeners."
          "The architecture along this road is all very high quality with every building looking brand new. Everything has a manicured look to it and little out of place beyond gardeners working in the perfectly styled areas."
          "The areas here seem almost unpopulated beyond those performing work to keep the buildings so clean and precise. A light breeze flows by and makes the tall shrubberies and grass move." ]
    | Evening ->
        [ "The buildings here are uniformly high quality with well manicured lawns, neatly arranged shrubberies and subtle high-end touches. Cars with expensive tags crawl past the large townhouses lining the street. There is a small increase in the visible amount of people compared to earlier, most of them seem to be arriving from or returning to their home."
          "The lighting here is extremely subtle and soft as the evening comes on. There is an increase of activity now but it all seems contained and small-scale, just a few members of the population of this place seem to be moving about."
          "There are signs of life here but it mostly consists of private chauffeurs waiting in cars or other staff in uniform wandering around outside of private premises. Very little of the residents here seem to walk through this street on their own." ]
    | Night
    | Midnight ->
        [ "The buildings here are uniformly high quality with well manicured lawns, neatly arranged shrubberies and subtle high-end touches. Cars with expensive tags are mostly absent now, having either been safely stored away or parked within a drive. The area is quiet and completely deserted, as most residents have gone home and turned the lights off. "
          "The area has a subdued look and only a few lit up windows give indication that residents are active inside. The roads and the sidewalk are entirely empty."
          "The lighting here feels soft and gentle in the silence and darkness. Nothing seems out of place or out of sync. There is a sense of loneliness that looms." ]

and private nature dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        [ "The buildings that can be seen from the street look small and insignificant. Mostly what you see here is foliage and the tall trees. The street here is narrow and seems to barely penetrate the wilderness that looms all around. The sound of birds chirping is surprisingly loud, and a few nature-goers can be spotted on the paths."
          "The forest presses right up against the edge of the narrow road. The buildings that do manage to penetrate feel small and unimportant when placed beside such tall trees. The sounds of the forest come to your attention easily, creating a loud chorus."
          "The road here feels like a thin cut made to permit traffic between thick woods.  Everything feels very calm and undisturbed, and very little besides birds can be seen on this peaceful road." ]
    | Midday
    | Afternoon ->
        [ "The buildings that can be seen from the street look small and insignificant. Mostly what you see here is foliage and the tall trees. The street here is narrow and seems to barely penetrate the wilderness that looms all around. The sound of birds chirping is clear and distinct and a handful of people hike or stroll within the wilds nearby."
          "The air smells of damp earth and the shadows grow longer, being stretched out over the sidewalk. The forest feels full of movement as various birds and wildlife flutter throughout."
          "The forest here looks thick and impenetrable, mostly only revealing tree trunks. Light shines through the leaves and highlights occasional birds that pass overhead." ]
    | Evening ->
        [ "The buildings that can be seen from the street look small and insignificant. Mostly what you see here is foliage and the tall trees. The street here is narrow and seems to barely penetrate the wilderness that looms all around. The sound of birds chirping is fading as they begin to nest. A few people are quickly returning home."
          "The light here is fading fast now and is mostly cut off by the dense leaf cover that dominates this area. Sounds from the wildlife grows more prominent now as birds and small insects begin their nocturnal routines."
          "The forest is growing more dark with the dying sun and is starting to grow strangely ominous. There are some last stragglers hiking back from the deep brush and shadows are starting to become deeper." ]
    | Night
    | Midnight ->
        [ "The buildings that can be seen from the street look small and insignificant. Mostly what you see here is foliage and the tall trees. The street here is narrow and seems to barely penetrate the wilderness that looms all around. The sound of insects dominates over the sound of birds.  No humans can be seen anywhere here now."
          "The road here feels very dark and seems barely lit by starlight that manages to penetrate the canopy above. There are very few human sounds here and they mostly have to do with the cars using the side road."
          "The area is mostly black and it can only be identified from the sparse bits of starlight that manage to slip through the heavy foliage that towers overhead. Insects now rule this domain." ]
