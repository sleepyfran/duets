/// Includes every possible result that can happen due to any interaction. Most of them will be
/// shared (Success, Failure) but there will also be some specifics for more specific actions.
#[derive(Clone)]
pub enum InteractResult {
    /// Generic state for when the interaction was successful.
    Success,
    /// Generic state for when the interaction was successful.
    Failure,
    /// Indicates that the skill associated with the interaction could not be further improved.
    SkillCap,
}
