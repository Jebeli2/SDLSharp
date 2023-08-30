namespace SDLSharp.Content.Flare
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FileParser : IDisposable
    {
        private string name;
        private string? line;
        private int lineNum;
        private StreamReader reader;
        private bool newSection;
        private string section;
        private string key;
        private string val;
        private string originalVal;


        public FileParser(string name, byte[]? data)
        {
            this.name = name;

            MemoryStream ms = data != null ? new MemoryStream(data, false) : new MemoryStream();
            reader = new StreamReader(ms);

            newSection = false;
            section = "";
            key = "";
            val = "";
            originalVal = "";
        }

        public bool NewSection => newSection;
        public string Section => section;
        public string Key => key;
        public string Val => val;
        public string OriginalVal => originalVal;

        public bool Next()
        {
            newSection = false;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine()?.Trim();
                lineNum++;
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith("#")) continue;
                if (line.StartsWith("["))
                {
                    newSection = true;
                    section = ParseSectionTitle(line);
                    continue;
                }
                if (line.Equals("APPEND")) continue;
                int firstSpace = line.IndexOf(" ");
                if (firstSpace >= 0)
                {
                    string directive = line.Substring(0, firstSpace);
                    if (directive.Equals("INCLUDE"))
                    {
                        continue;
                    }
                }
                ParseKeyPair(line, ref key, ref val);
                originalVal = val;
                return true;

            }
            return false;
        }
        public string GetRawLine()
        {
            if (!reader.EndOfStream)
            {
                return reader.ReadLine() ?? "";
            }
            return "";
        }
        public void IncrementLineNum()
        {
            lineNum++;
        }
        public bool MatchKey(string key)
        {
            return key.Equals(Key, StringComparison.OrdinalIgnoreCase);
        }

        public bool MatchSection(string section)
        {
            return section.Equals(Section, StringComparison.OrdinalIgnoreCase);
        }

        public bool MatchNewSection(string section)
        {
            return NewSection && MatchSection(section);
        }

        public bool MatchDifferentSection(string section)
        {
            return NewSection && !MatchSection(section);
        }

        public bool MatchSectionKey(string section, string key)
        {
            return MatchSection(section) && MatchKey(key);
        }

        public Color GetColorBGRAValue()
        {
            return GetColorBGRAValue(Color.Black);
        }
        public Color GetColorBGRAValue(Color def)
        {
            if (string.IsNullOrEmpty(val)) return def;
            int b = PopFirstInt(ref val);
            int g = PopFirstInt(ref val);
            int r = PopFirstInt(ref val);
            int a = PopFirstInt(ref val);
            return Color.FromArgb(a, r, g, b);
        }

        public Color GetColorRGBAValue()
        {
            return GetColorRGBAValue(Color.Black);
        }
        public Color GetColorRGBAValue(Color def)
        {
            if (string.IsNullOrEmpty(val)) return def;
            int r = PopFirstInt(ref val);
            int g = PopFirstInt(ref val);
            int b = PopFirstInt(ref val);
            int a = PopFirstInt(ref val);
            return Color.FromArgb(a, r, g, b);
        }
        public IList<string> GetStrValues()
        {
            List<string> list = new();
            var s = PopFirstString();
            while (!string.IsNullOrEmpty(s))
            {
                list.Add(s);
                s = PopFirstString();
            }
            return list;
        }
        public string GetStrVal(string def = "")
        {
            if (!string.IsNullOrEmpty(Val)) return Val;
            return def;
        }
        public bool GetBoolVal(bool def = false)
        {
            if (bool.TryParse(Val, out bool result)) return result;
            return def;
        }
        public float GetFloatVal(float def = 0)
        {
            if (float.TryParse(Val, out float result)) return result;
            return def;
        }
        public int GetIntVal(int def = 0)
        {
            if (int.TryParse(Val, out int result)) return result;
            return def;
        }
        public T GetEnumValue<T>(T def = default) where T : struct
        {
            if (Enum.TryParse(Val, true, out T result)) return result;
            return def;
        }
        public void Dispose()
        {
            reader.Dispose();
        }

        public int PopFirstInt(char separator = '\0')
        {
            return PopFirstInt(ref val, separator);
        }
        public float PopFirstFloat(char separator = '\0')
        {
            return PopFirstFloat(ref val, separator);
        }
        public string PopFirstString(char separator = '\0')
        {
            return PopFirstString(ref val, separator);
        }

        public Rectangle PopFirstRect()
        {
            return ParseRect(ref val);
        }

        public static Rectangle ParseRect(ref string value)
        {
            int x = PopFirstInt(ref value);
            int y = PopFirstInt(ref value);
            int w = PopFirstInt(ref value);
            int h = PopFirstInt(ref value);
            return new Rectangle(x, y, w, h);
        }
        public static int ParseDirection(string value)
        {
            int dir = 0;
            switch (value)
            {
                case "N":
                    dir = 3;
                    break;
                case "NE":
                    dir = 4;
                    break;
                case "E":
                    dir = 5;
                    break;
                case "SE":
                    dir = 6;
                    break;
                case "S":
                    dir = 7;
                    break;
                case "SW":
                    dir = 0;
                    break;
                case "W":
                    dir = 1;
                    break;
                case "NW":
                    dir = 2;
                    break;
                default:
                    int.TryParse(value, out dir);
                    break;
            }
            if (dir < 0 || dir > 7)
            {
                dir = 0;
            }
            return dir;
        }
        public static int ParseDurationMS(string value)
        {
            int val;
            if (value.EndsWith("ms"))
            {
                int.TryParse(value.Replace("ms", ""), out val);
                if (val == 0) return 0;
            }
            else if (value.EndsWith("s"))
            {
                int.TryParse(value.Replace("s", ""), out val);
                if (val == 0) return 0;
                val *= 1000;
            }
            else
            {
                int.TryParse(value, out val);
                if (val == 0) return 0;
            }
            if (val < 1) val = 1;
            return val;
        }
        public static string PopFirstString(ref string s, char separator = '\0')
        {
            int seppos = 0;
            if (separator == '\0')
            {
                seppos = s.IndexOf(',');
                int altSeppos = s.IndexOf(';');
                if (altSeppos >= 0 && altSeppos < seppos)
                {
                    seppos = altSeppos;
                }
            }
            else
            {
                seppos = s.IndexOf(separator);
            }
            string outs;
            if (seppos < 0)
            {
                outs = s;
                s = "";
            }
            else
            {
                outs = s.Substring(0, seppos);
                s = s.Substring(seppos + 1);
            }
            return outs;
        }
        public static int PopFirstInt(ref string s, char separator = '\0')
        {
            if (int.TryParse(PopFirstString(ref s, separator), out int result))
            {
                return result;
            }
            return 0;
        }

        private static float PopFirstFloat(ref string s, char separator = '\0')
        {
            if (float.TryParse(PopFirstString(ref s, separator), out float result))
            {
                return result;
            }
            return 0;
        }
        private static string ParseSectionTitle(string line)
        {
            int bracket = line.IndexOf("]");
            if (bracket > 0)
            {
                return line.Substring(1, bracket - 1);
            }
            return "";
        }
        private static void ParseKeyPair(string line, ref string key, ref string val)
        {
            int separator = line.IndexOf('=');
            if (separator < 0)
            {
                key = "";
                val = "";
            }
            else
            {
                key = line.Substring(0, separator);
                val = line.Substring(separator + 1);
                key = key.Trim();
                val = val.Trim();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(section);
            sb.Append('-');
            sb.Append(key);
            sb.Append(':');
            sb.Append(originalVal);
            return sb.ToString();
        }
    }
}
