using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agile.Minimalist.Commanding.SectionHandler;
using System.Configuration;

namespace Agile.Minimalist.Commanding.Map
{
    public class ConfigurationReader : IConfigurationReader
    {
        public RoutesCollection GetRoutes(string sectionName)
        {
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;

            CommandRouteSection routesSection = ConfigurationManager.GetSection(sectionName) as CommandRouteSection;

            return routesSection.Routes;
        }
    }

    public interface IConfigurationReader
    {
        RoutesCollection GetRoutes(string sectionName);
    }
}
