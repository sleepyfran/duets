use super::Place;

/// Defines a city in the game. Must belong to a country.
#[derive(Clone, Default)]
pub struct City {
    pub id: String,
    pub name: String,
    pub population: i32,
    pub places: Vec<Place>,
}
