using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Features.Behaviors
{
    public class OrderValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;
        private readonly ILogger<OrderValidationBehavior<TRequest, TResponse>> logger;

        OrderValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators, 
            ILogger<OrderValidationBehavior<TRequest, TResponse>> logger)
        {
            this.validators = validators;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // pre
            if (this.validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failurs = this.validators
                                    .Select(x => x.Validate(context))
                                    .SelectMany(x => x.Errors)
                                    .Where(x => x != null)
                                    .ToList();
                if (failurs.Any())
                {
                    throw new FluentValidation.ValidationException(failurs);
                }
            }

            try
            {
                return await next();
            }
            catch (System.Exception)
            {
                var requestName = typeof(TRequest).Name;
                this.logger.LogError($"Unhandeled exception occured. Request Name : {requestName}");
                throw;
            }

            // post
        }
    }
}