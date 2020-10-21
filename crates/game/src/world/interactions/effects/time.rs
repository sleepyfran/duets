use crate::context::Context;
use crate::world::interactions::TimeConsumption;

/// Applies the time consumption to the given context.
pub fn apply(consumption: TimeConsumption, context: &Context) -> Context {
    context.clone().modify_game_state(|game_state| {
        game_state.modify_calendar(|calendar| match consumption {
            TimeConsumption::None => calendar,
            TimeConsumption::TimeUnit(times) => calendar.increase_time(times),
            TimeConsumption::Days(days) => calendar.increase_days(days),
        })
    })
}
