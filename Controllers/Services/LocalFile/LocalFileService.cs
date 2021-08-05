using System.IO;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services
{
    public class LocalFileService
    {
        // Read a local file from the project.
        public static Task<string> ReadLocalFile(string filePath)
        {
            return File.ReadAllTextAsync(filePath);
        }
    }
}