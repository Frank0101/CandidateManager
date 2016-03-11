using CandidateManager.Core.DAL;
using System.Collections.Generic;
using System.IO;

namespace CandidateManager.Infra.DAL
{
    public class FileSystemRepository : IFileSystemRepository
    {
        public IEnumerable<string> GetFolderContent(string folderPath, string searchPattern = "*.*")
        {
            return Directory.GetFiles(folderPath, searchPattern);
        }
    }
}
