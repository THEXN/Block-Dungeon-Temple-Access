using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TShockAPI;

namespace SkullKingPlugin
{
    public class Configuration
    {
        public static readonly string FilePath = Path.Combine(TShock.SavePath, "阻止进入地牢或神庙.json");
        public bool 阻止玩家进入地牢总开关 = true;
        public bool 击杀未击败骷髅王进入地牢的玩家 = true;
        public bool 传送未击败骷髅王进入地牢的玩家 = true;
        public bool 阻止玩家进入神庙总开关 = true;
        public bool 击杀未击败世纪之花进入神庙的玩家 = true;
        public bool 传送未击败世纪之花进入神庙的玩家 = true;

        public void Write(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(str);
                }
            }
        }

        public static Configuration Read(string path)
        {
            if (!File.Exists(path))
                return new Configuration();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var cf = JsonConvert.DeserializeObject<Configuration>(sr.ReadToEnd());
                    return cf;
                }
            }
        }
    }
}
