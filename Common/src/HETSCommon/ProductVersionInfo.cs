using System.Collections.Generic;

namespace HETSCommon
{
    public class ProductVersionInfo
    {
        public ProductVersionInfo()
        {
            ApplicationVersions = new List<ApplicationVersionInfo>();
            DatabaseVersions = new List<DatabaseVersionInfo>();
        }

        public List<ApplicationVersionInfo> ApplicationVersions { get; set; }
        public List<DatabaseVersionInfo> DatabaseVersions { get; set; }
    }
}
