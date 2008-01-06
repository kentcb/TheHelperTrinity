using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("The Helper Trinity Unit Tests")]
[assembly: AssemblyDescription("Unit tests for The Helper Trinity.")]
[assembly: AssemblyCompany("Kent Boogaart")]
[assembly: AssemblyProduct("The Helper Trinity")]
[assembly: AssemblyCopyright("© Copyright. Kent Boogaart.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]


#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif