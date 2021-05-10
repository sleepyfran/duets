# Entities

Entities represent the domain of the game and that are used in all the rest of the layers. These entities contain only
the type definition (inside of the `Types.fs` file) and validation logic through their `create` functions which
encapsulate the entity creation to only allow valid data.

## Lenses

All (or almost all) the types defined in `Types.fs` will have a lens defined in the `Lenses.fs` file. These lenses are pairs of getters and setters that define how to access and update the inner properties of a record as well as let us combine them to easily access deeply nested properties on records, which is a must given that the whole core of Duets is inmutable by default and there's a lot of nesting in the main State. These lenses are used everywhere in the codebase through a library called Aether that exposes utilities for combining, querying and updating lenses.

For more details read the [Aether's guide for Lenses](https://xyncro.tech/aether/guides/lenses.html).
