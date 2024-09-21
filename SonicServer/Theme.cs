using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicServer
{
    public class ThemeRegistry
    {
        public static Dictionary<string, Theme> Themes = new Dictionary<string, Theme>()
        {
            { "DefaultDark", DefaultDark }
        };
        public static Theme DefaultDark = new Theme();
    }
    public struct Theme
    {
        public Color BackgroundColor = Color.FromArgb(20, 20, 20);
        public Color ElementColor = Color.FromArgb(30, 30, 30);
        public Color PageColor = Color.FromArgb(10, 10, 10);

        public Color TextBoxColor = Color.FromArgb(20, 20, 20);
        public Color TextColor = Color.FromArgb(225, 225, 225);
        public Color SecondaryTextColor = Color.FromArgb(175, 175, 175);

        public Color SeperatorColor = Color.FromArgb(155, 155, 155);

        public FlatStyle FlatStyle = FlatStyle.Flat;
        public BorderStyle BorderStyle = BorderStyle.None;

        public struct DataBoxColors
        {
            public Color CheckIn = Color.FromArgb(0, 200, 0);
            public Color CheckOut = Color.FromArgb(255, 50, 50);
            public Color OrderBtn = Color.FromArgb(75, 155, 155);
            public Color InfoBtn = Color.FromArgb(255, 255, 155);
            public Color ClientLbl = Color.Orchid;
            public Color InfoLbl = Color.FromArgb(100, 100, 255);

            public DataBoxColors()
            {
            }
        }

        public DataBoxColors DataBoxColorGuide = new DataBoxColors();

        public Theme() { }
    }
}
