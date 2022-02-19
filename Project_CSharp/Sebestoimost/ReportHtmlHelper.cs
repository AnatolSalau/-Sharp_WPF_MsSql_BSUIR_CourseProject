using System;

namespace Sebestoimost
{
    public class ReportHtmlHelper
    {
        public static string PageStart(string title)
        {
            return
                "<!doctype html>" +
                "<html lang=\"ru\">" +
                "<head>" +
                "<meta charset=\"utf-8\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">" +
                "<title>" + title + "</title>" +
                "<style>" +
                "body { font-family: sans-serif; font-size: 14px; font-weight: 400; line-height: 1.25; color: #212529; margin: 0; padding: 8px 16px; }" +
                "h1 { text-align: center; font-size: 24px; margin: 0; font-weight: 700; }" +
                ".dates { text-align: center; font-size: 16px; margin: 0 0 8px; font-weight: 500; }" +
                "table { width: 100%; margin: 0 0 16px; border-collapse: collapse; }" +
                "table td { padding: 2px 4px; border: 1px solid #6c757d; }" +
                "thead td { font-weight: 700; text-align: center }" +
                "thead, tbody { border-bottom: 2px solid #343a40; }" +
                "tbody tr { border-bottom: 1px solid #6c757d; }" +
                ".text-right { text-align: right; }" +
                "tr.substr { background-color: #e8e9ea; font-weight: 500; }" +
                "</style>" +
                "</head>" +
                "<body>";
        }
        public static string ReportHeader(string title, DateTime dateAt, DateTime dateTo)
        {
            return string.Format("<h1>{0}</h1><p class=\"dates\">{1:dd.MM.yyyy} - {2:dd.MM.yyyy}</p>", title, dateAt, dateTo);
        }
        public static string Table(string thead, string tbody)
        {
            return "<table><thead>" + thead + "</thead><tbody>" + tbody + "</tbody></table>";
        }

        public static string TrColspan(string body, int cnt)
        {
            return "<tr><td colspan=\"" + cnt + "\">" + body + "</td></tr>";
        }

        public static string Tr(string[] strData)
        {
            string res = "<tr>";
            foreach(string data in strData)
            {
                res += "<td>" + data + "</td>";
            }
            res += "</tr>";
            return res;
        }

        public static string TrSub(string[] strData)
        {
            string res = "<tr class=\"substr\">";
            foreach (string data in strData)
            {
                res += "<td>" + data + "</td>";
            }
            res += "</tr>";
            return res;
        }

        public static string PageEnd()
        {
            return "</body></html>";
        }
    }
}
