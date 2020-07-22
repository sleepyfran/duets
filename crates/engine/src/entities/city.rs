use super::Country;

/// Defines a city in the game. Must belong to a country.
pub struct City {
    pub name: String,
    pub country: Country,
    pub population: i32,
}
