using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicServer
{

    public abstract class AbstractGenericConfig<T> where T : class
    {
        public static string FileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string Resolve(string path)
        {
            return Path.Combine(FileDirectory, path);
        }
        public abstract T DefaultValue();
        public virtual void SaveToFile(string fileName)
        {
            File.WriteAllText(Resolve($"{fileName}.json"), JsonConvert.SerializeObject(this));
        }
        public static void SaveToFile(T instance, string fileName)
        {
            File.WriteAllText(Resolve($"{fileName}.json"), JsonConvert.SerializeObject(instance));
        }

        public T ReadFromFile(string fileName)
        {
            if (File.Exists(AbstractGenericConfig<T>.Resolve($"{fileName}.json")))
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Resolve($"{fileName}.json"))) ?? DefaultValue();
            else
                return DefaultValue();
        }
        public static T? ReadFromFileStatic(string fileName)
        {
            if (File.Exists(AbstractGenericConfig<T>.Resolve($"{fileName}.json")))
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(Resolve($"{fileName}.json")));
            else
                return null;
        }
    }
}
