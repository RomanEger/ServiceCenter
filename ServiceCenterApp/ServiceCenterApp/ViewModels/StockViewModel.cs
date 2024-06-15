using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using ServiceCenterApp.ReportViews;

namespace ServiceCenterApp.ViewModels;

public class StockViewModel : ViewModelBase
{
    private readonly ServiceCenterDbContext _dbContext;
    
    public StockViewModel(ServiceCenterDbContext dbContext)
    {
        _dbContext = dbContext;
        StockDetails = new ObservableCollection<StockDetailView>( GetDetails());
        if (StockDetails.Any(x => x.Count < 5))
        {
            Info = "Необходимо пополнить запасы!";
        }
        SaveChangesCommand = new MyCommand(SaveChanges);
        DeleteCommand = new MyCommand(Delete);
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
    
    public string Info { get; set; }
    
    public ObservableCollection<StockDetailView> StockDetails { get; set; }

    public ICommand SaveChangesCommand;

    public ICommand DeleteCommand;

    private async void SaveChanges() => _dbContext.SaveChangesAsync();
    
    private async void Delete()
    {
        StockDetails.ToList().Remove(SelectedStockDetail);
        var stockDetail = await _dbContext.StockDetails.FirstOrDefaultAsync(x => x.Id == SelectedStockDetail.Id);
        if (stockDetail is null) return;
        _dbContext.StockDetails.Remove(stockDetail);
        await _dbContext.SaveChangesAsync();
    }

    private IEnumerable<StockDetailView> GetDetails()
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
}