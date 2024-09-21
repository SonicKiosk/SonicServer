using Pastel;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicServer
{
    internal class Logger
    {
        private static string FixedRepeat(string repeat, int count)
        {
            return string.Concat(Enumerable.Repeat(repeat, count / repeat.Length));
        }
        private static void PadConsole(int count)
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine();
        }
        private static string SpaceTextInMiddleOfText(string source, string target)
        {
            return new string(' ', Math.Max(0, source.Length / 2 - target.Length / 2)) + target;
        }
        private static string HalfSpaceTextInMiddleOfTextThatsSoUselessItsBasicallyAHardcodedMethod(string source, string target)
        {
            return new string(' ', (int) (Math.Max(0, source.Length / 2 - target.Length / 2) / 1.5)) + target;
        }
        private static string[] MultiLineSpaceString(string source, string target)
        {
            string[] lines = target.Split('\n');
            string[] output = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
               string line = lines[i];
               output[i] = HalfSpaceTextInMiddleOfTextThatsSoUselessItsBasicallyAHardcodedMethod(FixedRepeat(source, source.Length), line);
            }
            return output;
        }

        public static void LogHeader(string headerCharacter, string text, Color headerColor, Color textColor, int padding, string disposable="*")
        {
            string header = FixedRepeat(headerCharacter, Console.WindowWidth).Pastel(headerColor);
            Console.WriteLine(header);
            PadConsole(padding);
            if (text.Split('\n').Length == 0)
                Console.WriteLine(SpaceTextInMiddleOfText(header, text.Pastel(textColor)));
            else
                foreach (string line in MultiLineSpaceString(header, text.Pastel(textColor)))
                    Console.WriteLine(line.Replace(disposable, " "));
            PadConsole(padding);
            Console.WriteLine(header);

            Console.WriteLine();
        }


        public string Tag { get; private set; } = "UNKNOWN";
        public Color TagColor { get; private set; } = Color.Gray;
        public float TagBrightnessMultiplier { get; private set; } = 1.2f;
        public Logger(string tag, Color? tagColor = null, float tagBrightnessMultiplier = 1.2f)
        {
            Tag = tag;
            TagColor = tagColor ?? Color.Gray;
            TagBrightnessMultiplier = tagBrightnessMultiplier;
        }

        // TODO: uhhhhh add logger methods that ARENT balls..
        public static (int, int, int) HSL2RGB(double h, double sl, double l)
        {
            double r = l, g = l, b = l;
            double v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                double m = l + l - v;
                double sv = (v - m) / v;
                h *= 6.0;
                int sextant = (int)h;
                double fract = h - sextant;
                double vsf = v * sv * fract;
                double mid1 = m + vsf;
                double mid2 = v - vsf;

                switch (sextant)
                {
                    case 0: r = v; g = mid1; b = m; break;
                    case 1: r = mid2; g = v; b = m; break;
                    case 2: r = m; g = v; b = mid1; break;
                    case 3: r = m; g = mid2; b = v; break;
                    case 4: r = mid1; g = m; b = v; break;
                    case 5: r = v; g = m; b = mid2; break;
                }
            }

            return
            (
                Convert.ToByte(Math.Clamp(r * 255.0, 0, 255)),
                Convert.ToByte(Math.Clamp(g * 255.0, 0, 255)),
                Convert.ToByte(Math.Clamp(b * 255.0, 0, 255))
            );
        }

        // add clamping to all these types of methods lol
        private static Color brightenColor(Color input, float amount)
        {
            float hue = input.GetHue() / 360;
            float saturation = input.GetSaturation();
            float brightness = input.GetBrightness();
            //Console.WriteLine($"{hue}, {saturation}, {brightness}");

            (int r, int g, int b) = HSL2RGB(hue, saturation / amount, brightness * amount);
            return Color.FromArgb(1, r, g, b);
        }
        private static Color saturateColor(Color input, float amount)
        {
            float hue = input.GetHue() / 360;
            float saturation = input.GetSaturation();
            float brightness = input.GetBrightness();
            //Console.WriteLine($"{hue}, {saturation}, {brightness}");

            (int r, int g, int b) = HSL2RGB(hue, saturation * amount, brightness);
            return Color.FromArgb(1, r, g, b);
        }



        public static Color ERROR_TAG_COLOR { get; internal set; } = Color.IndianRed;
        public static Color DEBUG_TAG_COLOR { get; internal set; } = Color.HotPink;
        public static Color WARN_TAG_COLOR  { get; internal set; } = Color.Gold;

        public static string GetFormattedTag(string tag, Color color)
        {
            string leading = "[".Pastel(color);
            string trailing = "]".Pastel(color);
            string colortag = tag.Pastel(brightenColor(saturateColor(color, 1.5f), 1.2f));
            return $"{leading}{colortag}{trailing} ";
        }
        private string getTagString()
        {
            return GetFormattedTag(Tag, TagColor);
            //string leading = "[".Pastel(TagColor);
            //string trailing = "]".Pastel(TagColor);
            //string colortag = Tag.Pastel(brightenColor(TagColor, 1.2f));
            //return $"{leading}{colortag}{trailing} ";
        }
        public static string GetErrorTag()
        {
            return GetFormattedTag("ERROR", ERROR_TAG_COLOR);
            //string leading = "[".Pastel(ERROR_TAG_COLOR);
            //string trailing = "]".Pastel(ERROR_TAG_COLOR);
            //string colortag = "ERROR".Pastel(brightenColor(saturateColor(ERROR_TAG_COLOR, 1.5f), 1.2f));
            //return $"{leading}{colortag}{trailing} ";
        }
        public static string GetWarnTag()
        {
            return GetFormattedTag("WARN", WARN_TAG_COLOR);
            //string leading = "[".Pastel(WARN_TAG_COLOR);
            //string trailing = "]".Pastel(WARN_TAG_COLOR);
            //string colortag = "WARN".Pastel(brightenColor(WARN_TAG_COLOR, 1.2f));
            //return $"{leading}{colortag}{trailing} ";
        }
        public static string GetDebugTag()
        {
            return GetFormattedTag("DEBUG", DEBUG_TAG_COLOR);
            //string leading = "[".Pastel(DEBUG_TAG_COLOR);
            //string trailing = "]".Pastel(DEBUG_TAG_COLOR);
            //string colortag = "DEBUG".Pastel(brightenColor(DEBUG_TAG_COLOR, 1.2f));
            //return $"{leading}{colortag}{trailing} ";
        }


        static string[] NormalizeArray(object?[] input) // sucks balls but wtv cba to write a better one
        {
            string[] output = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == null)
                {
                    output[i] = "null";
                }
                else if (input[i] is object[] nestedArray)
                {
                    output[i] = $"{{{string.Join(",", NormalizeArray(nestedArray))}}}";
                }
                else
                {
                    output[i] = input[i]?.ToString() ?? "[object->string failure]";
                }
            }

            return output;
        }


        public void Newline(int count=1)
        {
            PadConsole(count);
        }
        public void Info(params object?[] args)
        {
            //Console.WriteLine(getTagString() + string.Join(' ', args));
            Log(LogStyle.Info, args);
        }

        public void Error(params object?[] args)
        {
            //Console.WriteLine(getTagString() + GetErrorTag() + string.Join(' ', args));
            Log(LogStyle.Error, args);
        }

        public void Warn(params object?[] args)
        {
            //Console.WriteLine(getTagString() + GetWarnTag() + string.Join(' ', args));
            Log(LogStyle.Warn, args);
        }

        public void Debug(params object?[] args)
        {
            //Console.WriteLine(getTagString() + GetDebugTag() + string.Join(' ', args));
            Log(LogStyle.Debug, args);
        }
        public void ErrorDebug(params object?[] args)
        {
            //Console.WriteLine(getTagString() + GetErrorTag() + GetDebugTag() + string.Join(' ', args));
            Log(LogStyle.Error | LogStyle.Debug, args);
        }
        public void WarnDebug(params object?[] args)
        {
            //Console.WriteLine(getTagString() + GetWarnTag() + GetDebugTag() + string.Join(' ', args));
            Log(LogStyle.Warn | LogStyle.Debug, args);
        }

        [Flags]
        public enum LogStyle
        {
            Info = 0,
            Warn = 1 << 1,
            Error = 1 << 2,
            Debug = 1 << 3 
        }

        public void Log(LogStyle style, params object?[] args)
        {
            LogInternal(style, args);
        }

        private void LogInternal(LogStyle style, params object?[] args)
        {
            var output = new StringBuilder(getTagString());

            if ((style & LogStyle.Warn) == LogStyle.Warn)
            {
                output.Append(GetWarnTag());
            }
            if ((style & LogStyle.Error) == LogStyle.Error)
            {
                output.Append(GetErrorTag());
            }
            if ((style & LogStyle.Debug) == LogStyle.Debug)
            {
                //new Logger("_internal_logger", Color.White).Info(SServer.settings!.Developer);
                if (SServer.settings != null && !SServer.settings.Developer)
                    return;

                output.Append(GetDebugTag());
            }

            output.Append(string.Join(' ', NormalizeArray(args)));
            Console.WriteLine(output.ToString());
        }

        public void TestStyles(params object?[] args)
        {
            //Console.WriteLine(getTagString() + GetErrorTag() + GetWarnTag() + GetDebugTag() + string.Join(' ', args));
            Log(LogStyle.Info | LogStyle.Warn | LogStyle.Error | LogStyle.Debug, args);
        }
    }
}
