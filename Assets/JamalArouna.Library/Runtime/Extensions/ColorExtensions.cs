using UnityEngine;

namespace JamalArouna.Library.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static string ToHex(this Color color) =>
            $"#{ColorUtility.ToHtmlStringRGBA(color)}";
    }
}
