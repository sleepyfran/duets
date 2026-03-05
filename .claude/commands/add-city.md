---
name: add-city
description: Generates a new city
parameters:
- name: city_name
  type: string
  required: true
---
I'm looking to add more cities into my game. Enter planning mode and follow the example of @src/Duets.Data/World/Cities/LosAngeles/ (and the attached zones) to add the city of {{city_name}}? Things that are needed are:
- Make sure you research real places from the city to make it realistic
- There should be enough concerts to host bands of all sizes, look at @tests/Data.Tests/World.Tests.fs for what's expected
- Divide the city into three to seven zones. Think zones as in big chunks of the city as they make sense per the city's actual distribution, it doesn't necessarily need to be actual bureaus or
  something like that.
- Add a simplified metro system with up to two lines (only Blue and Red) at the moment and make sure they connect in a similar-ish way of how they'd do in the real city.
- Make sure you add exactly one hospital and one airport in the zone you see fit.
- Each zone should contain two to five streets, all interconnected, and each street a few types of each place type.
