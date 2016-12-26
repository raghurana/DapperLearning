using System;
using System.IO;

namespace DapperLearning.ConsoleApp.Utils
{
    /// <summary>
    ///     An interface that defines the contract between all resource readers.
    /// </summary>
    public interface IResourceReader
    {
        #region Methods

        /// <summary>
        ///     Gets the resource contents by reading it.
        /// </summary>
        /// <param name="resourceFolder">The resource folder.Separate folder heirarchy by a . </param>
        /// <param name="resourceName">Name of the resource file.</param>
        /// <returns>the contents of the resource.</returns>
        T GetResourceContents<T>(string resourceFolder, string resourceName);

        #endregion
    }

    /// <summary>
    ///     A Class which reads the resource as a string.
    /// </summary>
    public class AssemblyResourceReader : IResourceReader
    {
        #region Fields

        private readonly Type assemblyType;

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssemblyResourceReader" /> class.
        /// </summary>
        /// <param name="assemblyType">Type of the assembly in which the resource resides.</param>
        public AssemblyResourceReader(Type assemblyType)
        {
            this.assemblyType = assemblyType;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets the resource contents by reading it.
        /// </summary>
        /// <param name="resourceFolder">The resource folder.Separate folder heirarchy with . </param>
        /// <param name="resourceName">Name of the resource file.</param>
        /// <returns>the contents of the resource as a string. Callers can cast the return value to a string safely.</returns>
        /// <exception cref="ArgumentException">Invalid format of the resource folder.</exception>
        public T GetResourceContents<T>(string resourceFolder, string resourceName)
        {
            return ReadContents<T>(string.Format("{0}.{1}.{2}", assemblyType.Namespace, resourceFolder, resourceName));
        }

        #endregion

        #region Private Methods

        private T ReadContents<T>(String resourceNamespace)
        {
            var resourceStream = assemblyType.Assembly.GetManifestResourceStream(resourceNamespace);

            if (resourceStream == null)
                throw new ArgumentException(
                    string.Format(
                        "Could not locate resource {0}. Make sure resource's 'Build Action' is marked as Embedded-Resource at Compile time.",
                        resourceNamespace));

            using (TextReader textReader = new StreamReader(resourceStream))
            {
                var contents = Convert.ChangeType(textReader.ReadToEnd(), typeof(T));

                if (contents == null)
                    throw new NullReferenceException(
                        string.Format("Could not convert contents of resource {0} to type {1}", resourceNamespace,
                            typeof(T)));

                return (T)contents;
            }
        }

        #endregion
    }
}
