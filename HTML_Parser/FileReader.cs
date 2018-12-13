using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
    class FileReader
    {
        public async Task<List<string>> GetLinesAsync(string path)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = null;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

    }
}
