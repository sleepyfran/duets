/// Defines an instrument that the user can play. If allows_another_instrument is true then it means
/// that it can be combined with another instrument, for example vocals + guitar.
#[derive(Clone)]
pub struct Instrument {
    pub name: String,
    pub allows_another_instrument: bool,
}
