using System;
using System.Web.Compilation;

namespace WebFormsMvp.Web
{
    ///<summary>
    /// Represents a class that wraps the System.Web.Compilation.BuildManager class.
    ///</summary>
    public class BuildManagerWrapper : IBuildManager
    {
        ///<summary>
        /// Attemps to load a type by name based within the scope of the currently running ASP.NET application.
        ///</summary>
        ///<param name="typeName">The name of the type to resolve.</param>
        ///<param name="throwOnError">True if an exception should be thrown if the type cannot be found, otherwise false.</param>
        ///<returns>The type found, or null if not found and throwOnError is false.</returns>
        public Type GetType(string typeName, bool throwOnError)
        {
            return BuildManager.GetType(typeName, throwOnError);
        }
    }
}