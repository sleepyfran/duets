/**
 * This script generates the database specified in database.json, which contains
 * references and other non deserializable fields, into a readable JSON file.
 */
"use strict";

const fs = require("fs");

console.log("🗃 Opening & parsing file...");
const databasePath = `${__dirname}/../database.json`;
const jsonFileContent = fs.readFileSync(databasePath);
const database = JSON.parse(jsonFileContent);

console.log("🔍 Resolving references...");
const result = resolveReferencesOf(database);

const savePath = `${__dirname}/../generated/database.json`;
fs.writeFileSync(savePath, JSON.stringify(result));
console.log("✅ Done, saved in ", savePath);

function resolveReferencesOf(data) {
  if (isObject(data)) {
    const keys = Object.keys(data);
    return keys.reduce(
      (acc, field) => ({ ...acc, [field]: resolveReferencesOf(data[field]) }),
      {}
    );
  } else if (isArray(data)) {
    return data.map(resolveReferencesOf);
  }

  return resolveReferenceOfField(data);
}

function resolveReferenceOfField(field) {
  if (!isReference(field)) return field;

  const reference = field.slice(1);
  const referenceTokens = reference.split("_");
  const referenceType = referenceTokens[0];
  const referenceId = referenceTokens[1];

  if (!referenceType || !referenceId) {
    throw `Unable to parse reference ${field}`;
  }

  const databaseField = database[referenceType].find(
    (elem) => elem.id == referenceId
  );

  if (!databaseField) {
    throw `Could not find a valid reference of type ${referenceType} with the given ID ${referenceId} in reference ${field}`;
  }

  return resolveReferencesOf(databaseField);
}

function isReference(val) {
  return typeof val == "string" && val.startsWith("$");
}

function isObject(val) {
  return val != null && typeof val === "object" && !isArray(val);
}

function isArray(val) {
  return val != null && Array.isArray(val);
}
