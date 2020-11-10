/**
 * This script generates the database specified in database.json, which contains
 * references and other non deserializable fields, into a readable JSON file.
 */
"use strict";

const fs = require("fs");
const REF_TOKEN = "ref:";
const FILE_TOKEN = "file:";

console.log("🗃 Opening & parsing file...");
const databasePath = `${__dirname}/../def/database.json`;
const database = loadJsonContent(databasePath);

console.log("🔍 Resolving references...");
const joinedDatabase = resolveFilesOf(database);
const result = resolveReferencesOf(joinedDatabase);

const savePath = `${__dirname}/../generated/database.json`;
fs.writeFileSync(savePath, JSON.stringify(result));
console.log("✅ Done, saved in ", savePath);

function loadJsonContent(path) {
  const jsonFileContent = fs.readFileSync(path);
  return JSON.parse(jsonFileContent);
}

function resolveWith(data, resolver) {
  if (isObject(data)) {
    const keys = Object.keys(data);
    return keys.reduce(
      (acc, field) => ({ ...acc, [field]: resolveWith(data[field], resolver) }),
      {}
    );
  } else if (isArray(data)) {
    return data.map((item) => resolveWith(item, resolver));
  }

  return resolver(data);
}

function resolveFilesOf(data) {
  return resolveWith(data, resolveFileOf);
}

function resolveReferencesOf(data) {
  return resolveWith(data, resolveReferenceOf);
}

function resolveFileOf(field) {
  if (!isFile(field)) return field;

  const relativePath = field.replace(FILE_TOKEN, "");
  const path = `${__dirname}/../def/${relativePath}`;
  const jsonContent = loadJsonContent(path);

  return resolveFilesOf(jsonContent);
}

function resolveReferenceOf(field) {
  if (!isReference(field)) return field;

  const reference = field.replace(REF_TOKEN, "");
  const referenceTokens = reference.split("_");
  const referenceType = referenceTokens[0];
  const referenceId = referenceTokens[1];

  if (!referenceType || !referenceId) {
    throw `Unable to parse reference ${field}`;
  }

  const databaseField = joinedDatabase[referenceType].find(
    (elem) => elem.id == referenceId
  );

  if (!databaseField) {
    throw `Could not find a valid reference of type ${referenceType} with the given ID ${referenceId} in reference ${field}`;
  }

  return resolveReferencesOf(databaseField);
}

function isReference(val) {
  return typeof val == "string" && val.startsWith(REF_TOKEN);
}

function isFile(val) {
  return typeof val == "string" && val.startsWith(FILE_TOKEN);
}

function isObject(val) {
  return val != null && typeof val === "object" && !isArray(val);
}

function isArray(val) {
  return val != null && Array.isArray(val);
}
