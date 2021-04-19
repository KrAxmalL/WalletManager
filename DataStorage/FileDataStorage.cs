using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStorage
{
    public class FileDataStorage<TObject> where TObject : class, IStorable
    {
        private static readonly string BaseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinanceWpfStorage", typeof(TObject).Name);

        public FileDataStorage()
        {
            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
            }
        }

        public async Task AddOrUpdateAsync(TObject obj)
        {
            string stringObject = JsonSerializer.Serialize(obj);
            string filePath = Path.Combine(BaseFolder, obj.Guid.ToString("N"));

            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                await sw.WriteAsync(stringObject);
            }
        }

        public void Remove(Guid objGuid)
        {
            try
            {
                string filePath = Path.Combine(BaseFolder, objGuid.ToString("N"));
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                //if file was not found do nothing
            }

        }

        public async Task<TObject> GetAsync(Guid guid)
        {
            string stringObject = null;
            string filePath = Path.Combine(BaseFolder, guid.ToString("N"));
            if (!File.Exists(filePath))
            {
                return null;
            }
            using (StreamReader sr = new StreamReader(filePath))
            {
                stringObject = await sr.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<TObject>(stringObject);
        }

        public async Task<List<TObject>> GetAllAsync()
        {
            List<TObject> result = new List<TObject>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObject = null;
                using (StreamReader sr = new StreamReader(file))
                {
                    stringObject = await sr.ReadToEndAsync();
                }
                result.Add(JsonSerializer.Deserialize<TObject>(stringObject));
            }
            return result;
        }

        public List<TObject> GetAll()
        {
            List<TObject> result = new List<TObject>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObject = null;
                using (StreamReader sr = new StreamReader(file))
                {
                    stringObject = sr.ReadToEnd();
                }
                result.Add(JsonSerializer.Deserialize<TObject>(stringObject));
            }
            return result;
        }
    }
}
