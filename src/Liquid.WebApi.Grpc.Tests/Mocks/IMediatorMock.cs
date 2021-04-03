using FluentValidation;
using FluentValidation.Results;
using Liquid.Core.Exceptions;
using Liquid.WebApi.Grpc.Tests.TestCases.Service;
using MediatR;
using NSubstitute;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.WebApi.Grpc.Tests.Mocks
{
    /// <summary>
    /// IMediator Mock, returns the mock interface for tests purpose.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IMediatorMock
    {
        /// <summary>
        /// Gets the IMediator mock.
        /// </summary>
        /// <returns></returns>
        public static IMediator GetMock()
        {
            var mock = Substitute.For<IMediator>();
            return mock;
        }

        /// <summary>
        /// Gets the mock with validation errors.
        /// </summary>
        /// <returns></returns>
        public static IMediator GetMockWithValidationErrors()
        {
            var mock = Substitute.For<IMediator>();
            mock.When(x => x.Send(Arg.Any<Request>()))
                .Do(x => throw new ValidationException(new List<ValidationFailure>() { new ValidationFailure("test", "errorMessage") }));
            return mock;
        }

        /// <summary>
        /// Gets the mock with custom errors.
        /// </summary>
        /// <returns></returns>
        public static IMediator GetMockWithCustomErrors()
        {
            var mock = Substitute.For<IMediator>();
            mock.When(x => x.Send(Arg.Any<Request>()))
                .Do(x => throw new LightCustomException("Custom Exception", new ExceptionCustomCodes(13, "Custom Exception")));
            return mock;
        }
    }
}
