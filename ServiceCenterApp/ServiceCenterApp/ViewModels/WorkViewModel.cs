using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AddWorkCommand = new MyCommand(AddWork);
            DeleteCommand = new MyCommand(DeleteWork);
        }

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
            _dbContext.Works.ToList();

        private IEnumerable<string> GetClients() =>
            _dbContext.Clients.Select(x => x.Login + " | " + x.PhoneNumber).ToList();

        private IEnumerable<string> GetWorkTypes() =>
            _dbContext.WorkTypes.Select(x => x.Type).ToList();

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
        }

        private void DeleteWork()
        {
            Works.ToList().Remove(SelectedWork);
            _dbContext.Works.Remove(SelectedWork);
            _dbContext.SaveChanges();
        }

        public ICommand AddWorkCommand { get; private set; }
        
        public ICommand DeleteCommand { get; private set; }

        //можно добавить фильтры по статусу и тд
    }
}
