using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Experimental
{
    public abstract class What
    {
        protected virtual void SomeSomeSome()
        {
            Console.WriteLine("SomeSomeSome");
        } 
    }

    public abstract class Animal: What, IDisposable
    {
        public Animal()
        {
            
        }
        public static string Name { get; set; } // get / set == public, cause prop is public
        public static void JJJ()
        {
            Console.WriteLine("GRWFWE");
        }

        public abstract void SomeSome();
        
        IEnumerator<int> GetNumbers()
        {
            foreach (var v in Enumerable.Range(1, 7))
            {
                yield return v;
            }
        }

        protected override void SomeSomeSome()
        {
            base.SomeSomeSome();
        }

        public void SomeMethodForDelegateExample()
        {
            Console.WriteLine("From animal");
        }
        
        IEnumerable<int> GetListNumbers()
        {
            return new List<int> {1, 2, 3};
        }

        public void Dispose()
        {
        }
    }
}