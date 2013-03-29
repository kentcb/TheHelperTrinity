using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// this is used to version artifacts. AssemblyInformationalVersion should use semantic versioning (http://semver.org/)
[assembly: AssemblyInformationalVersion("1.5.1")]
[assembly: AssemblyVersion("1.5.1.0")]

[assembly: AssemblyCompany("Kent Boogaart")]
[assembly: AssemblyProduct("The Helper Trinity")]
[assembly: AssemblyCopyright("© Copyright. Kent Boogaart.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif