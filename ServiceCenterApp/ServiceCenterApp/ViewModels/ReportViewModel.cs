using ClassLibrary1;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using ServiceCenterApp.ReportViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ServiceCenterApp.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        public ICommand CompletedWorksReportCommand { get; private set; }

        public ICommand TimeSpentReportCommand { get; private set; }

        public ReportViewModel() 
        {
            CompletedWorksReportCommand = new MyCommand(CreateCompletedWorksReport);
            TimeSpentReportCommand = new MyCommand(CreateTimeSpentReport);
        }

        private bool _isFileExist;
        public bool IsFileExist 
        {
            get => _isFileExist;
            set
            {
                _isFileExist = value;
                OnPropertyChanged();
            }
        }

        private List<WorkView> CompletedWorks { get; set; }

        private string GetPath(string fileName)
        {
            using var openFolder = OpenFolder.CreateFolderDialog();

            openFolder.ShowDialog();

            var now = DateTime.Now.ToString("g").Replace('/', '_').Replace(':', '_');

            return openFolder.SelectedPath + @$"\{fileName}_{now}.docx";
        }

        public void CreateCompletedWorksReport()
        {
            var path = GetPath("отчет о выполненных работах");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);

            var head = docX.InsertParagraph($"Отчет о выполненных работах");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(CompletedWorks.Count + 1, 8);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs.First().Append("Название");
            table.Rows[0].Cells[1].Paragraphs.First().Append("Тип работы");
            table.Rows[0].Cells[2].Paragraphs.First().Append("Стоимость");
            table.Rows[0].Cells[3].Paragraphs.First().Append("Начало");
            table.Rows[0].Cells[4].Paragraphs.First().Append("Окончание");
            table.Rows[0].Cells[5].Paragraphs.First().Append("Описание");

            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                table.Rows[i + 1].Cells[0].Paragraphs.First().Append(CompletedWorks[i].Name);
                table.Rows[i + 1].Cells[1].Paragraphs.First().Append(CompletedWorks[i].TypeName);
                table.Rows[i + 1].Cells[2].Paragraphs.First().Append(CompletedWorks[i].TotalCost.ToString());
                table.Rows[i + 1].Cells[3].Paragraphs.First().Append(CompletedWorks[i].StartDate.ToString("f"));
                table.Rows[i + 1].Cells[4].Paragraphs.First().Append(CompletedWorks[i].EndDate.ToString("f"));
                table.Rows[i + 1].Cells[5].Paragraphs.First().Append(CompletedWorks[i].Description);
            }

            docX.InsertTable(table);

            docX.Save();
        }

        public void CreateTimeSpentReport()
        {
            var path = GetPath("отчет о затраченном времени");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);

            var head = docX.InsertParagraph($"Отчет о затраченном времени");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(CompletedWorks.Count + 1, 8);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs.First().Append("Название");
            table.Rows[0].Cells[1].Paragraphs.First().Append("Времени затрачено");

            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                var timeSpent = (CompletedWorks[i].EndDate - CompletedWorks[i].StartDate).TotalHours;
                table.Rows[i + 1].Cells[0].Paragraphs.First().Append(CompletedWorks[i].Name);
                table.Rows[i + 1].Cells[1].Paragraphs.First().Append(timeSpent.ToString());
            }

            docX.InsertTable(table);

            docX.Save();
        }
    }
}
