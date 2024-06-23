using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
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
        AddOrUpdateDetailCommand = new MyCommand(AddOrUpdateDetail);
        AddStockDetailCommand = new MyCommand(AddStockDetail);
        AddDetailCommand = new MyCommand(AddDetail);
        Details = GetDetailsString();
        Stocks = GetStocks();
        StockDetail = new StockDetail();
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

    private IEnumerable<string> _details;

    public IEnumerable<string> Details
    {
        get => _details;
        set
        {
            _details = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<string> GetDetailsString() => _dbContext.Details.Select(x => x.Name).ToList();

    private IEnumerable<string> _stocks;

    public IEnumerable<string> Stocks
    {
        get => _stocks;
        set
        {
            _stocks = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<string> GetStocks() => _dbContext.Stocks.Select(x => x.Name).ToList();
    
    private string _stock;

    public string Stock
    {
        get => _stock;
        set
        {
            _stock = value;
            var sId = _dbContext.Stocks.Where(x => x.Name == value).Select(x => x.Id).FirstOrDefault();
            OnPropertyChanged();
        }
    }
    
    private string _selectedDetail;
    
    public string SelectedDetail
    {
        get => _selectedDetail;
        set
        {
            _selectedDetail = value;
            Detail = _dbContext.Details.FirstOrDefault(x => x.Name == _selectedDetail) ?? new();
            OnPropertyChanged();
        }
    }

    private StockDetail StockDetail { get; set; }
    
    public StockDetail GetStockDetail()
    {
        var dId = _dbContext.Details.Where(x => x.Name == SelectedStockDetail.DetailName).Select(x => x.Id)
            .FirstOrDefault();
        var sId = _dbContext.Stocks.Where(x => x.Name == SelectedStockDetail.StockName).Select(x => x.Id)
            .FirstOrDefault();
        return _dbContext.StockDetails.FirstOrDefault(x => x.StockId == sId && x.DetailId == dId) ?? new StockDetail();
    }
    
    private int _count;

    public int Count
    {
        get => _count;
        set
        {
            if (value < 0)
            {
                MessageBox.Show("Кол-во должно быть больше 0");
                return;
            }
            if(StockDetail is null)
                return;
            _count = value;
            OnPropertyChanged();
        }
    }

    private Detail _detail = new ();

    public Detail Detail
    {
        get => _detail;
        set
        {
            _detail = value;
            StockDetail = _dbContext.StockDetails.FirstOrDefault(x => x.DetailId == Detail.Id) ?? new StockDetail();
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<StockDetailView> StockDetails { get; set; }

    public ICommand SaveChangesCommand { get; private set; }

    public ICommand DeleteCommand { get; private set; }

    public ICommand AddOrUpdateDetailCommand { get; private set; }

    public ICommand AddDetailCommand { get; private set; }
    
    public ICommand AddStockDetailCommand { get; private set; }

    public void AddStockDetail()
    {
        var s = _dbContext.Stocks.FirstOrDefault(x => x.Name == Stock);
        var d = _dbContext.Details.FirstOrDefault(x => x.Name == SelectedDetail);
        if (s == null || d == null)
        {
            MessageBox.Show("Ошибка");
            return;
        }

        var sdd = _dbContext.StockDetails.FirstOrDefault(x => x.StockId == s.Id && x.DetailId == d.Id);
        if (sdd is null)
        {
            StockDetail = new StockDetail()
            {
                CountDetail = Count,
                DetailId = d.Id,
                StockId = s.Id
            };
            try
            {
                _dbContext.StockDetails.Add(StockDetail);
            }
            catch
            {
                MessageBox.Show("Не удается добавить деталь");
            }
            
        }
        else
            sdd.CountDetail = Count;

        SaveChanges();
        var stockDetail = new StockDetailView()
        {
            Count = StockDetail.CountDetail,
            DetailName = d.Name,
            StockName = s.Name
        };
        StockDetails.Add(stockDetail);
    }
    
    private void AddOrUpdateDetail()
    {
        var d = _dbContext.Details.FirstOrDefault(x => x.Id == Detail.Id);
        if (d is null)
        {
            try
            {
                _dbContext.Details.Add(Detail);
            }
            catch
            {
                MessageBox.Show("Ошибка");
                return;
            }
        }
        SaveChanges();
    }

    private void SaveChanges()
    {
        try
        {
            _dbContext.SaveChanges();
            MessageBox.Show("Успешно");
        }
        catch
        {
            MessageBox.Show("Ошибка при сохранении");
        }
        Count = 0;
        Detail = new Detail();
        SelectedDetail = "";
        Stock = "";
    }

    private void AddDetail()
    {
        var dId = _dbContext.Details.Where(x => x.Name == SelectedStockDetail.DetailName).Select(x => x.Id)
            .FirstOrDefault();
        if (dId == 0)
        {
            MessageBox.Show("Деталь не существует");
            return;
        }
        var sId = _dbContext.Stocks.Where(x => x.Name == SelectedStockDetail.StockName).Select(x => x.Id)
            .FirstOrDefault();
        if (sId == 0)
        {
            MessageBox.Show("Склад не существует");
            return;
        }
        
        StockDetail.DetailId = dId;
        StockDetail.StockId = sId;
        StockDetail.CountDetail = SelectedStockDetail.Count;
        SaveChanges();
    }
    
    private void Delete()
    {
        var stockDetail = _dbContext.StockDetails.FirstOrDefault(x => x.Id == SelectedStockDetail.Id);
        if (stockDetail is null) return;
        _dbContext.StockDetails.Remove(stockDetail);
        SaveChanges();
        StockDetails.ToList().Remove(SelectedStockDetail);
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