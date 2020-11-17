use crate::context::Context;
use crate::world::interactions::*;
use common::entities::Identity;

#[derive(Clone, Default)]
pub struct SleepInteraction;

impl Identity for SleepInteraction {
    fn id(&self) -> String {
        "sleep".into()
    }
}

impl Interaction for SleepInteraction {
    fn name(&self) -> String {
        "Sleep".into()
    }

    fn description(&self) -> String {
        "Sleep advances four time units and puts your energy back to where it should be".into()
    }

    fn requirements(&self) -> Vec<Requirement> {
        vec![]
    }

    fn track_action(&self) -> bool {
        false
    }

    fn limit_daily_interactions(&self) -> InteractionTimes {
        InteractionTimes::Unlimited
    }

    fn effects(&self, context: &Context) -> InteractionEffects {
        InteractionEffects {
            always_applied: vec![
                InteractionEffect::Energy(EffectType::Positive(100)),
                InteractionEffect::Time(TimeConsumption::TimeUnit(4)),
            ],
            applied_after_interaction: vec![],
        }
    }

    fn sequence(&self, context: &Context) -> InteractSequence {
        Ok(vec![])
    }
}
