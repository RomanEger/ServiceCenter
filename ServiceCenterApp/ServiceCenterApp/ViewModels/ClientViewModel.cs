﻿using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using System.Collections.ObjectModel;
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
            ClientsList = new ObservableCollection<string>(GetClientsString());
            Clients = new ObservableCollection<Client>(GetClients());
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

        private string _clientStr = "";

        public string ClientStr
        {
            get => _clientStr;
            set
            {
                _clientStr = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<string> ClientsList { get; set; }

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
            if(Client.Login == null)
            {
                MessageBox.Show("Некорректный логин");
                return;
            }
            if(Client.PhoneNumber == null)
            {
                MessageBox.Show("Некорректный номер телефона");
                return;
            }
            var isClientExists = await _dbContext.Clients.AnyAsync(c => c.PhoneNumber == Client.PhoneNumber);
            if (isClientExists)
            {
                MessageBox.Show("Клиент уже зарегистрирован!");
            }
            else
            {
                await _dbContext.AddAsync(Client);
                await _dbContext.SaveChangesAsync();
                Clients.Add(Client);
                MessageBox.Show("Успешно");
            }
        }

        private IEnumerable<Client> GetClients() => 
            _dbContext.Clients.ToList();

        private IEnumerable<string> GetClientsString() => 
            _dbContext.Clients.Select( x => x.Login).ToList();
    }
}
