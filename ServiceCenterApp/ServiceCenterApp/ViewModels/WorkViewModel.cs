using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            });
            AddWorkCommand = new MyCommand(AddWork);
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

        private async Task<IEnumerable<Work>> GetWorks() =>
            await _dbContext.Works.ToListAsync();

        private async void AddWork()
        {
            await _dbContext.Works.AddAsync(SelectedWork);
            await _dbContext.SaveChangesAsync();
        }

        public ICommand AddWorkCommand { get; private set; }
        
        //можно добавить фильтры по статусу и тд
    }
}
