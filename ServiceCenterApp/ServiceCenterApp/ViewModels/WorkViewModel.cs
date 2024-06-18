using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using ServiceCenterApp.ReportViews;
using ServiceCenterApp.Views;

using System.Windows;
using System.Windows.Input;

namespace ServiceCenterApp.ViewModels
{
    public class WorkViewModel : ViewModelBase
    {
        private readonly ServiceCenterDbContext _dbContext;

        public WorkViewModel(ServiceCenterDbContext dbContext) 
        { 
            _dbContext = dbContext;
            Works = GetWorks();
            Clients = GetClients();
            WorkTypes = GetWorkTypes();
            StockDetails = GetStockDetailViews();
            WorkDetails = GetWorkDetails();
            Details = GetDetails();
            StockDetailsCmb = GetStockDetails();
            Stocks = GetStocks();
            AddWorkCommand = new MyCommand(AddWork);
            DeleteCommand = new MyCommand(DeleteWork);
            UpdateCommand = new MyCommand(UpdateWork);
            DeleteFromWorkCommand = new MyCommand(DeleteFromWork);
            AddDetailWorkCommand = new MyCommand(AddDetailWork);
        }

        private UserWork _userWork = new UserWork();

        private IEnumerable<Work> _works = new List<Work>();
        
        public IEnumerable<Work> Works 
        {
            get => _works;
            set
            {
                _works = value;
                OnPropertyChanged();
            }
        }

        private Work _selectedWork = new Work();

        public Work SelectedWork
        {
            get => _selectedWork;
            set
            {
                _selectedWork = value;
                StockDetailsByWork = GetStockDetailViewsByWork();
                OnPropertyChanged();
            }
        }

        private string _selectedClient;

        public string SelectedClient
        {
            get => _selectedClient ?? _dbContext.Clients.Where(x => x.Id == SelectedWork.ClientId)
                .Select(x => x.Login + " | " + x.PhoneNumber).FirstOrDefault() ?? "";
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        private string _selectedWorkType;

        public string SelectedWorkType
        {
            get => _selectedWorkType ?? _dbContext.WorkTypes.Where(x => x.Id == SelectedWork.WorkTypeId).Select(x => x.Type)
                .FirstOrDefault() ?? "";
            set
            {
                _selectedWorkType = value;
                OnPropertyChanged();
            }
        }
        
        private IEnumerable<StockDetailView> _stockDetailsByWork = new List<StockDetailView>();

        public IEnumerable<StockDetailView> StockDetailsByWork
        {
            get => _stockDetailsByWork;
            set
            {
                _stockDetailsByWork = value;
                OnPropertyChanged();
            }
        }
        
        private Detail _selectedDetail = new Detail();
        
        public Detail SelectedDetail
        {
            get => _selectedDetail;
            set
            {
                _selectedDetail = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Detail> _details = new List<Detail>();

        public IEnumerable<Detail> Details
        {
            get => _details;
            set
            {
                _details = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<WorkDetail> _workDetails = new List<WorkDetail>();

        public IEnumerable<WorkDetail> WorkDetails
        {
            get => _workDetails;
            set
            {
                _workDetails = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<StockDetailView> _stockDetails = new List<StockDetailView>();

        public IEnumerable<StockDetailView> StockDetails
        {
            get => _stockDetails;
            set
            {
                _stockDetails = value;
                OnPropertyChanged();
            }
        }

        private StockDetailView _selectedStockDetail = new StockDetailView();

        public StockDetailView SelectedStockDetail
        {
            get => _selectedStockDetail;
            set
            {
                _selectedStockDetail = value;
                OnPropertyChanged();
            }
        }

        private string _client;
        public string Client
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string> _clients;

        public IEnumerable<string> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        private string _workType;

        public string WorkType
        {
            get => _workType;
            set
            {
                _workType = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string> _workTypes;

        public IEnumerable<string> WorkTypes
        {
            get => _workTypes;
            set
            {
                _workTypes = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string> _stockDetailsCmb;

        public IEnumerable<string> StockDetailsCmb
        {
            get => _stockDetailsCmb;
            set
            {
                _stockDetailsCmb = value;
                OnPropertyChanged();
            }
        }

        private string _selectedStockDetailCmb;

        private StockDetail _stockDetail { get; set; }
        
        public string SelectedStockDetailCmb
        {
            get => _selectedStockDetailCmb;
            set
            {
                _selectedStockDetailCmb = value;
                _stockDetail = (
                        from sd in _dbContext.StockDetails
                        join d in _dbContext.Details on sd.DetailId equals d.Id
                        join s in _dbContext.Stocks on sd.StockId equals s.Id
                        where d.Name == value && s.Name == SelectedStock
                        select sd).FirstOrDefault() ?? new StockDetail();
                Stocks = GetStocks();
                OnPropertyChanged();
            }
        }

        private int _detailCount;

        public int DetailCount
        {
            get => _detailCount;
            set
            {
                if (value > _stockDetail.CountDetail)
                {
                    MessageBox.Show($"Всего деталей: {_stockDetail.CountDetail}");
                    return;
                }
                _detailCount = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<string> _stocks;

        public IEnumerable<string> Stocks
        {
            get => _stocks;
            set
            {
                _stocks = value;
                OnPropertyChanged();
            }
        }

        private string _selectedStock;

        public string SelectedStock
        {
            get => _selectedStock;
            set
            {
                _selectedStock = value;
                _stockDetail = (
                    from sd in _dbContext.StockDetails
                    join d in _dbContext.Details on sd.DetailId equals d.Id
                    join s in _dbContext.Stocks on sd.StockId equals s.Id
                    where d.Name == SelectedStockDetailCmb && s.Name == value
                    select sd).FirstOrDefault() ?? new StockDetail();
                StockDetailsCmb = GetStockDetails();
                OnPropertyChanged();
            }
        }
        
        private IEnumerable<string> GetStocks()
        {
            if(SelectedStockDetailCmb != null)
                return (from s in _dbContext.Stocks
                    join sd in _dbContext.StockDetails
                        on s.Id equals sd.StockId
                    join d in _dbContext.Details on sd.DetailId equals d.Id
                    where d.Name == SelectedStockDetailCmb
                    select s.Name).Distinct().ToList();
            return 
                (from s in _dbContext.Stocks
                join sd in _dbContext.StockDetails
                    on s.Id equals sd.StockId
                select s.Name).Distinct().ToList();
        }
        
        private IEnumerable<string> GetStockDetails()
        {
            if (SelectedStock != null)
                return (from sd in _dbContext.StockDetails
                    join d in _dbContext.Details
                        on sd.DetailId equals d.Id
                    join s in _dbContext.Stocks on sd.StockId equals s.Id
                    where s.Name == SelectedStock
                    select d.Name).Distinct().ToList();
            
            return (from sd in _dbContext.StockDetails
                    join d in _dbContext.Details
                        on sd.DetailId equals d.Id
                    select d.Name).Distinct().ToList();
        }
        
        private IEnumerable<Work> GetWorks() =>
            _dbContext.Works.ToList();

        private IEnumerable<string> GetClients() =>
            _dbContext.Clients.Select(x => x.Login + " | " + x.PhoneNumber).ToList();

        private IEnumerable<string> GetWorkTypes() =>
            _dbContext.WorkTypes.Select(x => x.Type).ToList();

        private IEnumerable<StockDetailView> GetStockDetailViews()
        {
            var list = (
                   from stockDetails in _dbContext.StockDetails
                   join stocks in _dbContext.Stocks
                   on stockDetails.StockId equals stocks.Id
                   join details in _dbContext.Details
                   on stockDetails.DetailId equals details.Id
                   select new StockDetailView
                   {
                       Id = stockDetails.Id,
                       StockName = stocks.Name,
                       DetailName = details.Name,
                       Count = stockDetails.CountDetail
                   }).ToList();
            return list;
        }

        private IEnumerable<StockDetailView> GetStockDetailViewsByWork()
        {
             var list = (from wd in _dbContext.WorkDetails
                join w in _dbContext.Works on wd.WorkId equals w.Id
                join d in _dbContext.Details on wd.DetailId equals d.Id
                where wd.WorkId == SelectedWork.Id
                select new StockDetailView
                {
                    DetailName = d.Name,
                    Count = wd.Count
                }).ToList();
            // var list = (
            //     from stockDetails in _dbContext.StockDetails
            //     join stocks in _dbContext.Stocks
            //         on stockDetails.StockId equals stocks.Id
            //     join details in _dbContext.Details
            //         on stockDetails.DetailId equals details.Id
            //     join workDetails in _dbContext.WorkDetails
            //         on stockDetails.DetailId equals workDetails.DetailId
            //     join works in _dbContext.Works 
            //         on workDetails.WorkId equals works.Id
            //     where workDetails.WorkId == SelectedWork.Id
            //     select new StockDetailView
            //     {
            //         Id = stockDetails.Id,
            //         StockName = stocks.Name,
            //         DetailName = details.Name,
            //         Count = stockDetails.CountDetail
            //     }).ToList();
            return list;
        }
        
        private IEnumerable<WorkDetail> GetWorkDetails() => 
            _dbContext.WorkDetails.Where(x => x.WorkId == SelectedWork.Id).ToList();

        private IEnumerable<Detail> GetDetails() =>
            (from detail in _dbContext.Details
             join stockDetail in _dbContext.StockDetails
             on detail.Id equals stockDetail.DetailId
             where stockDetail.CountDetail > 0
             select detail).ToList();

        private void AddWork()
        {
            if (string.IsNullOrWhiteSpace(SelectedWork.Name))
            {
                MessageBox.Show("Имя не указано!");
                return;
            }

            var arr = Client.Split(" | ");
            var client = _dbContext.Clients.FirstOrDefault(x => x.Login == arr[0]);

            if(client is null)
            {
                MessageBox.Show("Клиент не найден");
                return;
            }

            var workType = _dbContext.WorkTypes.FirstOrDefault(x => x.Type == WorkType);
            if (workType is null)
            {
                MessageBox.Show("Тип работ не найден");
                return;
            }

            SelectedWork.StatusId = 1;
            SelectedWork.WorkTypeId = workType.Id;
            SelectedWork.TotalCost = 0;
            SelectedWork.StartDate = DateTime.Now;
            SelectedWork.ClientId = client.Id;

            _dbContext.Works.Add(SelectedWork);
            _dbContext.SaveChanges();
            MessageBox.Show("Успешно");
            SelectedWork = new Work();
        }

        private void DeleteWork()
        {
            Works.ToList().Remove(SelectedWork);
            _dbContext.Works.Remove(SelectedWork);
            _dbContext.SaveChanges();
        }

        private bool _isDone;

        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                OnPropertyChanged();
            }
        }
        
        private void UpdateWork()
        {
            _userWork.EmployeeId = MainWindow.Employee.Id;
            _userWork.WorkId = SelectedWork.Id;
            
            if(!_dbContext.UserWorks.Any(x => x.WorkId == _userWork.WorkId))
                _dbContext.UserWorks.Add(_userWork);

            if (IsDone)
                SelectedWork.StatusId = 3;
            
            _dbContext.SaveChanges();
            MessageBox.Show("Успешно!");
        }

        private void DeleteFromWork()
        {
            var workDetail = (from workDetails in _dbContext.WorkDetails
                join stockDetails in _dbContext.StockDetails
                    on workDetails.DetailId equals stockDetails.DetailId
                where stockDetails.Id == SelectedStockDetail.Id && workDetails.WorkId == SelectedWork.Id
                select workDetails).FirstOrDefault();
            
            if (workDetail is null)
            {
                MessageBox.Show("Не удалось удалить деталь");
                return;
            }
            _dbContext.WorkDetails.Remove(workDetail);
            _dbContext.SaveChanges();
            StockDetailsByWork.ToList().Remove(SelectedStockDetail);
        }

        private void AddDetailWork()
        {
            var stock = _dbContext.Stocks.FirstOrDefault(x => x.Name == SelectedStock);
            var detail = _dbContext.Details.FirstOrDefault(x => x.Name == SelectedStockDetailCmb);
            if (stock == null || detail == null)
            {
                MessageBox.Show("Неудачно");
                return;
            }
            
            var stockDetail =
                _dbContext.StockDetails.FirstOrDefault(x => x.DetailId == detail.Id && x.StockId == stock.Id);
            if (stockDetail == null)
            {
                MessageBox.Show("Неудачно");
                return;
            }
            stockDetail.CountDetail -= DetailCount;
            
            var stockDetailView = new StockDetailView();
            stockDetailView.DetailName = SelectedStockDetailCmb;
            stockDetailView.StockName = SelectedStock;
            stockDetailView.Count = DetailCount;
            _stockDetailsByWork.ToList().Add(stockDetailView);

            var workDetail = new WorkDetail()
            {
                Count = DetailCount,
                DetailId = detail.Id,
                WorkId = SelectedWork.Id
            };
            
            _dbContext.WorkDetails.Add(workDetail);
            _dbContext.SaveChanges();
        }
        
        public ICommand AddWorkCommand { get; private set; }
        
        public ICommand DeleteCommand { get; private set; }

        public ICommand UpdateCommand { get; private set; }
        
        public ICommand DeleteFromWorkCommand { get; private set; }
        
        public ICommand AddDetailWorkCommand { get; private set; }
    }
}
