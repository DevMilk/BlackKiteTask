using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlackKiteTask.Common
{
    public class Utils
    {
        
        public static void ExportAsJson<T>(string filePath, T content)
        {
            //UnsafeRelaxedJsonEscaping allows "+", "-" characters in file for CyberRating Grades
            var serializerOption = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };
            File.WriteAllText($"{filePath}.json", JsonSerializer.Serialize(content, serializerOption));
        }
    }
}
