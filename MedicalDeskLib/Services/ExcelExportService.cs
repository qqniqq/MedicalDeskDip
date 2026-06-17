using ClosedXML.Excel;
using System.Data;

namespace MedicalDeskLib.Services;

public static class ExcelExportService
{
    public static void ExportDataTable(
        DataTable table,
        string fileName,
        string reportName)
    {
        using XLWorkbook workbook =
            new();

        var sheet =
            workbook.Worksheets.Add(
                "Отчет");

        // Заголовок

        sheet.Cell("A1").Value =
            "MedicalDesk";

        sheet.Cell("A2").Value =
            reportName;

        sheet.Cell("A3").Value =
            $"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}";

        sheet.Range(
            1,
            1,
            1,
            table.Columns.Count)
            .Merge();

        sheet.Range(
            2,
            1,
            2,
            table.Columns.Count)
            .Merge();

        sheet.Range(
            3,
            1,
            3,
            table.Columns.Count)
            .Merge();

        // Оформление заголовка

        sheet.Cell("A1")
            .Style.Font.Bold = true;

        sheet.Cell("A1")
            .Style.Font.FontSize = 18;

        sheet.Cell("A1")
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        sheet.Cell("A2")
            .Style.Font.Bold = true;

        sheet.Cell("A2")
            .Style.Font.FontSize = 14;

        sheet.Cell("A2")
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        sheet.Cell("A3")
            .Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;

        int startRow = 5;

        // Заголовки таблицы

        for (int i = 0;
             i < table.Columns.Count;
             i++)
        {
            var cell =
                sheet.Cell(
                    startRow,
                    i + 1);

            cell.Value =
                table.Columns[i]
                .ColumnName;

            cell.Style.Font.Bold =
                true;

            cell.Style.Fill.BackgroundColor =
                XLColor.LightBlue;

            cell.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;

            cell.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
        }

        // Данные

        for (int row = 0;
             row < table.Rows.Count;
             row++)
        {
            for (int col = 0;
                 col < table.Columns.Count;
                 col++)
            {
                var cell =
                    sheet.Cell(
                        row + startRow + 1,
                        col + 1);

                cell.Value =
                    table.Rows[row][col]
                    ?.ToString();

                cell.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;
            }
        }

        // Автофильтр

        sheet.Range(
            startRow,
            1,
            startRow + table.Rows.Count,
            table.Columns.Count)
            .SetAutoFilter();

        // Автоширина

        sheet.Columns()
             .AdjustToContents();

        // Минимальная ширина

        foreach (var column in sheet.Columns())
        {
            if (column.Width < 15)
            {
                column.Width = 15;
            }
        }

        workbook.SaveAs(
            fileName);
    }
}