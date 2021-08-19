using System;

namespace Experimental.Partials
{
    /// <summary>
    /// Example class (Is not used)
    /// </summary>

    // Неможна задавати одне обмеження в одному частоковому класі, а друге в іншому
    // Вони повинні бути прописані в якомусь одному з часткових класів. Або за бажанням вказати у всіх.
    public partial class Car<T, U> : IDisposable where T: class where U: class 
    {
        // public partial void OnConstructorStart()
        // {
        //     Console.WriteLine("Start");
        // }

        public partial int OnConstructoEnd(out int x)
        {
            Console.WriteLine("Car End");
            x = 7;
            return x;
        }

        public void Dispose()
        {
            //this.VehicleType(); We can use another methods from the same partial class 
        }
    }
}