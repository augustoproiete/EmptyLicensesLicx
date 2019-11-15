EmptyLicensesLicx
-----------------
An easy approach to building apps that use third-party controls from companies
such as Telerik, DevExpress, Infragistics, and others, without having to
install these controls in every single build node, for the sake of compiling
the Properties\licenses.licx file.

A reference to the EmptyLicensesLicx.targets have been added to your project,
and is hooked into the build pipeline to make sure that the
Properties\licenses.licx file in this project is always empty before the
compiler tries to compile it.

i.e. Visual Studio will continue to modify the Properties\licenses.licx file as
you make changes to this project, but every time you compile this project, the
contents of the Properties\licenses.licx file will be removed and the file will
become empty, before the compiler runs.

Remember to ignore/exclude the Properties\licenses.licx file from your source
control system!
