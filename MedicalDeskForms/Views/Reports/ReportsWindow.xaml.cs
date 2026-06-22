using MedicalDeskLib.Repositories;
using MedicalDeskLib.Services;
using Microsoft.Win32;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        ConfigureRequestColumns();

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
                { "CommissioningDate", "Дата ввода" },
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
    "AuthorId",

    "UserId",
    "StateId",

    "ExecutionHours",
    "ResolutionComment",

    "ApplicantPhone",
    "AcceptedAt",
    "CompletedAt",
    "SpecialistComment",

    "SerialNumber",
    "Notes"
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

                if (value is DateTime date)
                {
                    row[i] =
                        date.ToString(
                            "dd.MM.yyyy");
                }
                else
                {
                    row[i] =
                        value?.ToString()
                        ?? "";
                }
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
    private void ConfigureRequestColumns()
    {
        gridReport.Columns.Clear();

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Номер заявки",
                Binding =
                    new Binding("RequestNumber"),
                Width = 150
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Кабинет",
                Binding =
                    new Binding("RoomNumber"),
                Width = 100
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Заявитель",
                Binding =
                    new Binding("ApplicantName"),
                Width = 220
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Телефон",
                Binding =
                    new Binding("ApplicantPhone"),
                Width = 140
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Тип заявки",
                Binding =
                    new Binding("RequestTypeName"),
                Width = 180
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Описание проблемы",
                Binding =
                    new Binding("ProblemDescription"),
                Width = 300
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Статус",
                Binding =
                    new Binding("StatusName"),
                Width = 120
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Дата создания",
                Binding =
                    new Binding("CreatedAt")
                    {
                        StringFormat =
                            "dd.MM.yyyy"
                    },
                Width = 130
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Исполнитель",
                Binding =
                    new Binding("ExecutorName"),
                Width = 220
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Комментарий специалиста",
                Binding =
                    new Binding("SpecialistComment"),
                Width = 250
            });
    }
    private void ConfigureSpecialistColumns()
    {
        gridReport.Columns.Clear();

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Специалист",
                Binding =
                    new Binding(
                        "SpecialistName"),
                Width = 350
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Выполнено заявок",
                Binding =
                    new Binding(
                        "CompletedRequests"),
                Width = 180
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Активных заявок",
                Binding =
                    new Binding(
                        "ActiveRequests"),
                Width = 180
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Среднее время (ч)",
                Binding =
                    new Binding(
                        "AverageHours"),
                Width = 180
            });
    }
    private void ConfigureEquipmentColumns()
    {
        gridReport.Columns.Clear();

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Инвентарный номер",
                Binding =
                    new Binding(
                        "InventoryNumber"),
                Width = 180
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Оборудование",
                Binding =
                    new Binding(
                        "EquipmentName"),
                Width = 250
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Модель",
                Binding =
                    new Binding(
                        "Model"),
                Width = 180
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Производитель",
                Binding =
                    new Binding(
                        "Manufacturer"),
                Width = 180
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Кабинет",
                Binding =
                    new Binding(
                        "RoomNumber"),
                Width = 120
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Ответственный",
                Binding =
                    new Binding(
                        "UserName"),
                Width = 220
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Дата ввода",
                Binding =
                    new Binding(
                        "CommissioningDate")
                    {
                        StringFormat =
                            "dd.MM.yyyy"
                    },
                Width = 130
            });

        gridReport.Columns.Add(
            new DataGridTextColumn
            {
                Header =
                    "Состояние",
                Binding =
                    new Binding(
                        "StateName"),
                Width = 150
            });
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
        switch (currentReport)
        {
            case "Отчет по заявкам":

                ConfigureRequestColumns();

                LoadRequestsReport();

                break;

            case "Выполненные заявки":

                ConfigureRequestColumns();

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

                break;

            case "Отчет по специалистам":

                ConfigureSpecialistColumns();

                gridReport.ItemsSource =
                    repository.GetSpecialists();

                break;

            case "Отчет по оборудованию":

                ConfigureEquipmentColumns();

                gridReport.ItemsSource =
                    equipmentRepository.GetAll();

                break;
        }
    }

    private void btnRequests_Click(
        object sender,
        RoutedEventArgs e)
    {
        currentReport =
            "Отчет по заявкам";
        ConfigureRequestColumns();
        LoadRequestsReport();
    }

    private void btnCompleted_Click(
     object sender,
     RoutedEventArgs e)
    {
        currentReport =
            "Выполненные заявки";

        ConfigureRequestColumns();

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

        ConfigureSpecialistColumns();

        gridReport.ItemsSource =
            repository.GetSpecialists();
    }

    private void btnEquipment_Click(
        object sender,
        RoutedEventArgs e)
    {
        currentReport =
            "Отчет по оборудованию";

        ConfigureEquipmentColumns();

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

   

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}