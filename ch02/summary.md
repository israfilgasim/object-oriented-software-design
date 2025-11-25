### Step 1: The Foundation (Pointers & Basic Classes)

We started with a simple system: a `Catalogue` holding a list of `Book` objects.

* **The Concept:** Basic storage and string searching.
* **C++ Legacy:** Manual string parsing loops (`equal_ignore_case`) and `vector<Book*>`.
* **The Translation:**
  * **C#:** We replaced header/source files with Classes and Properties. We replaced manual char-loops with `String.Equals(..., StringComparison.OrdinalIgnoreCase)`.
  * **Go:** We replaced Classes with Structs and Constructors with Factory functions. We introduced the `Stringer` interface (`func (b Book) String() string`) to replace C++ operator overloading.

### Step 2: Composition & Enums (Adding "Attributes")

We introduced complexity by separating the data (`Attributes`) from the container (`Book`).

* **The Concept:**  **Composition** . A Book *has* Attributes. We also added search logic where "Empty" or "0" meant "Wildcard" (match anything).
* **The Translation:**
  * **C#:** We used standard `enum`. We used Composition naturally (`Book` holds a reference to `Attributes`).
  * **Go:** We handled Enums using `const`, `iota`, and manual string mapping arrays. We learned that Go prefers "Embedding" or struct references over deep inheritance hierarchies.

### Step 3: Inheritance & RTTI (Fiction vs. Cookbook)

We branched the logic. A `Book` could now be specifically a `Fiction` or a `Cookbook`, each with unique fields.

* **The Concept:** **Polymorphism** and  **Run-Time Type Information (RTTI)** .
* **C++ Legacy:** The use of `dynamic_cast` to check if a `Book*` was actually a `Fiction*`.
* **The Translation:**
  * **C#:** We used **Inheritance** (`class Fiction : Book`) and **Pattern Matching** (`if (book is Fiction f)`). This was very similar to the C++ OOP model.
  * **Go:** This was the biggest divergence. Since Go has  **no inheritance** , we used **Interfaces** and **Type Assertions** (`if fiction, ok := book.(*Fiction); ok`). This demonstrated Go's "Composition over Inheritance" philosophy.

### Step 4: Data-Driven Architecture (The Final Version)

We abandoned the rigid Class hierarchy (`Fiction` vs `Cookbook` classes) in favor of a flexible, generic Map.

* **The Concept:**  **Entity-Component / Data-Driven** . Instead of hard-coding fields in classes, we store data in a `Map<Key, Value>`.
* **The Change:**
  * **Flexibility:** We can now add a `Region` to a `Fiction` book without changing the source code (class definition).
  * **Unspecified Logic:** We removed the `UNSPECIFIED` enum. Now, if a book lacks a trait, we simply **omit the key** from the map.
* **The Translation:**
  * **C#:** Used `Dictionary<Key, object>`. We added a `Validate` helper to ensure type safety (e.g., preventing a String from being saved in a Year slot).
  * **Go:** Used `map[Key]interface{}`. We implemented validation logic inside the `NewAttributes` factory function to panic on type mismatches, ensuring the same safety as C++ `assert`.
