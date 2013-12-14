using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Minimalist.Commanding.Map
{
    public interface IMapToAnInstanceMethod
    {
        IMapToAnInstanceMethod Route<T>();
        IMapToAnInstanceMethod To<D>();
        CommandRouteMapper UsingMethod(string methodName);
    }
}
