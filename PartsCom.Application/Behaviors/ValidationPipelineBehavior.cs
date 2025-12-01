using MediatR;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using PartsCom.Application.Interfaces;

namespace PartsCom.Application.Behaviors;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any() || !typeof(TRequest).IsAssignableTo(typeof(IBaseCommand)))
        {
            return await next(cancellationToken);
        }

        ValidationFailure[] validationFailures = await ValidateAsync(request);
        
        if (validationFailures.Length == 0)
        {
            return await next(cancellationToken);
        }

        if (!typeof(TResponse).IsGenericType ||
            typeof(TResponse).GetGenericTypeDefinition() != typeof(ErrorOr<>))
        {
            throw new ValidationException(validationFailures);
        }

        var errors = validationFailures
            .Select(failure => Error.Custom(0,
                code: failure.ErrorCode,
                description: failure.ErrorMessage))
            .ToList();
        
        return (TResponse)(dynamic)errors;

    }
    
    private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        var context = new ValidationContext<TRequest>(request);
        
        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));
        
        ValidationFailure[] validationFailures = [.. validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)];
        
        return validationFailures;
    }
}
