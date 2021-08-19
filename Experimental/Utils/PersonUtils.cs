using System;

namespace Experimental.Utils
{
    internal static class PersonUtils
    {
        internal static void PrintName(this Person person)
        {
            Console.WriteLine("Name: {0}, Age: {1}", person.Name, person.Age); 
        }
    }
}