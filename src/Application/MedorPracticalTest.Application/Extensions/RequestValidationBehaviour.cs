using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace MedorPracticalTest.Application.Extensions
{
        // <summary>
        // Pipeline for MediatR defining additional validation logic
        // </summary>
        public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
        {
                private readonly IEnumerable<IValidator<TRequest>> _validators;

                public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

                public async Task<TResponse> Handle(
                        TRequest request,
                        RequestHandlerDelegate<TResponse> next,
                        CancellationToken cancellationToken)
                {
                        var context = new ValidationContext<TRequest>(request);

                        var validationFailures = await Task.WhenAll(
                                _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

                        var errors = validationFailures
                                .Where(validationResult => !validationResult.IsValid)
                                .SelectMany(validationResult => validationResult.Errors)
                                .Select(validationFailure => new ValidationFailure(
                                        validationFailure.PropertyName,
                                        validationFailure.ErrorMessage))
                                .ToList();

                        var resultErrors = String.Join(", ", errors);

                        if (errors.Count > 0)
                        {
                                var errorMessages = errors.Select(e => e.ErrorMessage).ToList();
                                throw new ValidationException(resultErrors);
                        }

                        return await next();
                }
        }
}
