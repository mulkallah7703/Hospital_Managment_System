using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using HospitalManagementSystem.Web.Models;

namespace HospitalManagementSystem.Web.Helpers
{
    public static class ExportHelper
    {
        private static readonly string[] ColumnHeaders =
        {
            "PatientID",
            "NationalID",
            "FirstName",
            "LastName",
            "Gender",
            "DateOfBirth",
            "Phone",
            "Email",
            "BloodType",
            "CreatedDate"
        };

        public static void ExportToExcel(IList<Patient> patients, HttpResponse response)
        {
            if (patients == null)
            {
                patients = new List<Patient>();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sb.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
            sb.AppendLine("<Worksheet ss:Name=\"Patients\">");
            sb.AppendLine("<Table>");

            sb.AppendLine("<Row>");
            foreach (string header in ColumnHeaders)
            {
                sb.Append("<Cell><Data ss:Type=\"String\">");
                sb.Append(XmlEscape(header));
                sb.AppendLine("</Data></Cell>");
            }
            sb.AppendLine("</Row>");

            foreach (Patient patient in patients)
            {
                sb.AppendLine("<Row>");
                AppendExcelNumberCell(sb, patient.PatientID);
                AppendExcelStringCell(sb, patient.NationalID);
                AppendExcelStringCell(sb, patient.FirstName);
                AppendExcelStringCell(sb, patient.LastName);
                AppendExcelStringCell(sb, patient.Gender);
                AppendExcelStringCell(sb, FormatDate(patient.DateOfBirth));
                AppendExcelStringCell(sb, patient.Phone);
                AppendExcelStringCell(sb, patient.Email);
                AppendExcelStringCell(sb, patient.BloodType);
                AppendExcelStringCell(sb, FormatDateTime(patient.CreatedDate));
                sb.AppendLine("</Row>");
            }

            sb.AppendLine("</Table>");
            sb.AppendLine("</Worksheet>");
            sb.AppendLine("</Workbook>");

            WriteDownload(
                response,
                "PatientReport.xls",
                "application/vnd.ms-excel",
                Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public static void ExportToWord(IList<Patient> patients, HttpResponse response)
        {
            if (patients == null)
            {
                patients = new List<Patient>();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sb.AppendLine(" xmlns:w=\"urn:schemas-microsoft-com:office:word\"");
            sb.AppendLine(" xmlns=\"http://www.w3.org/TR/REC-html40\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.AppendLine("<!--[if gte mso 9]><xml><w:WordDocument><w:View>Print</w:View></w:WordDocument></xml><![endif]-->");
            sb.AppendLine("<title>Patient Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Calibri, Arial, sans-serif; font-size: 11pt; }");
            sb.AppendLine("h1 { font-size: 16pt; color: #0b3d5c; }");
            sb.AppendLine("table { border-collapse: collapse; width: 100%; }");
            sb.AppendLine("th, td { border: 1px solid #333; padding: 4px 6px; text-align: left; }");
            sb.AppendLine("th { background: #f0f6fb; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head><body>");
            sb.AppendLine("<h1>Patient Report</h1>");
            sb.AppendLine("<p>Generated: " + HtmlEscape(DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)) + "</p>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr>");
            foreach (string header in ColumnHeaders)
            {
                sb.Append("<th>");
                sb.Append(HtmlEscape(header));
                sb.Append("</th>");
            }
            sb.AppendLine("</tr>");

            foreach (Patient patient in patients)
            {
                sb.AppendLine("<tr>");
                AppendHtmlCell(sb, patient.PatientID.ToString(CultureInfo.InvariantCulture));
                AppendHtmlCell(sb, patient.NationalID);
                AppendHtmlCell(sb, patient.FirstName);
                AppendHtmlCell(sb, patient.LastName);
                AppendHtmlCell(sb, patient.Gender);
                AppendHtmlCell(sb, FormatDate(patient.DateOfBirth));
                AppendHtmlCell(sb, patient.Phone);
                AppendHtmlCell(sb, patient.Email);
                AppendHtmlCell(sb, patient.BloodType);
                AppendHtmlCell(sb, FormatDateTime(patient.CreatedDate));
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            WriteDownload(
                response,
                "PatientReport.doc",
                "application/msword",
                Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public static void ExportToPdf(IList<Patient> patients, HttpResponse response)
        {
            if (patients == null)
            {
                patients = new List<Patient>();
            }

            byte[] pdfBytes = BuildPatientPdf(patients);
            WriteDownload(response, "PatientReport.pdf", "application/pdf", pdfBytes);
        }

        private static byte[] BuildPatientPdf(IList<Patient> patients)
        {
            // Landscape A4: 842 x 595
            const float pageWidth = 842f;
            const float pageHeight = 595f;
            const float marginLeft = 30f;
            const float marginTop = 40f;
            const float marginBottom = 30f;
            const float lineHeight = 12f;
            const float fontSize = 8f;

            List<string> contentStreams = new List<string>();
            StringBuilder page = new StringBuilder();
            float y = pageHeight - marginTop;

            Action startPage = () =>
            {
                page = new StringBuilder();
                y = pageHeight - marginTop;
                page.Append("BT\n");
                page.Append("/F1 ").Append(fontSize.ToString(CultureInfo.InvariantCulture)).Append(" Tf\n");
                WritePdfText(page, marginLeft, y, "Patient Report - Generated " + DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
                y -= (lineHeight + 4f);
                WritePdfText(page, marginLeft, y, string.Join(" | ", ColumnHeaders));
                y -= lineHeight;
                WritePdfText(page, marginLeft, y, new string('-', 140));
                y -= lineHeight;
            };

            Action endPage = () =>
            {
                page.Append("ET\n");
                contentStreams.Add(page.ToString());
            };

            startPage();

            if (patients.Count == 0)
            {
                WritePdfText(page, marginLeft, y, "No patients found.");
                endPage();
            }
            else
            {
                foreach (Patient patient in patients)
                {
                    if (y < marginBottom + lineHeight)
                    {
                        endPage();
                        startPage();
                    }

                    string line = string.Join(" | ", new[]
                    {
                        Truncate(patient.PatientID.ToString(CultureInfo.InvariantCulture), 6),
                        Truncate(NullToEmpty(patient.NationalID), 12),
                        Truncate(NullToEmpty(patient.FirstName), 12),
                        Truncate(NullToEmpty(patient.LastName), 12),
                        Truncate(NullToEmpty(patient.Gender), 8),
                        Truncate(FormatDate(patient.DateOfBirth), 10),
                        Truncate(NullToEmpty(patient.Phone), 12),
                        Truncate(NullToEmpty(patient.Email), 22),
                        Truncate(NullToEmpty(patient.BloodType), 6),
                        Truncate(FormatDateTime(patient.CreatedDate), 16)
                    });

                    WritePdfText(page, marginLeft, y, line);
                    y -= lineHeight;
                }

                endPage();
            }

            return AssemblePdf(contentStreams, pageWidth, pageHeight);
        }

        private static byte[] AssemblePdf(List<string> contentStreams, float pageWidth, float pageHeight)
        {
            List<byte[]> objects = new List<byte[]>();
            // Object numbers: 1=Catalog, 2=Pages, 3..=page objs, then content objs, then font
            int pageCount = contentStreams.Count;
            int firstPageObj = 3;
            int firstContentObj = firstPageObj + pageCount;
            int fontObj = firstContentObj + pageCount;

            // 1 Catalog
            objects.Add(Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"));

            // 2 Pages
            StringBuilder kids = new StringBuilder("<< /Type /Pages /Kids [");
            for (int i = 0; i < pageCount; i++)
            {
                if (i > 0)
                {
                    kids.Append(' ');
                }
                kids.Append(firstPageObj + i).Append(" 0 R");
            }
            kids.Append("] /Count ").Append(pageCount).Append(" >>");
            objects.Add(Encoding.ASCII.GetBytes(kids.ToString()));

            // Page objects
            for (int i = 0; i < pageCount; i++)
            {
                int contentObjNum = firstContentObj + i;
                string pageObj =
                    "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 " +
                    pageWidth.ToString(CultureInfo.InvariantCulture) + " " +
                    pageHeight.ToString(CultureInfo.InvariantCulture) +
                    "] /Contents " + contentObjNum + " 0 R /Resources << /Font << /F1 " +
                    fontObj + " 0 R >> >> >>";
                objects.Add(Encoding.ASCII.GetBytes(pageObj));
            }

            // Content stream objects
            foreach (string stream in contentStreams)
            {
                byte[] streamBytes = Encoding.ASCII.GetBytes(stream);
                string header = "<< /Length " + streamBytes.Length + " >>\nstream\n";
                string footer = "\nendstream";
                byte[] headerBytes = Encoding.ASCII.GetBytes(header);
                byte[] footerBytes = Encoding.ASCII.GetBytes(footer);
                byte[] combined = new byte[headerBytes.Length + streamBytes.Length + footerBytes.Length];
                Buffer.BlockCopy(headerBytes, 0, combined, 0, headerBytes.Length);
                Buffer.BlockCopy(streamBytes, 0, combined, headerBytes.Length, streamBytes.Length);
                Buffer.BlockCopy(footerBytes, 0, combined, headerBytes.Length + streamBytes.Length, footerBytes.Length);
                objects.Add(combined);
            }

            // Font object
            objects.Add(Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"));

            return BuildPdfFile(objects);
        }

        private static byte[] BuildPdfFile(List<byte[]> objects)
        {
            List<long> offsets = new List<long>();
            List<byte> file = new List<byte>();

            Action<string> appendAscii = text =>
            {
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                file.AddRange(bytes);
            };

            appendAscii("%PDF-1.4\n");

            for (int i = 0; i < objects.Count; i++)
            {
                offsets.Add(file.Count);
                appendAscii((i + 1).ToString(CultureInfo.InvariantCulture) + " 0 obj\n");
                file.AddRange(objects[i]);
                appendAscii("\nendobj\n");
            }

            long xrefPos = file.Count;
            appendAscii("xref\n0 " + (objects.Count + 1).ToString(CultureInfo.InvariantCulture) + "\n");
            appendAscii("0000000000 65535 f \n");
            foreach (long offset in offsets)
            {
                appendAscii(offset.ToString("D10", CultureInfo.InvariantCulture) + " 00000 n \n");
            }

            appendAscii("trailer\n<< /Size " + (objects.Count + 1).ToString(CultureInfo.InvariantCulture) +
                        " /Root 1 0 R >>\n");
            appendAscii("startxref\n");
            appendAscii(xrefPos.ToString(CultureInfo.InvariantCulture) + "\n");
            appendAscii("%%EOF");

            return file.ToArray();
        }

        private static void WritePdfText(StringBuilder page, float x, float y, string text)
        {
            page.Append("1 0 0 1 ")
                .Append(x.ToString(CultureInfo.InvariantCulture))
                .Append(' ')
                .Append(y.ToString(CultureInfo.InvariantCulture))
                .Append(" Tm (")
                .Append(PdfEscape(text))
                .Append(") Tj\n");
        }

        private static void WriteDownload(HttpResponse response, string fileName, string contentType, byte[] content)
        {
            response.Clear();
            response.Buffer = true;
            response.ContentType = contentType;
            response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            response.AddHeader("Content-Length", content.Length.ToString(CultureInfo.InvariantCulture));
            response.BinaryWrite(content);
            response.Flush();
            response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private static void AppendExcelStringCell(StringBuilder sb, string value)
        {
            sb.Append("<Cell><Data ss:Type=\"String\">");
            sb.Append(XmlEscape(NullToEmpty(value)));
            sb.AppendLine("</Data></Cell>");
        }

        private static void AppendExcelNumberCell(StringBuilder sb, int value)
        {
            sb.Append("<Cell><Data ss:Type=\"Number\">");
            sb.Append(value.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("</Data></Cell>");
        }

        private static void AppendHtmlCell(StringBuilder sb, string value)
        {
            sb.Append("<td>");
            sb.Append(HtmlEscape(NullToEmpty(value)));
            sb.Append("</td>");
        }

        private static string FormatDate(DateTime? value)
        {
            return value.HasValue
                ? value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private static string FormatDateTime(DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        private static string NullToEmpty(string value)
        {
            return value ?? string.Empty;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value ?? string.Empty;
            }

            return value.Substring(0, maxLength);
        }

        private static string XmlEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        private static string HtmlEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }

        private static string PdfEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            // Keep PDF string literals in WinAnsi-safe ASCII for Helvetica.
            StringBuilder sb = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                if (c == '\\' || c == '(' || c == ')')
                {
                    sb.Append('\\').Append(c);
                }
                else if (c >= 32 && c <= 126)
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('?');
                }
            }

            return sb.ToString();
        }
    }
}
