| README.md |
|:---|

<div align="center">

<img src="img/empty-licenses-licx.png" alt="EmptyLicensesLicx" width="100" />

</div>

<h1 align="center">EmptyLicensesLicx</h1>
<div align="center">

An easy approach to building apps that use third-party controls from companies such as Telerik, DevExpress, Infragistics, and others, without having to install these controls in every single build node, for the sake of compiling the `licenses.licx` file.

[![Latest version](https://img.shields.io/nuget/v/EmptyLicensesLicx.svg)](https://www.nuget.org/packages/EmptyLicensesLicx) [![NuGet Downloads](https://img.shields.io/nuget/dt/EmptyLicensesLicx.svg)](https://www.nuget.org/packages/EmptyLicensesLicx) [![Stack Overflow](https://img.shields.io/badge/stack%20overflow-licenses.licx-orange.svg)](http://stackoverflow.com/questions/tagged/licenses.licx)

</div>

## Give a Star! :star:

If you like or are using this project please give it a star. Thanks!

## Overview

When you are developing .NET applications that use third-party controls such as the ones that you can buy from [Telerik](https://www.telerik.com) or [DevExpress](https://www.devexpress.com) for example, a mysterious file called `licenses.licx` appears inside the `Properties` folder of your C# project (Or `My Project` folder if you're using VB .NET).

This means that the third-party control uses the [licensing model provided by the .NET Framework for licensing components and controls](https://docs.microsoft.com/en-us/previous-versions/fe8b1eh9(v=vs.140)).

This file is a transitional file generated (and modified) by Visual Studio that participates in license checking. In design mode, Visual Studio uses this file to make a note of every licensed control you use in your design. When you then build your application, Visual Studio reads this `licenses.licx` file and for every control mentioned there, loads the relevant assembly and runs the license code in that assembly to see if the assembly is properly licensed (that is, that the product to which it belongs has been properly installed on that machine). If everything checks out, Visual Studio embeds the license key into the executable. If it doesn't, you'll get weird error messages about the control not being licensed (my favorite is "`Could not transform licenses file 'licenses.licx' into a binary resource.`").

The `licenses.licx` is a file automatically added to your project (if you cannot see it there, click _Show All Files_). Visual Studio uses a program called `lc.exe` to compile the licenses into embedded resources in your application, and when things go wrong with the license compiling process, you might see error messages referencing this executable.

Here's an example of a line in a `licenses.licx` file:

```
DevExpress.XtraCharts.ChartControl, DevExpress.XtraCharts.v15.2.UI, Version=15.2.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
DevExpress.XtraMap.MapControl, DevExpress.XtraMap.v15.2, Version=15.2.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
Infragistics.Win.Misc.UltraButton, Infragistics2.Win.Misc.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb
Infragistics.Win.Misc.UltraGroupBox, Infragistics2.Win.Misc.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb
```

Each line contains a reference to a type that is contained in an assembly, in a comma delimited list format. The first value is the full name of the class, the second is the assembly that contains the class, and the other values are part of the assembly's identity.

## The Problem

There are many issues caused by having to compile this file. For example, when you upgrade a solution to the latest version of the third-party controls you use, you'll get compile errors until the file is updated manually by you, or until you manage to get Visual Studio to regenerate it.

But that's not the biggest issue with `licenses.licx`. The thing is that Visual Studio has a propensity of touching this file if you open the solution (that's "touching" as in changing the file date to the current date/time). This plays havoc with licensing, especially if you happen to open the solution on a machine that doesn't have the third-party controls installed. Suddenly your build machine will throw off these "cannot transform" messages and you're left wondering what went wrong. Another prevalent issue is when you have a team of developers working on a solution: Visual Studio will make changes to this file as they interact with the third-party controls during development.

## The Solution

The solution for the `licenses.licx` problem is to make sure you **always have an empty `licenses.licx` file in your project**. That does not mean to delete the file... That means ignoring the changes that Visual Studio makes to this file, and removing all of its contents, effectively making this file completely empty - but still keep the file there during build and ignore it in your source control system.

This means every developer in a team needs to know that **and** remember that before checking-in code to source control.

And that is the main reason I've created [EmptyLicensesLicx](https://augustoproiete.github.io/EmptyLicensesLicx).

After installing it via [NuGet](https://www.nuget.org/packages/EmptyLicensesLicx):

```powershell
PM> Install-Package EmptyLicensesLicx
```
or
```powershell
> dotnet add package EmptyLicensesLicx
``` 
a reference to the [EmptyLicensesLicx.targets file](https://github.com/augustoproiete/EmptyLicensesLicx/blob/master/src/build/EmptyLicensesLicx.targets) will be added to your project, which will hook into the build pipeline and to make sure that the `licenses.licx` file is always empty before the compiler tries to compile it.

This means you no longer will see the "_cannot transform_" errors in Visual Studio, or when using MSBuild in your continuous integration server. In fact, if this is the only reason you have been installing these third-party controls in your build servers, you no longer have to.

## Release History

Click on the [Releases](https://github.com/augustoproiete/EmptyLicensesLicx/releases) tab on GitHub.

---

_Copyright &copy; 2016-2020 C. Augusto Proiete & Contributors - Provided under the [MIT License](LICENSE)._
