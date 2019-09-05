using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Bittn.Api.Models;
using Xunit;
using FluentAssertions;

namespace Bittn.Api.Tests.Models
{
    public class BookPatientApiRequestTests
    {
        private readonly BookPatientApiRequest _request;

        public BookPatientApiRequestTests()
        {
            _request = new BookPatientApiRequest();
        }

        [Fact]
        public void Properties_Should_Include_JsonRequiredAttribute()
        {
            var t = _request.GetType();
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in props)
            {
                prop.Should().BeDecoratedWith<JsonRequiredAttribute>();
            }
        }
    }
}
