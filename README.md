# EmptyLicensesLicx

> An easy approach to building apps that use third-party controls from companies such as Infragistics, DevExpress, and others, without having to install these controls in every single build node, for the sake of compiling the Properties\licenses.licx file.

[![Latest version](https://img.shields.io/nuget/v/EmptyLicensesLicx.svg)](https://www.nuget.org/packages/EmptyLicensesLicx)
[![License](http://img.shields.io/:license-MIT-blue.svg)](https://opensource.org/licenses/mit-license.php)


## Goals
- No compiled code or tools -> 100% transparency
- Trivially added/installed via a [NuGet package](https://www.nuget.org/packages/EmptyLicensesLicx)
- No format strings or settings to learn
- Single well-structured [.targets](https://github.com/caioproiete/EmptyLicensesLicx/blob/master/src/build/EmptyLicensesLicx.targets) file with plain MSBuild
- Easily modified/improved by just adjusting a [single .targets file](https://github.com/caioproiete/EmptyLicensesLicx/blob/master/src/build/EmptyLicensesLicx.targets)


## Overview

When you are developing .NET applications that use third-party controls such as the ones that you can buy from [DevExpress](https://www.devexpress.com) or [Infragistics](http://www.infragistics.com) for example, a mysterious file called `licenses.licx` appears inside the `Properties` folder of your project.

This means that the third-party control uses the (*cough* crappy *cough*) [licensing model provided by the .NET Framework for licensing components and controls](https://msdn.microsoft.com/en-us/library/fe8b1eh9.aspx).

This file is a transitional file generated (and modified) by Visual Studio that participates in license checking. In design mode, Visual Studio uses this file to make a note of every licensed control you use in your design. When you then build your application, Visual Studio read this `Properties\licenses.licx` file and for every control mentioned there, will load the relevant assembly and run the license code in that assembly to see if the assembly is properly licensed (that is, that the product to which it belongs has been properly installed on that machine). If everything checks out, Visual Studio embeds the license key into the executable. If it doesn't, you'll get weird error messages about the control not being licensed (my favorite is "`Could not transform licenses file 'licenses.licx' into a binary resource.`" to which I usually invoke the colorful language of my ancestors).

The `Properties\Licenses.licx` is actually a file in your solution (if you cannot see it there, click Show All Files). Visual Studio uses a program called `lc.exe` to compile the licenses into embedded resources in your application, and when things go wrong with the license compiling I've seen error messages that reference this executable as well.

Here's an example of a line in a `licenses.licx` file:

```
Infragistics.Win.Misc.UltraButton, Infragistics2.Win.Misc.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb
Infragistics.Win.Misc.UltraGroupBox, Infragistics2.Win.Misc.v8.1, Version=8.1.20081.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb
DevExpress.XtraCharts.ChartControl, DevExpress.XtraCharts.v15.2.UI, Version=15.2.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
DevExpress.XtraMap.MapControl, DevExpress.XtraMap.v15.2, Version=15.2.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
```

Each line contains a reference to a type that is contained in an assembly, in a comma delimited list format. The first value is the name of the class, the second is the assembly that contains the class, and the other values are part of the assembly's identity.


## The Problem

There are many issues caused by having to compile this file. For example, when you upgrade a solution to the latest version of the third-party controls you use, you'll get compile errors until the file is updated manually by you, or until you manage to get Visual Studio to regenerate it.

But that's not the biggest issue with `licenses.licx`. The thing is that Visual Studio has a propensity of touching this file if you open the solution (that's "touching" as in changing the file date to the current date/time). This plays havoc with licensing, especially if you happen to open the solution on a non-licensed machine and you are using source control. Suddenly your build machine will throw off these "cannot transform" messages and you're left wondering what went wrong. Another prevalent issue is when you have a team of developers working on a solution: they're all unconsciously "modifying" this file.


## The Solution

The solution for the `licenses.licx` problem is to make sure you **always have an empty `licenses.licx` file in your project**. That does not mean to delete the file... That means ignoring whatever garbage Visual Studio will put inside this file, and removing all of its contents, effectively making this file completely empty - but still keep the file there and commit it to source control.

This means every developer in a team needs to know that **and** remember that before checking-in code in source control.

And that is the main reason I've created [EmptyLicensesLicx](http://caioproiete.github.io/EmptyLicensesLicx).

After installing it via [NuGet](https://www.nuget.org/packages/EmptyLicensesLicx):

    PM> Install-Package EmptyLicensesLicx

a reference to the [EmptyLicensesLicx.targets file](https://github.com/caioproiete/EmptyLicensesLicx/blob/master/src/build/EmptyLicensesLicx.targets) will be added to your project, which will hook into the build pipeline and will make sure that the `Properties\licenses.licx` file is always empty before the compiler tries to compile it.

This means you no longer will see the "cannot transform" errors in Visual Studio, or when using MSBuild in your continuous integration server. In fact, if this is the only reason you have been installing these third-party controls in your build servers, you no longer have to.

Problem solved. Developers 1 x 0 *Crappy* .NET framework licensing model.
