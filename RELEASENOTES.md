# **Scorpion Engine Release Notes**

## **Version 0.6.0**

### **Additions**

1. Added code analyzers to the solution to enforce coding standards and keep code clean
   * This required adding nuget packages to allow the analyzers to run
		1. Microsoft.CodeAnalysis.FxCopAnalyzers - v3.3.0
		2. StyleCop.Analyzers - v1.1.118
   * Added/setup required **editorconfig** files with appropriate coding analyzer rules
   * Added **stylecop.json** files for the stylecop analyzer
2. Refactored code to meet code analyzer requirements
   * This was a very large code refactor
3. Created PR templates for each branch that PR's flow into

---

## **Version 0.5.0**

### **Changes**

1. Updated the **Raptor** library from version **v0.7.0** to **v0.8.0**.

### **Misc**

1. Changed **MIT license** copyright from **Calvin Wilkinson** to **Kinson Digital**

---

## **Version 0.4.0**

### **New**

1. Updated the **Raptor** nuget package library to version **v0.7.0**.
   * This update have various breaking changes.  Refactoring of code base to accommodate these new changes were performed as well as disabling some code and unit tests to allow for a successful build to prepare for future changes.

---

## **Version 0.3.1**

### **Developer Related Items**

1. Cleanup/refactor **Target** in project file.
2. Remove old unused **YAML** pipeline file.

---

## **Version 0.3.0**

### **Developer Related Items**

1. Updated **Raptor** nuget package from **v0.4.0** to **v0.5.0**
2. Added **contributing document** to the repo to help developers know how to contribute to the project.
3. Added **MIT license** file to repo.

---

## **Version 0.2.1**

### **Changes**

1. Set **solution/projects** to use C# v8.0