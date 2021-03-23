# **Scorpion Engine Release Notes**

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
		1. Microsoft.CodeAnalysis.FxCopAnalyzers - v3.3.0
		2. StyleCop.Analyzers - v1.1.118


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

1. Set **solution/projects** to use C# v8.0