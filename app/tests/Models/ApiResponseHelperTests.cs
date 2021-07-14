using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bittn.Api.Models;
using Bittn.Api.Services.Models;
using Xunit;
using FluentAssertions;

namespace Bittn.Api.Tests.Models
{
    public class ApiResponseHelperTests
    {
        [Fact]
        public void Ok_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Ok(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.Should().BeNull();
        }

        [Fact]
        public void Ok_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Ok(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Ok_Should_Return_Correctly_With_PagedResponse_Data()
        {
            var content = new PagedResponse<string>
            {
                Data = new [] { "data" },
                PrevPageIndex = 111,
                NextPageIndex = 222
            };
            var message = "message";
            var actual = ApiResponseHelper.Ok(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content.Data);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.PrevPageIndex.Should().Be(content.PrevPageIndex);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.NextPageIndex.Should().Be(content.NextPageIndex);
        }

        [Fact]
        public void Fail_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Fail(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.Should().BeNull();
        }

        [Fact]
        public void Fail_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Fail(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void BadRequest_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.BadRequest(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.Should().BeNull();
        }

        [Fact]
        public void BadRequest_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.BadRequest(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Error_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Error(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.Should().BeNull();
        }

        [Fact]
        public void Error_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Error(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void ObjectResponse_Should_Return_Null_Content_Correctly()
        {
            var actual = ApiResponseHelper.ObjectResponse(StatusCodes.Status100Continue, true, "message");

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status100Continue);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("message");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void ObjectResponse_Should_Return_Correctly()
        {
            var actual = ApiResponseHelper.ObjectResponse(StatusCodes.Status100Continue, true, message: "message", "content");

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status100Continue);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("message");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be("content");
        }

        [Fact]
        public void ObjectResponse_Should_Return_PagedResponse_Correctly()
        {
            var content = new PagedResponse<string>
            {
                Data = new [] { "data" },
                PrevPageIndex = 111,
                NextPageIndex = 222
            };
            var actual = ApiResponseHelper.ObjectResponse(StatusCodes.Status100Continue, true, message: "message", content: content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status100Continue);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("message");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content.Data);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.PrevPageIndex.Should().Be(content.PrevPageIndex);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.NextPageIndex.Should().Be(content.NextPageIndex);
        }
    }
}
