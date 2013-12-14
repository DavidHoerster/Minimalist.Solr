using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Agile.Minimalist.Commands;
using Agile.Minimalist.Commanding.Map;

namespace Agile.Minimalist.Commanding
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall, ReleaseServiceInstanceOnTransactionComplete=false)]
    public class CommandHandler : ICommandHandler
    {
        static CommandHandler()
        {
            CommandRouteMapper
                .Init
                .FromConfiguration()
                    .UsingDefaultSection();
                //.AddMethodRoute()
                //    .Route<MyCommandClass>()
                //    .To<SomeDomainClass>()
                //    .UsingMethod("SomeMethodName")
                //.AddConstructorRoute()
                //    .Route<AnotherCommandClass>()
                //    .To<SomeOtherDomainClass>();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true, ReleaseInstanceMode = ReleaseInstanceMode.AfterCall)]
        public void Handle(CommandBase command)
        {
            CommandDomainMap map = CommandMapper.GetMappingForCommand(command.GetType().Name);
            if (map!=null && String.IsNullOrWhiteSpace(map.DomainClassMethodName))
            {
                //invoke the AR's ctor
                Type t = Type.GetType(map.DomainClassName);
                if (t != null)
                {
                    Activator.CreateInstance(t, command);
                }
            }
            else if (map!=null)
            {
                //invoke a AR method
                Type t = Type.GetType(map.DomainClassName);
                if (t !=null)
                {
                    Object domain = Activator.CreateInstance(t);
                    domain.GetType().InvokeMember(map.DomainClassMethodName, 
                                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, 
                                                null, 
                                                domain, 
                                                new object[] { command });
                }
            }
            else
            {
                //punt?
            }
        }
    }
}
