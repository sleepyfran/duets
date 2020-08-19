use serde::{Deserialize, Serialize};

use engine::entities::{Instrument, Object, ObjectType};

#[derive(Deserialize, Serialize)]
#[serde(remote = "ObjectType")]
pub enum ObjectTypeDef {
    #[serde(with = "super::InstrumentDef")]
    Instrument(Instrument),
    Computer,
}

#[derive(Deserialize, Serialize)]
#[serde(remote = "Object")]
#[serde(rename_all = "camelCase")]
pub struct ObjectDef {
    pub id: String,
    pub name: String,
    pub description: String,
    #[serde(with = "ObjectTypeDef")]
    pub r#type: ObjectType,
}
