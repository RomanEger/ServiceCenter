using System.Windows.Forms;
using ClassLibrary1;
using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using ServiceCenterApp.ReportViews;
using System.Windows.Input;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ServiceCenterApp.ViewModels;

public class ReportViewModel : ViewModelBase
{
    private readonly ServiceCenterDbContext _dbContext;

    public ICommand AnalyticsCommand {  get; private set; }

    public ICommand CompletedWorksReportCommand { get; private set; }

    public ICommand TimeSpentReportCommand { get; private set; }

    public ICommand DetailsUsageReportCommand { get; private set; }

    public ReportViewModel(ServiceCenterDbContext dbContext) 
    {
        _dbContext = dbContext;
        CompletedWorksReportCommand = new MyCommand(CreateCompletedWorksReport);
        TimeSpentReportCommand = new MyCommand(CreateTimeSpentReport);
        DetailsUsageReportCommand = new MyCommand(CreateDetailsUsageReport);
        AnalyticsCommand = new MyCommand(CreateAnalyticsReport);
        Task.Run(async () => {
            CompletedWorks = await GetWorkViewAsync();
            DetailsViews = await GetDetailsViewsAsync();
            AnalyticsViews = await GetAnalyticsDictAsync();
        });
    }

    private async Task<List<AnalyticsView>> GetAnalyticsDictAsync()
    {
        var list = await (
            from userWorks in _dbContext.UserWorks
            join users in _dbContext.Employees
                on userWorks.EmployeeId equals users.Id
            join works in _dbContext.Works
                on userWorks.WorkId equals works.Id
            where works.EndDate.Value.Month == DateTime.Now.Month && works.StatusId == 3
            select new
            {
                users.Login,
                works.TotalCost
            }).ToListAsync();

        var employees = list.GroupBy(x => x.Login);
            
        var resultList = new List<AnalyticsView>();
            
        foreach(var employee in employees)
        {
            decimal sum = 0;
            foreach(var emp in employee) 
            {
                sum += emp.TotalCost;
            }
            resultList.Add(new AnalyticsView() { Login = employee.Key, Sum = sum});
        }
        return resultList;
    }

    private async Task<List<WorkView>> GetWorkViewAsync()
    {
        var worksList = await (
            from works in _dbContext.Works
            join types in _dbContext.WorkTypes
                on works.WorkTypeId equals types.Id
            where works.StatusId == 3 && works.EndDate.Value.Month == DateTime.Now.Month
            select new WorkView
            {
                Name = works.Name,                   
                Description = works.Description ?? "",
                StartDate = works.StartDate,
                EndDate = works.EndDate.Value,
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

    private List<AnalyticsView> AnalyticsViews { get; set; }

    private string GetPath(string fileName)
    {
        using var openFolder = OpenFolder.CreateFolderDialog();

        openFolder.ShowDialog();

        var now = DateTime.Now.ToString("g").Replace('/', '_').Replace(':', '_');

        return openFolder.SelectedPath + @$"\{fileName}_{now}.docx";
    }

    public void CreateCompletedWorksReport()
    {
        try
        {
            var path = GetPath("отчет о выполненных работах");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);

            var month = (Month)DateTime.Now.Month;
            
            var head = docX.InsertParagraph($"Отчет о выполненных работах за {month}");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(CompletedWorks.Count + 1, 6);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs.First().Append("Название");
            table.Rows[0].Cells[1].Paragraphs.First().Append("Тип работы");
            table.Rows[0].Cells[2].Paragraphs.First().Append("Стоимость");
            table.Rows[0].Cells[3].Paragraphs.First().Append("Начало");
            table.Rows[0].Cells[4].Paragraphs.First().Append("Окончание");
            table.Rows[0].Cells[5].Paragraphs.First().Append("Описание");

            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                var totalCost = $"{CompletedWorks[i].TotalCost:f2}";
                table.Rows[i + 1].Cells[0].Paragraphs.First().Append(CompletedWorks[i].Name);
                table.Rows[i + 1].Cells[1].Paragraphs.First().Append(CompletedWorks[i].TypeName);
                table.Rows[i + 1].Cells[2].Paragraphs.First().Append(totalCost);
                table.Rows[i + 1].Cells[3].Paragraphs.First().Append(CompletedWorks[i].StartDate.ToString("f"));
                table.Rows[i + 1].Cells[4].Paragraphs.First().Append(CompletedWorks[i].EndDate.ToString("f"));
                table.Rows[i + 1].Cells[5].Paragraphs.First().Append(CompletedWorks[i].Description);
            }

            docX.InsertTable(table);

            docX.Save();
            MessageBox.Show($"Отчет сохранен по адресу {path}");
        }
        catch
        {
            MessageBox.Show("Не удалось сохранить отчет");
        }
        
    }

    public void CreateTimeSpentReport()
    {
        try
        {
            var path = GetPath("отчет о затраченном времени");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);

            var month = (Month)DateTime.Now.Month;
            
            var head = docX.InsertParagraph($"Отчет о затраченном времени за {month}");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(CompletedWorks.Count + 1, 2);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs.First().Append("Название");
            table.Rows[0].Cells[1].Paragraphs.First().Append("Времени затрачено (минут)");

            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                var timeSpent = (CompletedWorks[i].EndDate - CompletedWorks[i].StartDate).TotalMinutes;
                var time = Math.Ceiling(timeSpent);
                table.Rows[i + 1].Cells[0].Paragraphs.First().Append(CompletedWorks[i].Name);
                table.Rows[i + 1].Cells[1].Paragraphs.First().Append(time.ToString());
            }

            docX.InsertTable(table);

            docX.Save();
            MessageBox.Show($"Отчет сохранен по адресу {path}");
        }
        catch
        {
            MessageBox.Show("Не удалось сохранить отчет");
        }
        
    }

    public void CreateDetailsUsageReport()
    {
        try
        {
            var path = GetPath("отчет об использовании запасных частей");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);
            
            var month = (Month)DateTime.Now.Month;
            
            var head = docX.InsertParagraph($"Отчет об использовании запасных частей за {month}");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(DetailsViews.Count + 1, 5);
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
                table.Rows[i + 1].Cells[2].Paragraphs.First().Append(DetailsViews[i].DetailPrice.ToString("f2"));
                table.Rows[i + 1].Cells[3].Paragraphs.First().Append(DetailsViews[i].DetailCount.ToString());
                table.Rows[i + 1].Cells[4].Paragraphs.First().Append(DetailsViews[i].TotalCost.ToString("f2"));
            }

            docX.InsertTable(table);

            docX.Save();
            MessageBox.Show($"Отчет сохранен по адресу {path}");
        }
        catch
        {
            MessageBox.Show("Не удалось сохранить отчет");
        }
        
    }

    public void CreateAnalyticsReport()
    {
        try
        {
            var month = (Month)DateTime.Now.Month;
            
            var path = GetPath("анализ эффективности работы сотрудников");

            using var docX = IsFileExist ? DocX.Load(path) : DocX.Create(path);
            
            var head = docX.InsertParagraph($"Анализ эффективности работы сотрудников за {month}");

            head.Alignment = Alignment.center;
            head.FontSize(14);
            head.Font("TimesNewRoman");

            docX.InsertParagraph();

            var table = docX.AddTable(AnalyticsViews.Count + 1, 4);
            table.Alignment = Alignment.center;

            table.Rows[0].Cells[0].Paragraphs.First().Append("Работник");
            table.Rows[0].Cells[1].Paragraphs.First().Append("Сделано работ на сумму");
            table.Rows[0].Cells[2].Paragraphs.First().Append("Цель");
            table.Rows[0].Cells[3].Paragraphs.First().Append("Эффективность (%)");

            const decimal purpose = 100000;

            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                table.Rows[i + 1].Cells[0].Paragraphs.First().Append(AnalyticsViews[i].Login);
                table.Rows[i + 1].Cells[1].Paragraphs.First().Append(AnalyticsViews[i].Sum.ToString("f2"));
                table.Rows[i + 1].Cells[2].Paragraphs.First().Append(purpose.ToString());
                table.Rows[i + 1].Cells[3].Paragraphs.First().Append((AnalyticsViews[i].Sum * 100 / purpose).ToString("f2"));
            }

            docX.InsertTable(table);

            docX.Save();
            MessageBox.Show($"Отчет сохранен по адресу {path}");
        }
        catch
        {
            MessageBox.Show("Не удалось сохранить отчет");
        }
        
    }
    
    private enum Month
    {
        Январь = 1,
        Февраль,
        Март,
        Апрель,
        Май,
        Июнь,
        Июль,
        Август,
        Сентябрь,
        Октябрь,
        Ноябрь,
        Декабрь
    }
}