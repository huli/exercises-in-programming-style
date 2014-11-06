using System.Collections.Generic;

namespace BulletinBoardStyle
{
    internal class BulletinBoard
    {
        public delegate void DynamicEventHandler(object sender, DynamicEventArgs args);

        private Dictionary<string, List<DynamicEventHandler>> _subscribtions = new Dictionary<string, List<DynamicEventHandler>>();
 
        public void Publish(string eventName, DynamicEventArgs args)
        {
            if (_subscribtions.ContainsKey(eventName))
            {
                _subscribtions[eventName].ForEach(
                    handler => handler(this, args)
                    );
            }
        }

        public void Subscribe(string eventName, DynamicEventHandler eventHandler)
        {
            if (!_subscribtions.ContainsKey(eventName))
            {
                _subscribtions.Add(eventName, new List<DynamicEventHandler>());
            }
            _subscribtions[eventName].Add(eventHandler);
        }
    }
}