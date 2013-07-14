using System;
using System.Reflection;
using System.Resources;

// this is used to version artifacts. AssemblyInformationalVersion should use semantic versioning (http://semver.org/)
[assembly: AssemblyInformationalVersion("2.0.0-beta")]
[assembly: AssemblyVersion("2.0.0.0")]

[assembly: AssemblyCompany("Kent Boogaart")]
[assembly: AssemblyProduct("The Helper Trinity")]
[assembly: AssemblyCopyright("© Copyright. Kent Boogaart.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif