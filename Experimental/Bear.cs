using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Experimental
{
    //[assembly:InternalsVisibleTo("FriendAssembly")]
    
    public class Bear: Animal
    {
        public event EventHandler NameInserted;
            
        public int Value { get; set; } = 1;
        private string _field;

        public static float Health;
        
        public static int Age { get; set; }
        public string Name
        {
            get => _field;
            set
            {
                _field = value.Trim();
                
                if (this.NameInserted != null)
                {
                    this.NameInserted(this, new EventArgs());
                }
            }
        }

        public Bear()
        {
            Health = 400f;
        }
        
        internal void PrintBear()
        {
            Console.WriteLine("Called from expression tree");
        }
        
        public override string ToString()
        {
            return $"{this.Name}";
        }

        public override void SomeSome()
        {
            throw new NotImplementedException();
        }
    }
}