using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TSMS_2_.Model;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using TSMS_2_.Services;
using System.IO;
using PdfSharp.Charting;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using OxyPlot.Series;
using OxyPlot;
namespace TSMS_2_.ViewModel
{
    public class SalesReportViewModel : INotifyPropertyChanged
    {

        private readonly TableModel _tableModel = new TableModel();
        private ObservableCollection<SalesReport> _salesReports;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _categoryFilter;
        private string _fileName;

        // Команды
        public ICommand GenerateReportCommand { get; }
        public ICommand TotalSumCommand { get; }
        public ICommand SaveReportAsPdfCommand { get; }

        // Событие для изменения свойств
        public event PropertyChangedEventHandler PropertyChanged;
        public SalesReportViewModel()
        {
            _salesReports = new ObservableCollection<SalesReport>();
            _startDate = DateTime.Now.AddMonths(-1);
            _endDate = DateTime.Now;
            _categoryFilter = string.Empty;
            // Инициализация команд
            GenerateReportCommand = new RelayCommand(GenerateReport);
            SaveReportAsPdfCommand = new RelayCommand(SaveReportAsPdf);
        }
        private long _totalSum;
        public long TotalSum
        {
            get => _totalSum;
            set => SetProperty(ref _totalSum, value);
        }

        

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Свойства
        public ObservableCollection<SalesReport> SalesReports
        {
            get { return _salesReports; }
            set { SetProperty(ref _salesReports, value); }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        public string CategoryFilter
        {
            get { return _categoryFilter; }
            set { SetProperty(ref _categoryFilter, value); }
        }

        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }
        private Dictionary<long, long> _revenueByCategories;

        public Dictionary<long, long> RevenueByCategories
        {
            get => _revenueByCategories;
            set
            {
                _revenueByCategories = value;
                OnPropertyChanged(nameof(RevenueByCategories));
            }
        }
        // Генерация отчета
        private PlotModel _pieChartModel;
        public PlotModel PieChartModel
        {
            get => _pieChartModel;
            set
            {
                _pieChartModel = value;
                OnPropertyChanged(); // Для обновления интерфейса
            }
        }
        private decimal _averageCheck;

        public decimal AverageCheck
        {
            get => _averageCheck;
            set => SetProperty(ref _averageCheck, value);
        }
        private long _clientCheck;

        public long ClientCheck
        {
            get => _clientCheck;
            set => SetProperty(ref _clientCheck, value);
        }

        public void GenerateReport()
        {
            TotalSum = _tableModel.GetTotalRevenue(StartDate, EndDate);
            RevenueByCategories = _tableModel.GetTotalRevenueByCategories(StartDate, EndDate);

            // Получаем количество продаж
            ClientCheck=_tableModel.GetUniqueClientsCount(StartDate, EndDate);
            // Рассчитываем средний чек
            if (ClientCheck > 0)
            {
                AverageCheck = TotalSum / ClientCheck;
            }
            else
            {
                AverageCheck = 0; // Если нет продаж, средний чек = 0
            }

            SalesReports.Clear();  // Очищаем старые данные

            // Добавляем данные по категориям
            foreach (var categoryRevenue in RevenueByCategories)
            {
                string categoryName = _tableModel.GetCategoryName(categoryRevenue.Key);  // Получаем название категории по ее ID
                SalesReports.Add(new SalesReport
                {
                    Category = categoryName,  // Название категории
                    TotalRevenue = categoryRevenue.Value  // Сумма для этой категории
                });
            }

            // Построение диаграммы
            PieChartModel = new PlotModel { Title = "Отчет по категориям" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 1.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            // Добавляем данные по категориям в диаграмму
            foreach (var categoryRevenue in RevenueByCategories)
            {
                string categoryName = _tableModel.GetCategoryName(categoryRevenue.Key); // Название категории
                double revenue = categoryRevenue.Value;

                pieSeries.Slices.Add(new PieSlice(categoryName, revenue)
                {
                    IsExploded = true, // Опционально: выделение сегмента
                    Fill = OxyColor.FromHsv(new Random().NextDouble(), 0.8, 0.9) // Генерация случайного цвета
                });
            }

            PieChartModel.Series.Add(pieSeries);

            // Уведомляем интерфейс о новом графике
            OnPropertyChanged(nameof(PieChartModel));
            OnPropertyChanged(nameof(AverageCheck));  // Обновляем интерфейс для среднего чека
        }



        // Сохранение отчета в PDF
        public void SaveReportAsPdf()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                MessageBox.Show("Please enter a file name.");
                return;
            }

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = FileName  // По умолчанию будет предложено имя файла
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Создаем PDF-документ
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12);

                double yPosition = 20;

                // Сначала выводим общую сумму
                gfx.DrawString($"Общая сумма продаж: {TotalSum}",
                    font, XBrushes.Black, new XRect(20, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += 20;

                // Добавляем средний чек
                gfx.DrawString($"Средний чек: {AverageCheck:C}",
                    font, XBrushes.Black, new XRect(20, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += 20;

                gfx.DrawString($"Количество клиентов: {ClientCheck:C}",
                   font, XBrushes.Black, new XRect(20, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += 20;

                // Далее выводим данные по категориям
                foreach (var report in SalesReports)
                {
                    gfx.DrawString($"{report.Category} | {report.TotalRevenue}",
                        font, XBrushes.Black, new XRect(20, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += 20;
                }

                // Добавляем диаграмму
                if (PieChartModel != null)
                {
                    var exporter = new OxyPlot.SkiaSharp.PdfExporter
                    {
                        Width = (int)page.Width,  // Ширина страницы PDF
                        Height = 300  // Высота диаграммы
                    };

                    using (var stream = new MemoryStream())
                    {
                        // Экспорт диаграммы в поток
                        exporter.Export(PieChartModel, stream);
                        stream.Seek(0, SeekOrigin.Begin);

                        // Загружаем изображение из потока
                        XImage image = XImage.FromStream(stream);  // Передаем поток напрямую

                        // Рисуем изображение на странице PDF
                        gfx.DrawImage(image, 20, yPosition, page.Width - 40, 300);
                    }

                }

                // Сохраняем PDF-документ
                document.Save(saveFileDialog.FileName);
                MessageBox.Show("Report saved successfully.");
            }
        }

        // Командный класс для реализации ICommand
        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

            public void Execute(object parameter) => _execute();

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    }
}
