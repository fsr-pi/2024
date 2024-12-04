using MediatR;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Text.Json;

namespace Contract
{
  public class PrintPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
  {
    private readonly IEnumerable<IValidator<TRequest>> validators;

   

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
      Console.WriteLine($"-----{request.GetType().Name}:  {JsonSerializer.Serialize(request)}");
      return await next();
    }
  }
}
