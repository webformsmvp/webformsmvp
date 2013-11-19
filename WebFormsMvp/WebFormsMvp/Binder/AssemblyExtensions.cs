using System;
using System.Reflection;

namespace WebFormsMvp.Binder
{
    internal static class AssemblyExtensions
    {
        public static AssemblyName GetNameSafe(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            return new AssemblyName(assembly.FullName);
        }
    }
}