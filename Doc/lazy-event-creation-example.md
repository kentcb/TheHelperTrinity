# Lazy Event Creation Example

The following provides a complete example of using the `EventHelper`'s lazy event arguments creation facility. The code includes explanatory comments. 

```C#
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Kent.Boogaart.HelperTrinity;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MouseInfo mi = new MouseInfo();

            //attach event handler only to prove that the event works 
            //if attached, an EventArgs instance will be created every time the event fires 
            //if unattached, no objects at all will be created when the event fires 

            //mi.MouseMoved += delegate(object sender, MouseInfoEventArgs e) 
            //{ 
            //    Console.WriteLine("x = {0}, y = {1}", e.X, e.Y); 
            //};

            Console.WriteLine("Press any key to exit . . .");
            Console.ReadKey();
        }
    }

    public sealed class MouseInfo
    {
        private int _x;
        private int _y;

        public event EventHandler MouseMoved;
        private Func<MouseInfoEventArgs> _createMouseInfoEventArgs;

        public MouseInfo()
        {
            _createMouseInfoEventArgs = CreateMouseInfoEventArgs;

            //kick off a separate thread to simulate an event that fires often (approx every 5ms)
            Thread thread = new Thread(delegate()
            {
                Random r = new Random();

                while (true)
                {
                    //just randomly set coordinates and then raise event
                    _x = r.Next();
                    _y = r.Next();
                    OnMouseMoved();
                    Thread.Sleep(5);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void OnMouseMoved()
        {
            //using an anonymous delegate will cause a delegate instance to be created each time the event is raised 
            //this is not desirable if the event fires often and rarely has any listeners 
            //EventHelper.Raise(MouseMoved, this, delegate 
            //{ 
            //    return new MouseInfoEventArgs(_x, _y); 
            //}); 

            //this way uses a pre-created delegate so there is no need to create another on each invocation 
            //this is ideal because absolutely no objects are created if there are no listeners for the event 
            //if there are listeners, only the EventArgs instance is created
            EventHelper.Raise(MouseMoved, this, _createMouseInfoEventArgs);
        }

        private MouseInfoEventArgs CreateMouseInfoEventArgs()
        {
            return new MouseInfoEventArgs(_x, _y);
        }
    }

    public sealed class MouseInfoEventArgs : EventArgs
    {
        private int _x;
        private int _y;

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        internal MouseInfoEventArgs(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
```