These are examples of the *Open-Closed*  *Principle* . Once we’ve decided that class `Attributes` has captured the common member variables and functions for all attributes, we *closed* it for modification to provide code stability. We don’t expect to make more changes to the class. But we *opened* the class for extensions in the form of subclasses such as `FictionAttrs` and `CookbookAttrs` to provide the flexibility to add more kinds of book attributes. Class `Book` and its subclasses are another example of this design principle.

Therefore, `Book` contains a pointer to the superclass `Attributes`, rather than specifically to `FictionAttrs` or `CookbookAttrs`. This is an example of  *coding to the interface* , because superclass `Attributes` serves as the interface of its subclasses.

Indeed, what if the requirements change further, and the catalogue must store and search for other kinds of books, and each kind has unique attributes? We made our application more complex by attempting to handle requirement changes, namely new kinds of books and additional book attributes. Examples of the complexity include the following:

* Each kind of book requires a pair of `Book` and `Attributes` subclasses.
* Each kind of book requires an overloaded `add()` member function and an overloaded `find()` member function in class `Catalogue`. These functions have similar code.
* Each `find()` member function requires a `dynamic_cast<>()` call to ensure it will check the right kind of book, and another `dynamic_cast<>()` call to convert a pointer to the right kind of attributes. Otherwise, these member functions have similar code.

If our application needs to manage more kinds of books, we will have an increase of subclasses, duplicate code, and runtime type checks and conversions.

After the third iteration, we must admit that using subclasses to handle changes in the requirements for books and attributes was a poor design decision that won’t scale well if there are more kinds of books.
