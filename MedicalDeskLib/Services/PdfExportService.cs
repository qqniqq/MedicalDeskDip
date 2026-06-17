using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Data;

namespace MedicalDeskLib.Services;

public static class PdfExportService
{
    public static void ExportDataTable(
        DataTable table,
        string fileName,
        string reportName)
    {
        PdfDocument document =
            new();

        PdfPage page =
            document.AddPage();

        // Альбомный лист A4

        page.Width = 842;
        page.Height = 595;

        XGraphics graphics =
            XGraphics.FromPdfPage(page);

        XFont titleFont =
            new XFont(
                "Verdana",
                18,
                XFontStyle.Bold);

        XFont reportFont =
            new XFont(
                "Verdana",
                12,
                XFontStyle.Bold);

        XFont headerFont =
            new XFont(
                "Verdana",
                9,
                XFontStyle.Bold);

        XFont font =
            new XFont(
                "Verdana",
                8);

        // Заголовок

        graphics.DrawString(
            "MedicalDesk",
            titleFont,
            XBrushes.Black,
            20,
            30);

        graphics.DrawString(
            reportName,
            reportFont,
            XBrushes.Black,
            20,
            55);

        graphics.DrawString(
            $"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}",
            font,
            XBrushes.Black,
            20,
            80);

        double y = 120;

        int columnWidth = 90;

        // Заголовки таблицы

        for (int i = 0;
             i < table.Columns.Count;
             i++)
        {
            double x =
                20 + (i * columnWidth);

            graphics.DrawRectangle(
                XPens.Black,
                x,
                y,
                columnWidth,
                25);

            graphics.DrawString(
                table.Columns[i].ColumnName,
                headerFont,
                XBrushes.Black,
                x + 3,
                y + 17);
        }

        y += 25;

        // Строки таблицы

        foreach (DataRow row in table.Rows)
        {
            for (int i = 0;
                 i < table.Columns.Count;
                 i++)
            {
                double x =
                    20 + (i * columnWidth);

                graphics.DrawRectangle(
                    XPens.Black,
                    x,
                    y,
                    columnWidth,
                    25);

                string text =
                    row[i]?.ToString()
                    ?? "";

                if (text.Length > 20)
                {
                    text =
                        text.Substring(
                            0,
                            20) + "...";
                }

                graphics.DrawString(
                    text,
                    font,
                    XBrushes.Black,
                    x + 3,
                    y + 17);
            }

            y += 25;

            // Новая страница

            if (y > 520)
            {
                page =
                    document.AddPage();

                page.Width = 842;
                page.Height = 595;

                graphics =
                    XGraphics.FromPdfPage(
                        page);

                graphics.DrawString(
                    "MedicalDesk",
                    titleFont,
                    XBrushes.Black,
                    20,
                    30);

                graphics.DrawString(
                    reportName,
                    reportFont,
                    XBrushes.Black,
                    20,
                    55);

                y = 90;

                // Повторяем заголовки таблицы

                for (int i = 0;
                     i < table.Columns.Count;
                     i++)
                {
                    double x =
                        20 + (i * columnWidth);

                    graphics.DrawRectangle(
                        XPens.Black,
                        x,
                        y,
                        columnWidth,
                        25);

                    graphics.DrawString(
                        table.Columns[i].ColumnName,
                        headerFont,
                        XBrushes.Black,
                        x + 3,
                        y + 17);
                }

                y += 25;
            }
        }

        document.Save(
            fileName);
    }
}