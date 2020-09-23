pub trait OptionCloneExtensions<T>
where
    T: Clone,
{
    /// Unwraps whatever is inside the option cloning it.
    fn unwrap_cloned(self) -> T;
}

impl<T: Clone> OptionCloneExtensions<T> for Option<&T> {
    fn unwrap_cloned(self) -> T {
        self.cloned().unwrap()
    }
}
