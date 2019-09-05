using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Bittn.Api.Models;
using Xunit;
using FluentAssertions;

namespace Bittn.Api.Tests.Models
{
    public class FindHelpApiRequestTests
    {
        private readonly FindHelpApiRequest _request;

        public FindHelpApiRequestTests()
        {
            _request = new FindHelpApiRequest();
        }

        [Theory]
        [InlineData("Page", "SortField", "SortDescending")]
        public void Properties_Should_Include_JsonRequiredAttribute(params string[] exclusions)
        {
            var t = _request.GetType();
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => exclusions.All(e => !string.Equals(e, p.Name)));
            foreach(var prop in props)
            {
                prop.Should().BeDecoratedWith<JsonRequiredAttribute>();
            }
        }
    }
}
