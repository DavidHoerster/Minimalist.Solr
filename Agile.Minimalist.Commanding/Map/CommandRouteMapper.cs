using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Agile.Minimalist.Commanding.SectionHandler;

namespace Agile.Minimalist.Commanding.Map
{
    public class CommandRouteMapper : IMapToAConstructor, 
                                        IMapToAnInstanceMethod,
                                        IMapFromConfiguration,
                                        Agile.Minimalist.Commanding.Map.ICommandRouteMapper
    {
        private CommandRouteMapper() { }

        private CommandDomainMap _commandDomainMap;

        public static CommandRouteMapper Init { get { CommandMapper.Clear(); return new CommandRouteMapper(); } }

        private IConfigurationReader _configurationReader;
        public IConfigurationReader ConfigurationSectionReader
        {
            get { return _configurationReader ?? new ConfigurationReader(); }
            set { _configurationReader = value; }
        }

        public IMapToAConstructor AddConstructorRoute()
        {
            return this;
        }

        public IMapToAnInstanceMethod AddMethodRoute()
        {
            return this;
        }

        public IMapFromConfiguration FromConfiguration()
        {
            return this;
        }

        IMapToAConstructor IMapToAConstructor.Route<T>()
        {
            _commandDomainMap = new CommandDomainMap();
            _commandDomainMap.CommandName = typeof(T).Name;
            return this;
        }

        CommandRouteMapper IMapToAConstructor.To<D>()
        {
            _commandDomainMap.DomainClassName = typeof(D).AssemblyQualifiedName;
            CommandMapper.AddMapping(_commandDomainMap);
            return this;
        }

        IMapToAnInstanceMethod IMapToAnInstanceMethod.Route<T>()
        {
            _commandDomainMap = new CommandDomainMap();
            _commandDomainMap.CommandName = typeof(T).Name;
            return this;
        }

        IMapToAnInstanceMethod IMapToAnInstanceMethod.To<D>()
        {
            _commandDomainMap.DomainClassName = typeof(D).AssemblyQualifiedName;
            return this;
        }

        CommandRouteMapper IMapToAnInstanceMethod.UsingMethod(string methodName)
        {
            _commandDomainMap.DomainClassMethodName = methodName;
            CommandMapper.AddMapping(_commandDomainMap);
            return this;
        }

        void IMapFromConfiguration.UsingSectionName(string sectionName)
        {
            BuildCommandMapper(false, sectionName);
            return;
        }


        void IMapFromConfiguration.UsingDefaultSection()
        {
            BuildCommandMapper(true, string.Empty);
            return;
        }

        private void BuildCommandMapper(bool useDefaultSection, string sectionName)
        {
            var routes = useDefaultSection ? ConfigurationSectionReader.GetRoutes("commandRoutes") : ConfigurationSectionReader.GetRoutes(sectionName);

            if (routes == null)
            {
                throw new NullReferenceException();
            }
            foreach(RouteConfigurationElement r in routes)
            {
                _commandDomainMap = new CommandDomainMap();
                _commandDomainMap.DomainClassName = r.DomainClass;
                _commandDomainMap.DomainClassMethodName = r.DomainClassMethod;
                _commandDomainMap.CommandName = r.Command;
                CommandMapper.AddMapping(_commandDomainMap);
            }
        }
    }
}
