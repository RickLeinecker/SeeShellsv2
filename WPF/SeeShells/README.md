# SeeShells WPF Application 
This is the main portion of the SeeShells project containing the desktop application.

## Project Setup

### Required Software
- [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) Community or newer edition

### Opening the Project
1. Using Visual Studio, open the `SeeShells.sln` Project File in the root of the WPF directory 
2. Use Visual Studio to build and run the project.

_Troubleshooting note: If you receive the following error when first trying to build the project:_
```
A numeric comparison was attempted on "$(MsBuildMajorVersion)" that evaluates to "" instead of a number, in condition "($(MsBuildMajorVersion) < 16)".
```
_Restart Visual Studio and build the project again. SeeShells uses [Fody Costura](https://github.com/Fody/Costura) package, which modifies the compilation process for the project._  

---

### Usage Documentaiton 
To see more a detailed description about the SeeShells program and using it, see the [documentation](https://rickleinecker.github.io/SeeShells/help).