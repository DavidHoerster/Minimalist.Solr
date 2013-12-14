using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Minimalist.Commands
{
    [Serializable]
    [DataContract]
    [KnownType("GetKnownTypes")]
    public abstract class CommandBase
    {
        static Type[] GetKnownTypes()
        {
            List<Type> commandTypes = new List<Type>();
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(CommandBase)))
                    .ToList()
                    .ForEach(t => commandTypes.Add(t));

            return commandTypes.ToArray();
        }
    }
}
