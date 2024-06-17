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
            AddWorkCommand = new MyCommand(AddWork);
            DeleteCommand = new MyCommand(DeleteWork);
            UpdateCommand = new MyCommand(UpdateWork);
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

        private IEnumerable<Work> GetWorks() =>
            _dbContext.Works;

        private IEnumerable<string> GetClients() =>
            _dbContext.Clients.Select(x => x.Login + " | " + x.PhoneNumber);

        private IEnumerable<string> GetWorkTypes() =>
            _dbContext.WorkTypes.Select(x => x.Type);

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

        private IEnumerable<WorkDetail> GetWorkDetails() => 
            _dbContext.WorkDetails.Where(x => x.WorkId == SelectedWork.Id);

        private IEnumerable<Detail> GetDetails() =>
            (from detail in _dbContext.Details
             join stockDetail in _dbContext.StockDetails
             on detail.Id equals stockDetail.DetailId
             where stockDetail.CountDetail > 0
             select detail);

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

        private void UpdateWork()
        {
            _userWork.EmployeeId = MainWindow.Employee.Id;
            _userWork.WorkId = SelectedWork.Id;
            
            _dbContext.UserWorks.Add(_userWork);
            
            
            
            
            _dbContext.SaveChanges();
        }

        public ICommand AddWorkCommand { get; private set; }
        
        public ICommand DeleteCommand { get; private set; }

        public ICommand UpdateCommand { get; private set; }
    }
}
