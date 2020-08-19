use engine::entities::{Object, ObjectType};

use crate::shared::lang;

fn create_object_with_name(name: String) -> Object {
    Object {
        id: "".into(),
        name,
        description: "".into(),
        r#type: ObjectType::Computer,
    }
}

#[test]
fn describe_of_empty_list_returns_empty_string() {
    let empty = lang::list::describe(&vec![]);
    assert_eq!(empty, "");
}

#[test]
fn describe_of_one_item_list_returns_name_of_item_with_definer() {
    let object_with_a = lang::list::describe(&vec![create_object_with_name("pear".into())]);
    assert_eq!(object_with_a, "a pear");

    let object_with_an = lang::list::describe(&vec![create_object_with_name("apple".into())]);
    assert_eq!(object_with_an, "an apple");
}

#[test]
fn describe_of_two_items_list_returns_name_of_items_separated_by_and_with_definer() {
    let two_objects = lang::list::describe(&vec![
        create_object_with_name("banana".into()),
        create_object_with_name("apple".into()),
    ]);
    assert_eq!(two_objects, "a banana and an apple");
}

#[test]
fn describe_of_three_or_more_items_list_returns_name_of_items_separated_by_commas_and_and_with_definer(
) {
    let three_objects = lang::list::describe(&vec![
        create_object_with_name("pear".into()),
        create_object_with_name("banana".into()),
        create_object_with_name("apple".into()),
    ]);
    assert_eq!(three_objects, "a pear, a banana and an apple");

    let four_objects = lang::list::describe(&vec![
        create_object_with_name("pear".into()),
        create_object_with_name("banana".into()),
        create_object_with_name("apple".into()),
        create_object_with_name("orange".into()),
    ]);
    assert_eq!(four_objects, "a pear, a banana, an apple and an orange");
}
