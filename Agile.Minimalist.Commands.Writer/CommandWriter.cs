using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Agile.Minimalist.Commands.Writer
{
    public class CommandWriter : ICommandWriter
    {
        public void SendCommand(CommandBase theCommand)
        {
            var client = new CommandServiceClient("NetMsmqBinding_ICommandHandler2");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                client.Handle(theCommand);
                scope.Complete();
            }

            client.Close();
        }
    }
}
