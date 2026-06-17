using System.Data;
using System.Windows;
using MedicalDeskLib.Repositories;
using Microsoft.Win32;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views.Reports;

public partial class ReportsWindow : Window
{
    private readonly ReportRepository
        repository = new();

    private readonly RequestRepository
        requestRepository = new();

    private readonly EquipmentRepository
        equipmentRepository = new();

    private string currentReport =
        "Отчет";

    public ReportsWindow()
    {
        InitializeComponent();

        dpDateFrom.SelectedDate =
            DateTime.Today.AddMonths(-1);

        dpDateTo.SelectedDate =
            DateTime.Today;

        LoadStatistics();

        currentReport =
            "Отчет по заявкам";

        LoadRequestsReport();
    }

    private DataTable BuildDataTable()
    {
        DataTable table =
            new();

        Dictionary<string, string> headers =
            new()
            {
                { "Id", "Код" },
                { "RequestNumber", "Номер заявки" },
                { "RoomNumber", "Кабинет" },
                { "ApplicantName", "Заявитель" },
                { "ApplicantPhone", "Телефон" },
                { "RequestTypeName", "Тип заявки" },
                { "ProblemDescription", "Описание проблемы" },
                { "StatusName", "Статус" },
                { "CreatedAt", "Дата создания" },
                { "AcceptedAt", "Дата принятия" },
                { "CompletedAt", "Дата завершения" },
                { "ExecutorName", "Исполнитель" },
                { "SpecialistComment", "Комментарий специалиста" },

                { "InventoryNumber", "Инвентарный номер" },
                { "EquipmentName", "Оборудование" },
                { "Model", "Модель" },
                { "Manufacturer", "Производитель" },
                { "UserName", "Ответственный" },
                { "StateName", "Состояние" },

                { "SpecialistName", "Специалист" },
                { "CompletedRequests", "Выполнено заявок" },
                { "ActiveRequests", "Активных заявок" },
                { "AverageHours", "Среднее время (ч)" }
            };

        var first =
            gridReport.Items
            .Cast<object>()
            .FirstOrDefault();

        if (first == null)
            return table;

        var props =
            first.GetType()
            .GetProperties();
        string[] hiddenFields =
   {
    "Id",
    "RequestTypeId",
    "StatusId",
    "ExecutorId",
    "AuthorId"
};

        var visibleProps =
      props
      .Where(x =>
          !hiddenFields.Contains(
              x.Name))
      .ToList();

        foreach (var prop in visibleProps)
        {
            table.Columns.Add(
                headers.ContainsKey(prop.Name)
                    ? headers[prop.Name]
                    : prop.Name);
        }

        foreach (var item in gridReport.Items)
        {
            if (item ==
                System.Windows.Data.CollectionView.NewItemPlaceholder)
                continue;

            DataRow row =
                table.NewRow();

            for (int i = 0;
     i < visibleProps.Count;
     i++)
            {
                object? value =
                    visibleProps[i]
                    .GetValue(item);

                row[i] =
                    value?.ToString() ?? "";
            }

            table.Rows.Add(row);
        }

        return table;
    }

    private void LoadStatistics()
    {
        var stats =
            repository.GetRequestStatistics();

        txtTotal.Text =
            stats.TotalRequests.ToString();

        txtNew.Text =
            stats.NewRequests.ToString();

        txtWork.Text =
            stats.ActiveRequests.ToString();

        txtCompleted.Text =
            stats.CompletedRequests.ToString();
    }

    private DateTime DateFrom =>
        dpDateFrom.SelectedDate ??
        DateTime.Today.AddMonths(-1);

    private DateTime DateTo =>
        dpDateTo.SelectedDate ??
        DateTime.Today;

    private void LoadRequestsReport()
    {
        gridReport.ItemsSource =
            requestRepository
            .GetAll()
            .Where(x =>
                x.CreatedAt.Date >= DateFrom.Date
                &&
                x.CreatedAt.Date <= DateTo.Date)
            .ToList();
    }

    private void btnApplyPeriod_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadRequestsReport();
    }

    private void btnRequests_Click(
        object sender,
        RoutedEventArgs e)
    {
        currentReport =
            "Отчет по заявкам";

        LoadRequestsReport();
    }

    private void btnCompleted_Click(
        object sender,
        RoutedEventArgs e)
    {
        currentReport =
            "Выполненные заявки";

        gridReport.ItemsSource =
            requestRepository
            .GetAll()
            .Where(x =>
                x.StatusName == "Завершена"
                &&
                x.CreatedAt.Date >= DateFrom.Date
                &&
                x.CreatedAt.Date <= DateTo.Date)
            .ToList();
    }
    private void btnSpecialists_Click(
    object sender,
    RoutedEventArgs e)
    {
        currentReport =
            "Отчет по специалистам";

        gridReport.ItemsSource =
            repository.GetSpecialists();
    }

    private void btnEquipment_Click(
        object sender,
        RoutedEventArgs e)
    {
        currentReport =
            "Отчет по оборудованию";

        gridReport.ItemsSource =
            equipmentRepository.GetAll();
    }

    private void btnExcel_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (gridReport.Items.Count == 0)
        {
            MessageBox.Show(
                "Нет данных для экспорта");

            return;
        }

        SaveFileDialog dialog =
            new();

        dialog.Filter =
            "Excel (*.xlsx)|*.xlsx";

        dialog.FileName =
            $"{currentReport}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            DataTable table =
                BuildDataTable();

            ExcelExportService.ExportDataTable(
                table,
                dialog.FileName,
                currentReport);

            MessageBox.Show(
                "Отчет успешно сохранен",
                "Excel",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void btnPdf_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (gridReport.Items.Count == 0)
        {
            MessageBox.Show(
                "Нет данных для экспорта");

            return;
        }

        SaveFileDialog dialog =
            new();

        dialog.Filter =
            "PDF (*.pdf)|*.pdf";

        dialog.FileName =
            $"{currentReport}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            DataTable table =
                BuildDataTable();

            PdfExportService.ExportDataTable(
                table,
                dialog.FileName,
                currentReport);

            MessageBox.Show(
                "PDF отчет успешно сохранен",
                "PDF",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}