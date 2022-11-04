using System.Text;
using System.Text.RegularExpressions;

namespace G_ON_WEBAPP.Utilities
{
    public class ConvertText
    {
        static Regex ConvertToUnsign_rg = null;
        public static string ConvertToUnsign(string strUnicode)
        {
            if (ReferenceEquals(ConvertToUnsign_rg, null))
            {
                ConvertToUnsign_rg = new Regex("p{IsCombiningDiacriticalMarks}+");
            }
            var temp = strUnicode.Normalize(NormalizationForm.FormD);
            return ConvertToUnsign_rg.Replace(temp, string.Empty).Replace("đ", "d").Replace("Đ", "D").ToLower();
        }

        public static string UnicodeToAscii(string UnicodeText)
        {
            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = UnicodeText.Normalize(NormalizationForm.FormD);
            return v_reg_regex.Replace(v_str_FormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }
    }
}
