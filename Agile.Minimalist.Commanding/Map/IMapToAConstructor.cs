using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Minimalist.Commanding.Map
{
    public interface IMapToAConstructor
    {
        IMapToAConstructor Route<T>();
        CommandRouteMapper To<D>();
    }
}
