using System;

namespace Experimental.Partials
{
    /// <summary>
    /// Example class (Is not used)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public partial class Car<T, U>: Vehicle where T: class where U: class
    {
        public int Value { get; private set; }
        public Car()
        {
            OnConstructorStart(); // partial method
            Console.WriteLine("Car constructor");
            OnConstructoEnd(out int x); // second partial method
            Value = x;
        }

        partial void OnConstructorStart(); // private by default
        public partial int OnConstructoEnd(out int x); 
        
        
        public override void VehicleType()
        {
            Console.WriteLine("Car");
        }

        public override void Sound()
        {
            Console.WriteLine("Beep");
            // this.Dispose(); We can use another methods from the same partial class 
        }
    }
}