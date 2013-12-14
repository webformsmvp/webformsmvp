using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyTitle("Web Forms MVP")]
[assembly: AssemblyDescription("")]

#if DEBUG
[assembly: InternalsVisibleTo("WebFormsMvp.UnitTests")]
#endif

[assembly: AllowPartiallyTrustedCallers]

[assembly: CLSCompliant(true)]