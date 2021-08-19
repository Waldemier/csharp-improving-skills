using System;

namespace Experimental.Partials
{
    public abstract class Vehicle
    {
        public abstract void VehicleType();

        public virtual void Sound()
        {
            Console.WriteLine("None");
        }
    }
}