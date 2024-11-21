//using System;
//using System.Collections.ObjectModel;
//using System.IO;
//using System.Windows;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Xml.Linq;
//using TSMS_2_.Model;
//using TSMS_2_.ViewModel;

//namespace SalesApp
//{
//    public class SalesReportViewModel 
//    {
//        private ObservableCollection<SalesReport> _salesReports;
//        private DateTime _startDate;
//        private DateTime _endDate;
//        private string _categoryFilter;
//        private string _fileName;

//        public ObservableCollection<SalesReport> SalesReports
//        {
//            get { return _salesReports; }
//            set { SetProperty(ref _salesReports, value); }
//        }

//        public DateTime StartDate
//        {
//            get { return _startDate; }
//            set { SetProperty(ref _startDate, value); }
//        }

//        public DateTime EndDate
//        {
//            get { return _endDate; }
//            set { SetProperty(ref _endDate, value); }
//        }

//        public string CategoryFilter
//        {
//            get { return _categoryFilter; }
//            set { SetProperty(ref _categoryFilter, value); }
//        }

//        public string FileName
//        {
//            get { return _fileName; }
//            set { SetProperty(ref _fileName, value); }
//        }

//        public SalesReportViewModel()
//        {
//            _salesReports = new ObservableCollection<SalesReport>();
//            _startDate = DateTime.Now.AddMonths(-1); // по умолчанию месяц назад
//            _endDate = DateTime.Now; // по умолчанию текущая дата
//            _categoryFilter = string.Empty;
//        }

//        public void GenerateReport()
//        {
//            // Пример: фильтруем отчеты по датам и категории
//            var filteredReports = GetSalesData()
//                .Where(r => r.Date >= _startDate && r.Date <= _endDate)
//                .ToList();

//            if (!string.IsNullOrEmpty(_categoryFilter))
//            {
//                filteredReports = filteredReports.Where(r => r.Category.Equals(_categoryFilter, StringComparison.OrdinalIgnoreCase)).ToList();
//            }

//            SalesReports.Clear();
//            foreach (var report in filteredReports)
//            {
//                SalesReports.Add(report);
//            }
//        }

//        public void SaveReportAsPdf()
//        {
//            if (string.IsNullOrEmpty(FileName))
//            {
//                MessageBox.Show("Please enter a file name.");
//                return;
//            }

//            // Сохраняем PDF
//            PdfDocument document = new PdfDocument();
//            PdfPage page = document.AddPage();
//            XGraphics gfx = XGraphics.FromPdfPage(page);
//            XFont font = new XFont("Arial", 12);

//            double yPosition = 20;

//            foreach (var report in SalesReports)
//            {
//                gfx.DrawString($"{report.Category} | {report.Date.ToShortDateString()} | {report.Amount} | {report.Quantity}",
//                    font, XBrushes.Black, new XRect(20, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
//                yPosition += 20;
//            }

//            document.Save(FileName);
//            MessageBox.Show("Report saved successfully.");
//        }

//        private List<SalesReport> GetSalesData()
//        {
//            // Здесь имитация получения данных, можно подключить к БД
//            return new List<SalesReport>
//            {
//                new SalesReport { Category = "Electronics", Date = DateTime.Now, Amount = 1500, Quantity = 5 },
//                new SalesReport { Category = "Clothing", Date = DateTime.Now.AddDays(-1), Amount = 500, Quantity = 2 },
//                new SalesReport { Category = "Groceries", Date = DateTime.Now.AddDays(-3), Amount = 300, Quantity = 10 }
//            };
//        }
//    }
//}
