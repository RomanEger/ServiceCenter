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
            Task.Run(async () => 
            { 
                Works = await GetWorks();
                Clients = await GetClients();
                WorkTypes = await GetWorkTypes();
            });
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

        private async Task<IEnumerable<Work>> GetWorks() =>
            await _dbContext.Works.ToListAsync();

        private async Task<IEnumerable<string>> GetClients() =>
            await _dbContext.Clients.Select(x => x.Login + " | " + x.PhoneNumber).ToListAsync();

        private async Task<IEnumerable<string>> GetWorkTypes() =>
            await _dbContext.WorkTypes.Select(x => x.Type).ToListAsync();

        private async void AddWork()
        {
            if (string.IsNullOrWhiteSpace(SelectedWork.Name))
            {
                MessageBox.Show("Имя не указано!");
                return;
            }

            var arr = Client.Split(" | ");
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Login == arr[0]);

            if(client is null)
            {
                MessageBox.Show("Клиент не найден");
                return;
            }

            var workType = await _dbContext.WorkTypes.FirstOrDefaultAsync(x => x.Type == WorkType);
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

            await _dbContext.Works.AddAsync(SelectedWork);
            await _dbContext.SaveChangesAsync();
        }

        private async void DeleteWork()
        {
            Works.ToList().Remove(SelectedWork);
            _dbContext.Works.Remove(SelectedWork);
            await _dbContext.SaveChangesAsync();
        }

        public ICommand AddWorkCommand { get; private set; }
        
        public ICommand DeleteCommand { get; private set; }

        //можно добавить фильтры по статусу и тд
    }
}
