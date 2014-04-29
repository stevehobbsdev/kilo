using System.Collections.Generic;
using System.Web.Optimization;

namespace Kilo.Mvc.Optimization
{
    /// <summary>
    /// Returns the files in the order that they're added to the bundle, rather than changing the order based on certain keywords.
    /// </summary>
    public class PassThruBundleOrderer : IBundleOrderer
    {
        /// <summary>
        /// Returns the files in the same order they're added to the bundle
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="files">The files.</param>
        /// <returns>A list of bundle files</returns>
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
