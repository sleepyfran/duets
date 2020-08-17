use engine::entities::Object;
use in_definite;

/// Transforms a list of objets into a description of the list handling the use of a/an, commans
/// and 'and' in the last element. Example:
/// ['guitar', 'fake guitar'] -> "a guitar and a fake guitar"
pub fn describe(objects: &Vec<Object>) -> String {
    let mut list_description = String::default();

    for (index, object) in objects.iter().enumerate() {
        let separator = if index == 0 {
            ""
        } else if index == objects.len() - 1 {
            " and "
        } else {
            ", "
        };

        list_description.push_str(&format!(
            "{}{} {}",
            separator,
            in_definite::get_a_or_an(&object.name),
            object.name
        ));
    }

    list_description
}
