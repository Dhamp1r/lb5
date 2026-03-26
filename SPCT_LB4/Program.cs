//using System;
//using System.Collections.Generic;

//public class ResourceManager
//{
//    private static ResourceManager _instance;
//    private static readonly object _lock = new object();
//    private Dictionary<string, string> _resources;

//    private ResourceManager()
//    {
//        _resources = new Dictionary<string, string>();
//        Console.WriteLine("ResourceManager успішно ініціалізовано.");
//    }

//    public static ResourceManager Instance
//    {
//        get
//        {
//            lock (_lock)
//            {
//                if (_instance == null)
//                {
//                    _instance = new ResourceManager();
//                }
//                return _instance;
//            }
//        }
//    }

//    public void LoadResource(string key, string value)
//    {
//        _resources[key] = value;
//        Console.WriteLine($"Ресурс завантажено: {key} -> {value}");
//    }

//    public void GetResource(string key)
//    {
//        if (_resources.ContainsKey(key))
//            Console.WriteLine($"Отримано ресурс: {_resources[key]}");
//        else
//            Console.WriteLine($"Ресурс {key} не знайдено.");
//    }
//}

//class Program
//{
//    static void Main()
//    {
//        Console.OutputEncoding = System.Text.Encoding.UTF8;
//        Console.InputEncoding = System.Text.Encoding.UTF8;

//        ResourceManager manager1 = ResourceManager.Instance;
//        manager1.LoadResource("Logo", "logo_v1.png");

//        ResourceManager manager2 = ResourceManager.Instance;
//        manager2.GetResource("Logo");

//        Console.WriteLine($"manager1 та manager2 - це один екземпляр? {ReferenceEquals(manager1, manager2)}");
//    }
//}

//using System;

//public interface ICurrencyConverter
//{
//    double ConvertToUsd(double uahAmount);
//}

//public class ExternalBankApi
//{
//    public double FetchExchangeRate(string currencyFrom, string currencyTo)
//    {
//        Console.WriteLine($"[API Баску] Отримання поточного курсу {currencyFrom} до {currencyTo}...");
//        if (currencyFrom == "UAH" && currencyTo == "USD") return 0.025;
//        return 1.0;
//    }
//}

//public class CurrencyAdapter : ICurrencyConverter
//{
//    private readonly ExternalBankApi _bankApi;

//    public CurrencyAdapter(ExternalBankApi bankApi)
//    {
//        _bankApi = bankApi;
//    }

//    public double ConvertToUsd(double uahAmount)
//    {
//        double rate = _bankApi.FetchExchangeRate("UAH", "USD");
//        return uahAmount * rate;
//    }
//}

//class Program
//{
//    static void Main()
//    {
//        Console.OutputEncoding = System.Text.Encoding.UTF8;
//        Console.InputEncoding = System.Text.Encoding.UTF8;

//        ExternalBankApi api = new ExternalBankApi();
//        ICurrencyConverter adapter = new CurrencyAdapter(api);

//        double uah = 1000;
//        double usd = adapter.ConvertToUsd(uah);

//        Console.WriteLine($"Конвертація: {uah} UAH = {usd} USD");
//    }
//}

using System;
using System.Collections.Generic;

public interface IObserver
{
    void Update(string orderStatus);
}

public class Order
{
    private List<IObserver> _observers = new List<IObserver>();
    private string _status;
    public int OrderId { get; private set; }

    public Order(int id) { OrderId = id; _status = "Створено"; }

    public void Subscribe(IObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IObserver observer) => _observers.Remove(observer);

    public void ChangeStatus(string newStatus)
    {
        _status = newStatus;
        Console.WriteLine($"\n[Система] Статус замовлення #{OrderId} змінено на: {_status}");
        NotifyObservers();
    }

    private void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_status);
        }
    }
}

public class Customer : IObserver
{
    private string _name;

    public Customer(string name) { _name = name; }

    public void Update(string orderStatus)
    {
        Console.WriteLine($"Сповіщення для {_name}: Статус вашого замовлення оновлено на '{orderStatus}'.");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Order order = new Order(991);
        Customer cust1 = new Customer("Олександр");
        Customer cust2 = new Customer("Марія");

        order.Subscribe(cust1);
        order.Subscribe(cust2);

        order.ChangeStatus("Відправлено зі складу");
        order.ChangeStatus("Доставлено у відділення");
    }
}