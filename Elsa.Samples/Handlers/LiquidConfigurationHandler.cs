using Elsa.Samples.Models;
using Elsa.Scripting.Liquid.Messages;
using Fluid;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Samples.Handlers
{
    public class LiquidConfigurationHandler : INotificationHandler<EvaluatingLiquidExpression>
    {
        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
        {
            var context = notification.TemplateContext;
            context.MemberAccessStrategy.Register<User>();
            context.MemberAccessStrategy.Register<RegistrationModel>();

            return Task.CompletedTask;
        }
    }
}