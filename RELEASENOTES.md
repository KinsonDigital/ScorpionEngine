# **Scorpion Engine Release Notes**

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.8.0</span>

### **New** 🎉

1. Added new enumeration named `TextureType` to represent either whole/entire textures or sub textures that are contained inside of a texture atlas
2. Added new `EntityFactory` that can be used to generate the following types of entities:
   * Animated entities from a texture atlas
   * Non animated entities from a entire single texture
   * Non animated entities from a texture atlas
3. Added new type called `Animator` with abstractions to be able to manage animation of an entity
   * Using the new `IAnimator` interface can be used to create custom animation management
4. Added new `IInitializable` interface
   * This can be used for enforcing the `Init()` method on custom classes
5. Added new type `RenderSection` that is used by the rendering system to render sub textures of a texture atlas
   * This represents the section of a texture that rendered  
6. Added new exception `InvalidTextureTypeException` that is thrown when trying to add a new texture to an entity that is not of one of the values described in the `TextureType` enumeration
7. Added new class `InputFactory` that can be used to generate `KeyboardWatcher` and `MouseWatcher` objects
8. Added new type abstract `GameInputWatcher` type that the `KeyboardWatcher` and `MouseWatcher` inherit from
9. Added new type `KeyboardMovementBehavior` that can be used to move entities based on keyboard input
10. Created new type `BehaviorFactory` type that can be used to make it easier to generate behaviors
11. `Entity` has now been abstracted away with the interface `IEntity`
12. `Renderer` has now been abstracted away with the interface `IRenderer`
13. `StopWatch` has now been abstracted away with the interface `IStopWatch`
14. `Counter` has now been abstracted away with the interface `ICounter`
15. Added ability for entities to be flipped horizontally and vertically.  The bool properties `FlippedHorizontally` and `FlippedVertically` can be set to true or false to achieve this
16. Added additional `Render()` overloads to the `Renderer` class to allow rendering an entity to the screen at a custom X and Y location

### **Breaking Changes** 💣

1. Removed `MoveFowardKeyboardBehavior` from library
2. Removed the exception classes `EntityAlreadyInitializedException` and `EntityNotInitializedException`
3. Removed classes `DynamicEntity`, `StaticEntity`, and `TextEntity`
4. Removed the following classes dealing with content.  Content loading and atlas management is now done with the <span style='font-weight: bold; color: dodgerblue'>Raptor</span> framework
   * `AtlasData` class
   * `AtlasDataLoader` class
   * `AtlasRepository` class
   * `AtlasSpriteData` class
   * `InValidReason` enum
5. Removed the enums `AnimationDirection` and `AnimationState`
6. Removed the class `ObjectAnimation`
7. Removed the class `KeyBehavior`
   * The functionality of this class is provided by the `KeyboardWatcher` class
8. Removed the class `Behavior`
9. Removed the class `LimitNumberBehavior`
10. Renamed interface `IDrawable` to `IDrawableObject`
11. Renamed class `GameRenderer` to `Renderer`
12. Renamed interface `IBehavior` to `IEntityBehavior`

### **Tech Debt/Cleanup** 🧹

1. Lots of code refactoring and cleanup performed across entire code base

### **Improvements** 🌟

1. Improved content loading system by improving performance by internally caching loaded textures that can be used

### **Nuget/Library Updates** 📦

1. <span style='font-weight: bold; color: dodgerblue'>Scorpion Engine Project:</span>
   * Changed from Microsoft.CodeAnalysis.FxCopAnalyzers **v3.3.0** Microsoft.CodeAnalysis.NetAnalyzers **v5.0.3**
     * Microsoft.CodeAnalysis.FxCopAnalyzers has been deprecated at **v3.3.0**
   * Added Newtonsoft.Json **v12.0.3**
   * Added SimpleInjector **v5.3.0**
   * Added coverlet.msbuild **v3.0.2**
   * Updated KinsonDigital.Raptor.Windows from **v0.8.0** to **v0.25.0**

1. <span style='font-weight: bold; color: dodgerblue'>Test Game Project:</span>
   * Added coverlet.msbuild **v3.0.3**
   * Added Newtonsoft.Json **v12.0.3**
   * Changed from Microsoft.CodeAnalysis.FxCopAnalyzers **v3.3.0** Microsoft.CodeAnalysis.NetAnalyzers **v5.0.3**
     * Microsoft.CodeAnalysis.FxCopAnalyzers has been deprecated at **v3.3.0**

1. <span style='font-weight: bold; color: dodgerblue'>Scorpion Engine Unit Test Project:</span>
   * Changed from Microsoft.CodeAnalysis.FxCopAnalyzers **v3.3.0** Microsoft.CodeAnalysis.NetAnalyzers **v5.0.3**
     * Microsoft.CodeAnalysis.FxCopAnalyzers has been deprecated at **v3.3.0**
   * Updated coverlet.msbuild from **v2.9.0 to **v3.0.3**
   * Updated Moq from **v4.14.5** to **v4.16.1**
   * Updated Microsoft.NET.Test.Sdk from **v16.7.0** to **v16.9.1**

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.7.0</span>

### **New** 🎉

1. Added rules to **editorconfig** files in solution to improve coding standards

### **Changes** ✨
1. Refactored code to fix **Disposable** pattern issues

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.6.0</span>

### **New** 🎉
1. Added/setup required **editorconfig** files with appropriate coding analyzer rules
2. Added **stylecop.json** files for the stylecop analyzer
3. Created PR templates for each branch that PR's flow into

### **Nuget/Library Updates** 📦

1. Added code analyzers to the solution to enforce coding standards and keep code clean
   * This required adding nuget packages to allow the analyzers to run
		1. Microsoft.CodeAnalysis.FxCopAnalyzers - **v3.3.0**
		2. StyleCop.Analyzers - **v1.1.118**


### **Tech Debt/Cleanup** 🧹

1. Refactored code to meet code analyzer requirements
   * This was a very large code refactor

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.5.0</span>

### **New** 🎉

1. Changed **MIT license** copyright from **Calvin Wilkinson** to **Kinson Digital**

### **Nuget/Library Updates** 📦

1. Updated the **Raptor** library from version **v0.7.0** to **v0.8.0**.

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.4.0</span>

### **New** 🎉

1. Updated the **Raptor** nuget package library to version **v0.7.0**.
   * This update have various breaking changes.  Refactoring of code base to accommodate these new changes were performed as well as disabling some code and unit tests to allow for a successful build to prepare for future changes.

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.3.1</span>

### **Tech Debt/Cleanup** 🧹

1. Cleanup/refactor **Target** in project file.
2. Remove old unused **YAML** pipeline file.

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.3.0</span>

### **New** 🎉

1. Added **contributing document** to the repo to help developers know how to contribute to the project.

### **Nuget/Library Updates** 📦

1. Updated **Raptor** nuget package from **v0.4.0** to **v0.5.0**

2. Added **MIT license** file to repo.

---

## <span style='color:mediumseagreen;font-weight:bold'>Version 0.2.1</span>

### **Changes** ✨

1. Set **solution/projects** to use C# **v8.0**