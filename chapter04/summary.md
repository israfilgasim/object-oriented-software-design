### Part 1: The Monolith (`AutomobileApp`)

This initial phase establishes the baseline functionality, but intentionally introduces a common architectural flaw known as the "God Class."

**Design Note: High Coupling and Low Cohesion**
The immediate design issue here is that the class is doing too much. The `AutomobileApp` mixes the domain logic of the vehicle (Engine, Heading, Speed) with the logic of external services (Soap, Wax, Vacuum).

* **The Problem:** A change in how a "Car Wash" works shouldn't require you to recompile the "Car" code. This class violates the Single Responsibility Principle.

**Language-Based Implementation**

* **In C++:** The code uses `typedef` to alias standard integers. It relies on a header file (`.h`) to define the class structure separately from implementation.
* **In C#:** The translation replaces `typedef` with standard Strong Types or `using` aliases. Arrays are objects that must be explicitly allocated with `new`, unlike the fixed memory block in C++.
* **In Go:** The concept of a "Class" is replaced by a `struct`. Visibility is not controlled by keywords like `public`, but by Capitalization (Uppercase is exported/public, lowercase is private).

---

### Part 2: Separation of Concerns (`Garage` & `Automobile`)

This step refactors the monolithic class into interacting objects. The "Entity" (Car) is separated from the "Services" (Garage, Car Wash).

**Design Note: Association and Dependency**
This introduces the concept of objects operating *on* other objects. The `Garage` is not a Car; it *uses* a Car. This is a "Has-A" or "Uses-A" relationship.

* **The Improvement:** This achieves Separation of Concerns. The `Automobile` class now only manages its own state (speed, direction), while the `Garage` manages the inventory (tires, oil) and performs actions upon the car.

**Language-Based Implementation**

* **In C++:** The design explicitly uses pointers (`Automobile *c`). This is crucial because if you passed the car by value, the garage would repair a *copy* of the car, leaving the original broken.
* **In C#:** Objects are Reference Types by default. Passing an `Automobile` object to a method behaves similarly to a pointer—changes made inside the Garage reflect on the original object automatically.
* **In Go:** You must be explicit about pointers (`*Automobile`). If you omit the asterisk, Go passes a copy of the struct value, meaning the repairs wouldn't "stick" to the original car.

---

### Part 3: The Template Method (`MotorVehicle` Base Class)

This step introduces Inheritance and the centralization of logic. A generic parent class defines a workflow that children must follow.

**Design Note: The Template Method Pattern**
The `MotorVehicle` class defines a `drive()` method that calls a specific sequence of functions (`start`, `accelerate`, `turn`). This dictates the *order* of operations, while the subclasses (`Car`, `Truck`) dictate the *implementation* of those operations.

* **The Concept:** This leverages Polymorphism. The `drive()` function code lives in the parent, but at runtime, it executes the child's specific `start_engine()` logic.

**Language-Based Implementation**

* **In C++:** This relies on `virtual` functions. A `pure virtual` function (`= 0`) forces the child to provide an implementation.
* **In C#:** This maps directly to `abstract class` and the `override` keyword. Unlike C++, C# is stricter about visibility; a parent usually cannot access `private` methods of a child, so we often use `protected`.
* **In Go:** This requires a paradigm shift. Go has no inheritance. To achieve this, Go uses **Embedding** (putting a `BaseVehicle` struct inside a `Car` struct) to share code, and **Interfaces** to handle the polymorphism.

---

### Part 4: The Pure Contract (`MotorVehicleInterface`)

This final step removes all shared implementation logic, leaving only a contract of behavior.

**Design Note: Interface-Based Programming**
Here, we remove the "Base Class" that holds data. We are no longer saying "A Car *is* a type of MotorVehicle with shared code." We are saying "A Car *agrees* to behave like a MotorVehicle."

* **The Flexibility:** This is the most decoupled design. It allows completely unrelated objects to be treated the same way, as long as they satisfy the method signatures.

**Language-Based Implementation**

* **In C++:** An interface is simply a class containing only pure virtual functions and no data members.
* **In C#:** The `interface` keyword is used. A distinct difference here is that interface members in C# must be `public` in the implementing class.
* **In Go:** This uses **Implicit Interfaces** (Duck Typing). You do not declare that `Truck` implements `MotorVehicle`. If the `Truck` struct happens to have the right methods, Go automatically treats it as a `MotorVehicle`.
