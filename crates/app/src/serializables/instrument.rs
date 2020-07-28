use serde::Deserialize;

use engine::entities::Instrument;

#[derive(Deserialize)]
#[serde(remote = "Instrument")]
#[serde(rename_all = "camelCase")]
pub struct InstrumentDef {
    pub name: String,
    pub allows_another_instrument: bool,
}
