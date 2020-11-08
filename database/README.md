# Duets > Database 📁

The game uses a static JSON database to load all the static elements of the game: countries, cities, places, skills, instruments, etc. All these things are defined in the file `database.json` and validated with the schema defined in the `schema` folder.

In order to avoid repetition and manual errors, the database uses custom references that are not easy to deserialize with `serde` (the crate we're using to deserialize JSON), so the scripts defined in the `scripts` folder help with the validation and generation of a `serde` compatible JSON file that is output in the `generated` folder and contains the final database.

In order to generate the database simply run:

```bash
node database/scripts/generate-database.js
```

This will automatically load the `database.json` file, validate it and generate a compatible file for the game.
