using ClassLibrary1;
using Microsoft.EntityFrameworkCore;
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
        private readonly ServiceCenterDbContext _dbContext;

        public ICommand CompletedWorksReportCommand { get; private set; }

        public ICommand TimeSpentReportCommand { get; private set; }

        public ICommand DetailsUsageReportCommand { get; private set; }

        public ReportViewModel(ServiceCenterDbContext dbContext) 
        {
            _dbContext = dbContext;
            CompletedWorksReportCommand = new MyCommand(CreateCompletedWorksReport);
            TimeSpentReportCommand = new MyCommand(CreateTimeSpentReport);
            DetailsUsageReportCommand = new MyCommand(CreateDetailsUsageReport);
            Task.Run(async () => {
                CompletedWorks = await GetWorkViewAsync();
                DetailsViews = await GetDetailsViewsAsync();
            });
        }

        private async Task<List<WorkView>> GetWorkViewAsync()
        {
            var worksList = await (
                from works in _dbContext.Works
                join types in _dbContext.WorkTypes
                on works.WorkTypeId equals types.Id
                where works.StatusId == 3 && (works.StartDate.Month == DateTime.Now.Month || works.EndDate.Value.Month == DateTime.Now.Month)
                select new WorkView
                {
                    Name = works.Name,                   
                    Description = works.Description ?? "",
                    StartDate = works.StartDate,
                    EndDate = works.EndDate ?? DateTime.UtcNow,
                    TotalCost = works.TotalCost,
                    TypeName = types.Type
                }
                ).ToListAsync();
            return worksList;
        }

        private async Task<List<DetailsView>> GetDetailsViewsAsync()
        {
            var detailsList = await (
                from workDetails in _dbContext.WorkDetails
                join works in _dbContext.Works
                on workDetails.WorkId equals works.Id
                join details in _dbContext.Details
                on workDetails.DetailId equals details.Id
                where works.StartDate.Month == DateTime.Now.Month || works.EndDate.Value.Month == DateTime.Now.Month
                select new DetailsView
                {
                    WorkName = works.Name,
                    DetailName = details.Name,
                    DetailPrice = details.Price,
                    DetailCount = workDetails.Count,
                    TotalCost = (workDetails.Count * details.Price)
                }
                ).ToListAsync();
            return detailsList;
        }

        private async Task<List<EmployeeEfficiency>> GetEmployeeEfficiency()
        {
            var list = await (
                from userWorks in _dbContext.UserWorks
                join works in _dbContext.Works
                on userWorks.WorkId equals works.Id
                join employees in _dbContext.Employees
                on userWorks.EmployeeId equals employees.Id
                where works.StartDate.Month == DateTime.Now.Month || works.EndDate.Value.Month == DateTime.Now.Month
                select new EmployeeEfficiency
                {
                    EmployeeLogin = employees.Login,
                    CompletedWorks = 1
                }
                ).ToListAsync();

            var groupList = list.GroupBy( x => x.EmployeeLogin );


            
            return list;
        }

        record class Person(string Name, string Company);

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

        private List<DetailsView> DetailsViews { get; set; }

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

        public void CreateDetailsUsageReport()
        {
            var path = GetPath("отчет об использовании запасных частей");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);

            var head = docX.InsertParagraph($"Отчет об использовании запасных частей");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(DetailsViews.Count + 1, 8);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs.First().Append("Название работы");
            table.Rows[0].Cells[1].Paragraphs.First().Append("Название детали");
            table.Rows[0].Cells[2].Paragraphs.First().Append("Цена детали");
            table.Rows[0].Cells[3].Paragraphs.First().Append("Кол-во деталей");
            table.Rows[0].Cells[4].Paragraphs.First().Append("Сумма");

            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                table.Rows[i + 1].Cells[0].Paragraphs.First().Append(DetailsViews[i].WorkName);
                table.Rows[i + 1].Cells[1].Paragraphs.First().Append(DetailsViews[i].DetailName);
                table.Rows[i + 1].Cells[2].Paragraphs.First().Append(DetailsViews[i].DetailPrice.ToString());
                table.Rows[i + 1].Cells[3].Paragraphs.First().Append(DetailsViews[i].DetailCount.ToString());
                table.Rows[i + 1].Cells[4].Paragraphs.First().Append(DetailsViews[i].TotalCost.ToString());
            }

            docX.InsertTable(table);

            docX.Save();
        }
    }
}
