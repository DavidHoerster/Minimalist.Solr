using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Agile.Minimalist.Commands.Writer
{
    public partial class CommandServiceClient : ClientBase<ICommandHandler>,
                                                ICommandHandler
    {
        public CommandServiceClient()
        {
        }

        public CommandServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CommandServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CommandServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CommandServiceClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public void Handle(CommandBase command)
        {
            base.Channel.Handle(command);
        }
    }
}
