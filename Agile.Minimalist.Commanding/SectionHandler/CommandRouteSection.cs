using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Agile.Minimalist.Commanding.SectionHandler
{
    public class CommandRouteSection : ConfigurationSection
    {
        [ConfigurationProperty("routes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RoutesCollection))]
        public RoutesCollection Routes
        {
            get
            {
                RoutesCollection routes = (RoutesCollection)this["routes"];
                return routes;
            }
        }
    }

    public class RoutesCollection : ConfigurationElementCollection
    {
        public RoutesCollection()
        {
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RouteConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RouteConfigurationElement)element).Command;
        }

        public RouteConfigurationElement this[int index]
        {
            get { return (RouteConfigurationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index)!=null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public RouteConfigurationElement this[string Name]
        {
            get { return (RouteConfigurationElement)BaseGet(Name); }
        }

        public int IndexOf(RouteConfigurationElement route)
        {
            return BaseIndexOf(route);
        }

        public void Add(RouteConfigurationElement route)
        {
            BaseAdd(route);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

    }

    public class RouteConfigurationElement : ConfigurationElement
    {
        public RouteConfigurationElement() { }
        public RouteConfigurationElement(string command, string domain, string method)
        {
            Command = command;
            DomainClass = domain;
            DomainClassMethod = method;
        }

        [ConfigurationProperty("command", IsRequired=true)]
        public String Command { get { return (string)this["command"]; } set { this["command"] = value; } }

        [ConfigurationProperty("routesToClass", IsRequired=true)]
        public String DomainClass { get { return (string)this["routesToClass"]; } set { this["routesToClass"] = value; } }
        
        [ConfigurationProperty("usingMethod", IsRequired=false)]
        public String DomainClassMethod { get { return (string)this["usingMethod"]; } set { this["usingMethod"] = value; } }
    }
}
