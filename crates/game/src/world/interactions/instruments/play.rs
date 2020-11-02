use common::entities::{Identity, Instrument};

use crate::constants;
use crate::context::Context;
use crate::world::interactions::*;

#[derive(Clone, Default)]
pub struct PlayInteraction {
    pub instrument: Instrument,
}

impl Identity for PlayInteraction {
    fn id(&self) -> String {
        "play".into()
    }
}

impl Interaction for PlayInteraction {
    fn name(&self) -> String {
        "Play".into()
    }

    fn description(&self) -> String {
        "Playing the instrument will advance the time by one time unit and increase the skill moderately".into()
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
        InteractionEffects {
            always_applied: vec![
                InteractionEffect::Energy(EffectType::Negative(
                    constants::effects::negative::HEALTH_NORMAL_INTERACTION,
                )),
                InteractionEffect::Time(TimeConsumption::TimeUnit(1)),
            ],
            applied_after_interaction: vec![InteractionEffect::Skill(
                self.instrument.associated_skill.clone(),
                EffectType::Positive(constants::effects::positive::SKILL_PLAY_INTERACTION),
            )],
        }
    }

    fn sequence(&self, context: &Context) -> InteractSequence {
        Ok(InteractItem::End)
    }
}
