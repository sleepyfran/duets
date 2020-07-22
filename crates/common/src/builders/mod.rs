/// Defines the presence or absence of a value in a builder. We use this to be able to enforce the
/// addition of some parameters before allowing to build. Use with generics like:
///
/// struct ExampleBuilder<MandatoryFieldAssigned, AnotherMandatoryFieldAssigned>
/// where
///     MandatoryField: Assignable,
///     AnotherMandatoryField: Assignable
/// {
///     // These phanton data fields have to be declared.
///     mandatory_field_assigned: PhantomData<MandatoryFieldAssigned>
///     another_mandatory_field_assigned: PhantomData<AnotherMandatoryFieldAssigned>
/// }
///
/// Now these two generics can either be Assigned or Unassigned, which allow us to implement the
/// build function only for the all Assigned case:
///
/// impl ExampleBuilder<Assigned, Assigned> {
///     ...
/// }
pub struct Assigned;
pub struct Unassigned;

pub trait Assignable {}
pub trait ValueAssigned: Assignable {}
pub trait ValueUnassigned: Assignable {}

impl Assignable for Assigned {}
impl Assignable for Unassigned {}

impl ValueAssigned for Assigned {}

impl ValueUnassigned for Unassigned {}
