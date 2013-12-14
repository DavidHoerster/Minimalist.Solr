using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Minimalist.Commanding
{
    public static class CommandMapper
    {
        static Dictionary<string, CommandDomainMap> domainObjectMapping = new Dictionary<string, CommandDomainMap>();

        public static void AddMapping(CommandDomainMap mapping)
        {
            if (!domainObjectMapping.ContainsKey(mapping.CommandName))
            {
                domainObjectMapping.Add(mapping.CommandName, mapping);
            }
        }

        public static CommandDomainMap GetMappingForCommand(string commandName)
        {
            return domainObjectMapping.ContainsKey(commandName) ? domainObjectMapping[commandName] : null;
        }

        public static void Clear()
        {
            domainObjectMapping.Clear();
        }

    }
}
