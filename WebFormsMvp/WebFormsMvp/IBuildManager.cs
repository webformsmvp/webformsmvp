using System;

namespace WebFormsMvp
{
    ///<summary>
    /// Represents a class that can resolve types by name in the currently running ASP.NET application.
    ///</summary>
    public interface IBuildManager
    {
        ///<summary>
        /// Attemps to load a type by name based within the scope of the currently running ASP.NET application.
        ///</summary>
        ///<param name="typeName">The name of the type to resolve.</param>
        ///<param name="throwOnError">True if an exception should be thrown if the type cannot be found, otherwise false.</param>
        ///<returns>The type found, or null if not found and throwOnError is false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GetType",
            Justification = "Wrapping existing framework function name.")]
        Type GetType(string typeName, bool throwOnError);
    }
}