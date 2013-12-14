using Agile.Minimalist.Eventing;
using Agile.Minimalist.Eventing.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Domain
{
    public abstract class DomainBase
    {
        protected void ApplyEvent(EventBase evt)
        {
            List<Type> eventHandlers =
                EventMapper.GetEventHandlersForEventMessage(evt.GetType());

            foreach (var handler in eventHandlers)
            {
                var localHandler = handler;
                Task.Factory.StartNew(() =>
                {
                    var instance = Activator.CreateInstance(localHandler);
                    instance.GetType().InvokeMember("Handle",
                                                         System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod,
                                                         null,
                                                         instance,
                                                         new object[] { evt });
                });
            }
        }
    }
}
