using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Eventing.Handler
{
    public static class EventMapper
    {
        private static Dictionary<string, List<Type>> _eventHandlerMappings = new Dictionary<string, List<Type>>();

        static EventMapper()
        {
            var eventHandlers = Assembly.GetExecutingAssembly()
                                        .GetTypes()
                                        .Where(
                                            t => t.FindInterfaces((t1, c) => t1.Name == c.ToString(), "IHandle`1").Length > 0);

            foreach (var handlerClass in eventHandlers)
            {
                var handlerInterfaces = handlerClass.GetInterfaces();
                foreach (var handlerInterface in handlerInterfaces)
                {
                    var arg = handlerInterface.GetGenericArguments();
                    if (arg != null)
                    {
                        if (_eventHandlerMappings.ContainsKey(arg[0].Name))
                        {
                            //add this handler to the existing list
                            _eventHandlerMappings[arg[0].Name].Add(handlerClass);
                        }
                        else
                        {
                            //add a new handler to the existing list
                            _eventHandlerMappings.Add(arg[0].Name, new List<Type>() { handlerClass });
                        }
                    }
                }
            }
        }

        public static List<Type> GetEventHandlersForEventMessage(Type eventType)
        {
            return _eventHandlerMappings[eventType.Name];
        }

    }
}
