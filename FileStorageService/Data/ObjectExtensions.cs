using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileStorageService.Data
{
    public static class ObjectExtensions
    {
        public static string AsJson(this object value)
        {
            return JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
