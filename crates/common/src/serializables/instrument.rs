use serde::ser::SerializeSeq;
use serde::{Deserialize, Deserializer, Serialize, Serializer};

use engine::entities::Instrument;

#[derive(Deserialize, Serialize)]
#[serde(remote = "Instrument")]
#[serde(rename_all = "camelCase")]
pub struct InstrumentDef {
    pub name: String,
    pub allows_another_instrument: bool,
}

pub fn deserialize<'de, D>(deserializer: D) -> Result<Vec<Instrument>, D::Error>
where
    D: Deserializer<'de>,
{
    #[derive(Deserialize)]
    struct Wrapper(#[serde(with = "super::InstrumentDef")] Instrument);

    let v = Vec::deserialize(deserializer)?;
    Ok(v.into_iter().map(|Wrapper(a)| a).collect())
}

pub fn serialize<S>(value: &Vec<Instrument>, serializer: S) -> Result<S::Ok, S::Error>
where
    S: Serializer,
{
    #[derive(Serialize)]
    struct Wrapper<'s>(#[serde(with = "super::InstrumentDef")] &'s Instrument);

    let mut seq = serializer.serialize_seq(Some(value.len()))?;
    for e in value {
        seq.serialize_element(&Wrapper(e))?;
    }
    seq.end()
}
