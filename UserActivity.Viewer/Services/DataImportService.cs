using Microsoft.Win32;
using System.Collections.Generic;
using UserActivity.CL.WPF.Entities;
using UserActivity.CL.WPF.Services;

namespace UserActivity.Viewer.Services
{
    /// <summary>
    /// Data import service.
    /// </summary>
    public class DataImportService
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public static DataImportService Create() => new DataImportService();

        /// <summary>
        /// Import user activity data from file.
        /// </summary>
        public List<SessionGroup> ImportFile()
        {
            var groups = new List<SessionGroup>();

            var openFileDialog = new OpenFileDialog()
            {
                Filter = string.Format("UAD-файлы (*.{0})|*.{0}|RDF-файлы (*.{1})|*.{1}",
                    XmlUserActivityDataContext.UadFileExtension, RDFUserActivityDataContext.RdfFileExtension),
                Multiselect = true,
            };
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                foreach (var stream in openFileDialog.OpenFiles())
                {
                    using (stream)
                    {
                        var sessionGroup = RDFUserActivityDataContext.LoadSessionGroup(stream);
                        groups.Add(sessionGroup);
                    }
                }
            }

            return groups;
        }
    }
}