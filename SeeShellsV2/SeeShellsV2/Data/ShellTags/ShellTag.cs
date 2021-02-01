using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class ShellTag : IShellTag
    {
        public string Name { init; get; }

        public double Importance { init; get; }

        public Color Color { get => assignedColors[Name]; }

        public ShellTag(string name, double importance)
        {
            if (!assignedImportanceScores.ContainsKey(name))
                assignedImportanceScores.Add(name, importance);
            else if (assignedImportanceScores[name] != importance)
                throw new ArgumentException("importance score of new ShellTag {0} differs from previously registered score", name);

            if (!assignedColors.ContainsKey(name))
                assignedColors.Add(name, GetUniqueColor());

            Name = name;
            Importance = importance;
        }

        public int CompareTo(IShellTag other)
        {
            return Name.CompareTo(other.Name);
        }

        private Color GetUniqueColor()
        {
            return ColorFromHSV((nextColorSeed++ / goldenRatio) % 1, saturation, value);
        }

        private Dictionary<string, double> assignedImportanceScores = new Dictionary<string, double>();
        private Dictionary<string, Color> assignedColors = new Dictionary<string, Color>();

        private readonly double goldenRatio = 1.61803398874989484820458683436;
        private readonly double saturation = 0.9;
        private readonly double value = 0.9;

        private int nextColorSeed = 0;

        // borrowed color mappings from https://stackoverflow.com/a/1626232

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
