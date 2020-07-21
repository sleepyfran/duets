use super::city::City;

/// Defines a country in the game.
pub struct Country {
    pub name: String,
    pub cities: Vec<City>,
    pub population: i32,
}
