using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiceCenterApp.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly ServiceCenterDbContext _dbContext;
        public ClientViewModel(ServiceCenterDbContext dbContext) 
        {
            _dbContext = dbContext;
            RegistrationCommand = new MyCommand(ClientRegistration);
            SaveClientChangesCommand = new MyCommand(SaveChanges);
            DeleteClientCommand = new MyCommand(DeleteClient);
            Task.Run(async () => Clients = new ObservableCollection<Client>(await GetClients()));
        }

        private Client _client = new Client();

        public Client Client
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Client> Clients { get; set; }
        public ICommand RegistrationCommand { get; private set; }
        
        public ICommand SaveClientChangesCommand { get; private set; }

        public ICommand DeleteClientCommand { get; private set; }

        private async void SaveChanges() => await _dbContext.SaveChangesAsync();
        
        private async void DeleteClient()
        {
            Clients.ToList().Remove(Client);
            _dbContext.Clients.Remove(Client);
            await _dbContext.SaveChangesAsync();
        }

        private async void ClientRegistration()
        {
            if(Client.PhoneNumber == null)
            {
                MessageBox.Show("Некорректный номер телефона");
                return;
            }
            var isClientExists = await _dbContext.Clients.AnyAsync(c => c.Login == Client.Login || c.PhoneNumber == Client.PhoneNumber);
            if (isClientExists)
            {
                MessageBox.Show("Клиент уже зарегистрирован!");
            }
            else
            {
                await _dbContext.AddAsync(Client);
                await _dbContext.SaveChangesAsync();
                MessageBox.Show("Успешно");
            }
        }

        private async Task<IEnumerable<Client>> GetClients() => 
            await _dbContext.Clients.ToListAsync();
    }
}
