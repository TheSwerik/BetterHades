using System;
using System.Collections.ObjectModel;

namespace BetterHades.Components.IO
{
    public class OutputImp : Input, IObserver<Connection>
    {
        public OutputImp() { }

        // Observer
        public void OnNext(Connection value) { IsActive = value == true; }
        public void OnCompleted() { Console.WriteLine("OUTPUT COMPLETE"); }
        public void OnError(Exception error) { Console.WriteLine("OUTPUT " + error); }

        // Helper Methods
        protected override bool Check() { return IsActive; }
    }
}