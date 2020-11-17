use common::entities::Identity;

use crate::constants;
use crate::context::Context;
use crate::world::interactions::*;

#[derive(Clone, Default)]
pub struct BrowseWebInteraction;

impl Identity for BrowseWebInteraction {
    fn id(&self) -> String {
        "browse_web".into()
    }
}

impl Interaction for BrowseWebInteraction {
    fn name(&self) -> String {
        "Browse Web".into()
    }

    fn description(&self) -> String {
        "Not the best way of passing your time, but it'll make you happier".into()
    }

    fn requirements(&self) -> Vec<Requirement> {
        vec![Requirement::HealthAbove(20), Requirement::EnergyAbove(20)]
    }

    fn track_action(&self) -> bool {
        false
    }

    fn limit_daily_interactions(&self) -> InteractionTimes {
        InteractionTimes::Unlimited
    }

    fn effects(&self, _: &Context) -> InteractionEffects {
        InteractionEffects {
            always_applied: vec![
                InteractionEffect::Energy(EffectType::Negative(
                    constants::effects::negative::ENERGY_NORMAL_INTERACTION,
                )),
                InteractionEffect::Mood(EffectType::Positive(
                    constants::effects::positive::BROWSE_WEB_INTERACTION,
                )),
                InteractionEffect::Time(TimeConsumption::TimeUnit(1)),
            ],
            applied_after_interaction: vec![],
        }
    }

    fn sequence(&self, _: &Context) -> InteractSequence {
        Ok(vec![])
    }
}
