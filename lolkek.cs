public class Calculation
{
    public int Id { get; set; }
    public string Expression { get; set; }
    public string Result { get; set; }
    public DateTime Timestamp { get; set; }
}
public class AppDbContext : DbContext
{
    public DbSet<Calculation> Calculations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "calculations.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}
public class CalculatorViewModel : INotifyPropertyChanged
{
    public string Expression { get; set; } = "";
    public string Result { get; set; } = "";
    public ObservableCollection<Calculation> History { get; set; }

    public ICommand CalculateCommand { get; }

    public CalculatorViewModel()
    {
        CalculateCommand = new Command(Calculate);
        History = new ObservableCollection<Calculation>();
        LoadHistory();
    }

    private void Calculate()
    {
        try
        {
            var result = new DataTable().Compute(Expression, null).ToString();
            Result = result;
            SaveCalculation(Expression, result);
            LoadHistory();
        }
        catch (Exception ex)
        {
            Result = "Error";
        }
    }

    // ... методы SaveCalculation и LoadHistory (аналогично предыдущему примеру)
}
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new CalculatorViewModel();
    }
}