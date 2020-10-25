use common::entities::{Identity, Instrument};
use common::results::InteractResult;
use simulation::queries::character::SkillQueries;

use crate::constants;
use crate::context::Context;
use crate::world::interactions::*;

#[derive(Clone, Default)]
pub struct ComposeInteraction {
    pub instrument: Instrument,
}

impl Identity for ComposeInteraction {
    fn id(&self) -> String {
        "compose".into()
    }
}

impl Interaction for ComposeInteraction {
    fn name(&self) -> String {
        "Compose".into()
    }

    fn description(&self) -> String {
        "Composing will allow you to create song ideas or improved previous ideas that you can later record".into()
    }

    fn requirements(&self) -> Vec<Requirement> {
        vec![Requirement::HealthAbove(20), Requirement::MoodAbove(20)]
    }

    fn track_action(&self) -> bool {
        true
    }

    fn limit_daily_interactions(&self) -> InteractionTimes {
        InteractionTimes::Multiple(2)
    }

    fn effects(&self, context: &Context) -> InteractionEffects {
        let instrument_skill = context
            .game_state
            .character
            .get_skill_with_level(&self.instrument.associated_skill);

        // Round up to 1 if it's zero, otherwise the song won't improve.
        let instrument_skill_level = if instrument_skill.level == 0 {
            1
        } else {
            instrument_skill.level
        };

        InteractionEffects {
            always_applied: vec![
                InteractionEffect::Energy(EffectType::Negative(
                    constants::effects::negative::HEALTH_NORMAL_INTERACTION,
                )),
                InteractionEffect::Time(TimeConsumption::TimeUnit(2)),
            ],
            applied_after_interaction: vec![InteractionEffect::Song(EffectType::Positive(
                constants::effects::positive::SONG_COMPOSE_INTERACTION * instrument_skill_level,
            ))],
        }
    }

    fn sequence(&self, context: &Context) -> InteractSequence {
        let songs_in_progress = context.clone().game_state.character.songs_in_progress;
        if !songs_in_progress.is_empty() {
            Ok(InteractItem::Chain(
                Box::new(InteractItem::Options(
                    songs_in_progress
                        .iter()
                        .enumerate()
                        .map(|(index, song)| InteractOption {
                            id: index.to_string(),
                            text: song.name.clone(),
                        })
                        .collect(),
                )),
                Box::new(common_sequence(context)),
            ))
        } else {
            Ok(common_sequence(context))
        }
    }

    fn messages(&self, context: &Context) -> (String, String) {
        (
            format!(
                "You successfully played the {}, that improved your skills by {}",
                self.instrument.name,
                constants::effects::positive::SKILL_PLAY_INTERACTION
            ),
            "Well, at least you tried... Maybe you should take a little break before trying again"
                .into(),
        )
    }
}

fn common_sequence(context: &Context) -> InteractItem {
    InteractItem::End
}
