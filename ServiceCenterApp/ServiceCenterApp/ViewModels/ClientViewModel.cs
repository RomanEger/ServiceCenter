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
    public class ClientViewModel : ViewModelBase
    {
        private readonly ServiceCenterDbContext _dbContext;
        public ClientViewModel(ServiceCenterDbContext dbContext) 
        {
            _dbContext = dbContext;
            RegistrationCommand = new MyCommand(ClientRegistration);
            SaveClientChangesCommand = new MyCommand(SaveChanges);
            Task.Run(async () => Clients = await GetClients());
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

        private IEnumerable<Client> _clients;

        public IEnumerable<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }
        public ICommand RegistrationCommand { get; private set; }
        
        public ICommand SaveClientChangesCommand { get; private set; }

        private async void SaveChanges() => await _dbContext.SaveChangesAsync();
        
        private async void ClientRegistration()
        {
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
