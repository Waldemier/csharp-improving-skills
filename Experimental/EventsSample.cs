using System;

namespace Experimental
{
    public class ClassCounter
    {
        public delegate void MethodContainer();

        public MethodContainer? onCount { get; set; } = new MethodContainer(() => { Console.WriteLine("Default"); });
        
        public event MethodContainer OnCountEvent;
        public event MethodContainer onCountEvent2;
        
        public void Counter()
        {
            for (int i = 0; i < 100; i++)
            {
                if (i.Equals(71))
                {
                    // Delegate
                    onCount?.Invoke(); // if delegate is null -> do nothing
                    // Event
                    OnCountEvent?.Invoke(); // if event is null -> do nothing

                    OnCountEvent += onCountEvent2;
                    
                    OnCountEvent?.Invoke();
                    OnCountEvent = null; // Can be possible only in this class.
                }
            }
        }
    }
}