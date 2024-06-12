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
        Task.Run(async () => StockDetails = await GetDetails());
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

    private IEnumerable<StockDetailView> _stockDetails;
    
    private IEnumerable<StockDetailView> StockDetails
    {
        get => _stockDetails;
        set
        {
            _stockDetails = value;
            OnPropertyChanged();
        }
    }

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

    private async Task<IEnumerable<StockDetailView>> GetDetails()
    {
        var list = await(
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
            }).ToListAsync();
        return list;
    }
}