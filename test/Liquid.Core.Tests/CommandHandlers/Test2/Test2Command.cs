using System.Text.Json.Serialization;
using MediatR;

namespace Liquid.Domain.Tests.CommandHandlers.Test2
{
    public class Test2Command : IRequest<Test2Response>
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}