### **Iteration 1: Initial Cohesive Classes**

The development began with basic requirements: adding fiction books with title and author attributes and enabling case-insensitive searches with wildcards.

* **Design:** Two classes were created. The `Book` class was responsible for storing attributes, and the `Catalogue` class managed the list of books and search functionality.
* **Result:** This design adhered to the  **Single Responsibility Principle** , ensuring each class was cohesive with a single primary purpose.

**Language Flavors:**

* **C# Implementation:** Replaced C++ header/source separation with single `.cs` files. It utilized C# **Properties** (e.g., `public string Title { get; }`) instead of manual getters and employed `List<Book>` with automatic Garbage Collection, removing the need for manual memory management.
* **Go Implementation:** Utilized **Structs** instead of classes and replaced constructors with factory functions (e.g., `NewBook`). To handle output, it implemented the `Stringer` interface (`func (b Book) String() string`) rather than overloading operators.

---

### **Iteration 2: Encapsulation and Delegation**

New requirements added `year` and `genre` attributes to fiction books. Implementing this directly in the `Book` class would have forced changes in the `Catalogue` class, violating the principle of preventing changes from leaking out.

* **Design:** The attributes were moved into a separate `Attributes` class. The `Catalogue` class was updated to delegate attribute matching to this new class.
* **Result:** This applied the **Encapsulate What Varies Principle** and the  **Delegation Principle** , resulting in **loose coupling** where the `Catalogue` did not need to know the specific details of the attributes.

**Language Flavors:**

* **C# Implementation:**
  * **Enums:** Used strictly typed `enum Genre`, enabling `ToString()` to print "Adventure" automatically without manual switch statements.
  * **Composition:** The `Book` class held a reference to `Attributes`. Wildcard logic was streamlined using `string.IsNullOrEmpty` and `string.Equals(..., StringComparison.OrdinalIgnoreCase)`.
* **Go Implementation:**
  * **Enums:** Simulated enums using `const` and `iota`. This required a manual `String()` method (array lookup) to print "adventure" instead of the integer value.
  * **Composition:** The `Book` struct held a pointer `*Attributes`. Wildcard matching mimicked C++ logic by manually checking for empty strings.

---

### **Iteration 3: Subclasses (A Wrong Turn)**

The requirements expanded to include cookbooks, which required a unique `region` attribute.

* **Design:** The developers utilized inheritance, creating `Book` and `Attributes` superclasses with specific subclasses: `Fiction`/`Cookbook` and `FictionAttrs`/`CookbookAttrs`.
* **Result:** While this attempted to follow the **Open-Closed** and **Code to the Interface** principles, it created significant complexity. The design required overloaded functions, duplicate code, and runtime type checking, ultimately violating the  **Don’t Repeat Yourself (DRY) Principle** .

**Language Flavors:**

* **C# Implementation (Inheritance):** Mirrored the C++ design closely (`class Fiction : Book`). It replaced `dynamic_cast` with the modern `is` pattern matching operator (`if (book is Fiction f)`), which is cleaner syntactically but retains the architectural rigidity.
* **Go Implementation (Interfaces):**
  * **No Inheritance:** Since Go does not support inheritance, the team was forced to define a `Book` interface to achieve this design.
  * **Type Assertion:** `dynamic_cast` was replaced with **Type Assertion** (`fiction, ok := book.(*Fiction)`). This step felt "unnatural," highlighting Go's preference for composition over inheritance.

---

### **Iteration 4: Backtracking to a Data-Driven Design**

Recognizing the complexity of Iteration 3, the developers backtracked to find a better decision branch.

* **Design:** They removed the subclasses, determining that class `Book` alone was sufficient. They replaced the rigid class structure with a standard template library (STL) map containing key-value pairs (using `std::variant` to handle different data types like `int`, `string`, or `enum`).
* **Result:** This simplified the `Catalogue` class to use single `add()` and `find()` functions, eliminating duplicate code and runtime type conversions. This final design successfully encapsulated changes, allowing for new book types (like "how-to" books) and attributes without modifying the core classes.

**Language Flavors:**

* **C# Implementation (Dictionary):**
  * **Storage:** Used `Dictionary<Key, object>` to replace `std::map`.
  * **Validation:** Added a `Validate` helper function to enforce type safety (throwing `ArgumentException` on mismatch) because `object` is generic. It utilized elegant collection initializer syntax for readability.
* **Go Implementation (Map):**
  * **Storage:** Used `map[Key]interface{}`.
  * **Validation:** The `NewAttributes` factory function implemented a **Type Switch** (`switch v.(type)`) to ensure data integrity, `panic`-ing on type mismatches (similar to C++ `assert`).
  * **Logic:** As in C++, the "Unspecified" enum was removed; missing keys in the map were treated as wildcards.
