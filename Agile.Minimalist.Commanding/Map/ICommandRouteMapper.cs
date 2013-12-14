using System;
namespace Agile.Minimalist.Commanding.Map
{
    public interface ICommandRouteMapper
    {
        IMapToAConstructor AddConstructorRoute();
        IMapToAnInstanceMethod AddMethodRoute();
        IMapFromConfiguration FromConfiguration();
    }
}
