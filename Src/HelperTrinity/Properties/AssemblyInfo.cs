using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("The Helper Trinity")]
[assembly: AssemblyDescription("A set of helper classes applicable to almost any .NET coding venture.")]
[assembly: AssemblyCompany("Kent Boogaart")]
[assembly: AssemblyProduct("The Helper Trinity")]
[assembly: AssemblyCopyright("© Copyright. Kent Boogaart.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: AssemblyVersion("1.3.0.0")]
[assembly: AssemblyFileVersion("1.3.0.0")]
#if !SILVERLIGHT
[assembly: System.Security.AllowPartiallyTrustedCallers]
#endif

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif