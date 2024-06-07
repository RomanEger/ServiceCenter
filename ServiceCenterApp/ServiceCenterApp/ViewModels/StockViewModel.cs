using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;

namespace ServiceCenterApp.ViewModels;

public class StockViewModel : ViewModelBase
{
    private readonly ServiceCenterDbContext _dbContext;
    
    public StockViewModel(ServiceCenterDbContext dbContext)
    {
        _dbContext = dbContext;
        Task.Run(async () => Details = await GetDetails());
        SaveChangesCommand = new MyCommand(SaveChanges);
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

    private IEnumerable<Detail> _details;
    
    private IEnumerable<Detail> Details
    {
        get => _details;
        set
        {
            _details = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveChangesCommand;
    
    private async void SaveChanges() => _dbContext.SaveChangesAsync();
    
    private async Task<IEnumerable<Detail>> GetDetails() => await _dbContext.Details.ToCompletedWorksAsync();
}