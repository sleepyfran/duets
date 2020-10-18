use common::entities::{ActionTrack, GameState, Instrument};
use common::results::InteractResult;

use crate::commands::skills::SkillCommands;
use crate::interactions::Result;
use crate::queries::character::SkillQueries;

pub struct PlayInteractionInput {
    pub interaction_id: String,
    pub instrument: Instrument,
    pub game_state: GameState,
}

/// Increases the ability of the character in the skill associated with the instrument they're playing
/// only if they haven't played more than twice today.
pub fn play(input: PlayInteractionInput) -> Result {
    let today = input.game_state.calendar.date;
    let plays_today = input
        .game_state
        .action_registry
        .get_from_date(&input.interaction_id, today);
    let updated_registry = input
        .game_state
        .action_registry
        .register(&input.interaction_id, &input.game_state.calendar);

    if plays_today.len() >= 1 {
        (InteractResult::Failure, input.game_state)
    } else {
        let skill = input.instrument.associated_skill;
        let instrument_skill = input.game_state.character.get_skill_with_level(skill);
        let modified_skill = instrument_skill.increase_by(1);
        if instrument_skill.level == modified_skill.level {
            (InteractResult::SkillCap, input.game_state)
        } else {
            (
                InteractResult::Success,
                // TODO: Proof of concept. Move all this to a higher abstraction.
                input
                    .game_state
                    .modify_character(|character| character.with_skill(modified_skill))
                    .modify_calendar(|calendar| calendar.increase_time_once())
                    .modify_action_registry(|_| updated_registry),
            )
        }
    }
}
