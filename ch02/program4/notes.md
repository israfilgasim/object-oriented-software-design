We no longer need `UNSPECIFIED` values. In our new design, if an attribute is missing in a search target, that attribute will be a don’t-care.

We’ve eliminated the duplicate code and now follow the Don’t Repeat Yourself (DRY) Principle. There is only one `add()` member function and only one `find()` member function. We no longer need calls to `dynamic_cast<>()` to do runtime type checks and conversions. A bonus of this design is a book can have any attributes, not limited to certain ones based on the kind of book, as shown in the following listing.

Only class `Attributes` will need modifications. Classes `Book` and `Catalogue` will not require modifications.
