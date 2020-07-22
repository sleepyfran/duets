use std::marker::PhantomData;

use common::builders::{Assignable, Assigned, Unassigned};
use engine::entities::{Character, City};

use crate::serializables::GameState;

/// Builder that allows to create new games containing the bare bones data needed to start playing.
#[derive(Default)]
pub struct GameBuilder<CharacterPresent, CityPresent>
where
    CharacterPresent: Assignable,
    CityPresent: Assignable,
{
    character_present_assigned: PhantomData<CharacterPresent>,
    city_present_assigned: PhantomData<CityPresent>,

    pub character: Option<Character>,
    pub starting_city: Option<City>,
    pub starting_year: i16,
}

pub type GameBuilderCompleted = GameBuilder<Assigned, Assigned>;

impl GameBuilder<Unassigned, Unassigned> {
    /// Creates a new game with no field specified. Character and Starting City need to be set
    /// in order for the create function to be available.
    pub fn new() -> GameBuilder<Unassigned, Unassigned> {
        GameBuilder {
            character_present_assigned: PhantomData {},
            city_present_assigned: PhantomData {},
            character: None,
            starting_city: None,
            starting_year: 1995,
        }
    }
}

impl<CharacterPresent, CityPresent> GameBuilder<CharacterPresent, CityPresent>
where
    CharacterPresent: Assignable,
    CityPresent: Assignable,
{
    /// Includes the character in the builder.
    pub fn with_character(self, character: Character) -> GameBuilder<Assigned, CityPresent> {
        GameBuilder {
            character_present_assigned: PhantomData {},
            city_present_assigned: PhantomData {},
            character: Some(character),
            starting_city: self.starting_city,
            starting_year: self.starting_year,
        }
    }

    /// Includes the starting city in the builder.
    pub fn with_starting_city(
        self,
        starting_city: City,
    ) -> GameBuilder<CharacterPresent, Assigned> {
        GameBuilder {
            character_present_assigned: PhantomData {},
            city_present_assigned: PhantomData {},
            character: self.character,
            starting_city: Some(starting_city),
            starting_year: self.starting_year,
        }
    }

    /// Includes the starting year in the builder.
    pub fn with_starting_year(
        self,
        starting_year: i16,
    ) -> GameBuilder<CharacterPresent, CityPresent> {
        GameBuilder {
            character_present_assigned: PhantomData {},
            city_present_assigned: PhantomData {},
            character: self.character,
            starting_city: self.starting_city,
            starting_year: starting_year,
        }
    }
}

impl GameBuilder<Assigned, Assigned> {
    /// Creates an initial savegame with the fields assigned from the initial wizard and the rest
    /// set to the default option.
    pub fn create(self) -> GameState {
        GameState {
            character: self.character.unwrap(),
            current_city: self.starting_city.unwrap(),
        }
    }
}
