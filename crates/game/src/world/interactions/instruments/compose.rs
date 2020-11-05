use std::collections::HashSet;
use strum::IntoEnumIterator;

use common::entities::{Identity, Instrument, Song, VocalStyle};
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
        vec![
            Requirement::HealthAbove(20),
            Requirement::MoodAbove(20),
            Requirement::EnergyAbove(20),
        ]
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

        let skill_level_multiplier = instrument_skill.level / 10;

        // Round up to 1 if it's zero, otherwise the song won't improve.
        let instrument_skill_level = if skill_level_multiplier == 0 {
            1
        } else {
            skill_level_multiplier
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
            Ok(build_existing_song_sequence(songs_in_progress, context))
        } else {
            Ok(build_new_song_sequence(context))
        }
    }
}

fn build_existing_song_sequence(songs_in_progress: HashSet<Song>, context: &Context) -> Sequence {
    vec![InteractItem::Confirmation {
        question: "You have unfinished songs, do you want to continue one of them?".into(),
        branches: (
            vec![InteractItem::Options {
                question: "Which song do you want to improve?".into(),
                options: songs_in_progress
                    .iter()
                    .map(|song| InteractOption {
                        id: song.id.to_string(),
                        text: song.name.clone(),
                    })
                    .collect(),
            }],
            build_new_song_sequence(context),
        ),
    }]
}

fn build_new_song_sequence(context: &Context) -> Sequence {
    vec![
        InteractItem::TextInput("Composing a new song. What name is it going to have?".into()),
        InteractItem::Options {
            question: "What genre is it going to have?".into(),
            options: context
                .database
                .genres
                .iter()
                .map(|genre| InteractOption {
                    id: genre.id.to_string(),
                    text: genre.name.to_string(),
                })
                .collect(),
        },
        InteractItem::Options {
            question: "What vocal style is the song going to have?".into(),
            options: VocalStyle::iter()
                .map(|style| InteractOption {
                    id: style.to_string(),
                    text: style.to_string(),
                })
                .collect(),
        },
    ]
}
