using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bittn.Api.Controllers;
using Bittn.Api.Models;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Services;
using Bittn.Api.Services.Models;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Bittn.Api.Tests.Controllers
{
    public class BittnControllerTests
    {
        private readonly BittnController _controller;

        public BittnControllerTests()
        {
            _controller = new BittnController();
        }

        [Fact]
        public void Class_Should_Include_ApiControllerAttribute()
        {
            var t = _controller.GetType();
            t.Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public void Class_Should_Include_RouteAttribute()
        {
            var t = _controller.GetType();
            t.Should().BeDecoratedWith<RouteAttribute>(attr => attr.Template == "api");
        }

        [Fact]
        public void GetConditions_Should_Include_HttpGetAttribute()
        {
            var methodName = nameof(_controller.GetConditions);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<HttpGetAttribute>(attr => attr.Template == "conditions");
        }

        [Fact]
        public void GetConditions_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.GetConditions);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void GetConditions_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.GetConditions);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void GetConditions_Should_Call_IBittnService_GetConditions()
        {
            var service = Substitute.For<IBittnService>();
            GetConditionsRequest arg = null;
            service.GetConditions(Arg.Do<GetConditionsRequest>(a => arg = a)).Returns(new GetConditionsResponse());

            var name = "name";
            var page = 1;
            await _controller.GetConditions(name, page, service);

            await service.Received(1).GetConditions(Arg.Any<GetConditionsRequest>());
            arg.Name.Should().Be(name);
            arg.PageIndex.Should().Be(page);
            arg.SortField.Should().BeNull();
            arg.SortDescending.Should().BeNull();
        }

        [Fact]
        public async void GetConditions_Should_Return_Correctly()
        {
            var response = new GetConditionsResponse
            {
                Data = new [] { new IllnessDetails() },
                PrevPageIndex = 111,
                NextPageIndex = 222
            };
            var service = Substitute.For<IBittnService>();
            service.GetConditions(Arg.Any<GetConditionsRequest>()).Returns(response);

            var actual = await _controller.GetConditions("any", 1, service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be($"{response.Data.Count()} condition(s) found.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response.Data);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.PrevPageIndex.Should().Be(response.PrevPageIndex);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.NextPageIndex.Should().Be(response.NextPageIndex);
        }

        [Fact]
        public void FindHelp_Should_Include_HttpPostAttribute()
        {
            var methodName = nameof(_controller.FindHelp);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<HttpPostAttribute>(attr => attr.Template == "findHelp");
        }

        [Fact]
        public void FindHelp_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.FindHelp);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status400BadRequest, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void FindHelp_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.FindHelp);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void FindHelp_Should_Call_IBittnService_FindHelp()
        {
            var request = new FindHelpApiRequest
            {
                ConditionId = 1,
                Severity = 2,
                Page = 3,
                SortField = "field",
                SortDescending = true
            };

            var service = Substitute.For<IBittnService>();
            FindHelpRequest arg = null;
            service.FindHelp(Arg.Do<FindHelpRequest>(a => arg = a)).Returns(new FindHelpResponse());

            await _controller.FindHelp(request, service);

            await service.Received(1).FindHelp(Arg.Any<FindHelpRequest>());
            arg.ConditionId.Should().Be(request.ConditionId);
            arg.SeverityLevel.Should().Be(request.Severity);
            arg.PageIndex.Should().Be(request.Page);
            arg.SortField.Should().Be(request.SortField);
            arg.SortDescending.Should().Be(request.SortDescending);
        }

        [Fact]
        public async void FindHelp_Should_Return_Correctly()
        {
            var response = new FindHelpResponse();
            var service = Substitute.For<IBittnService>();
            service.FindHelp(Arg.Any<FindHelpRequest>()).Returns(response);

            var actual = await _controller.FindHelp(new FindHelpApiRequest(), service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be($"{response.Data?.Count() ?? 0} help found.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response.Data);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.PrevPageIndex.Should().Be(response.PrevPageIndex);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.NextPageIndex.Should().Be(response.NextPageIndex);
        }

        [Fact]
        public void BookPatient_Should_Include_HttpPostAttribute()
        {
            var methodName = nameof(_controller.BookPatient);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<HttpPostAttribute>(attr => attr.Template == "bookings");
        }

        [Fact]
        public void BookPatient_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.BookPatient);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status201Created, null)]
        [InlineData(StatusCodes.Status400BadRequest, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void BookPatient_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.BookPatient);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void BookPatient_Should_Call_IBittnService_BookPatient()
        {
            var request = new BookPatientApiRequest
            {
                ConditionId = 123,
                ConditionName = "conditionName",
                HelpId = 456,
                HelpName = "helpName",
                PatientName = "patientName",
                Severity = 789
           };

            var service = Substitute.For<IBittnService>();
            BookPatientRequest arg = null;
            service.BookPatient(Arg.Do<BookPatientRequest>(a => arg = a)).Returns(new BookPatientResponse());

            await _controller.BookPatient(request, service);

            await service.Received(1).BookPatient(Arg.Any<BookPatientRequest>());
            arg.ConditionId.Should().Be(request.ConditionId);
            arg.ConditionName.Should().Be(request.ConditionName);
            arg.HelpId.Should().Be(request.HelpId);
            arg.HelpName.Should().Be(request.HelpName);
            arg.PatientName.Should().Be(request.PatientName);
            arg.SeverityLevel.Should().Be(request.Severity);
        }

        [Fact]
        public async void BookPatient_Should_Return_Correctly()
        {
            var response = new BookPatientResponse();
            var service = Substitute.For<IBittnService>();
            service.BookPatient(Arg.Any<BookPatientRequest>()).Returns(response);

            var actual = await _controller.BookPatient(new BookPatientApiRequest(), service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Booking created.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response);
        }

        [Fact]
        public void GetBookings_Should_Include_HttpGetAttribute()
        {
            var methodName = nameof(_controller.GetBookings);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<HttpGetAttribute>(attr => attr.Template == "bookings");
        }

        [Fact]
        public void GetBookings_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.GetBookings);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void GetBookings_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.GetConditions);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void GetBookings_Should_Call_IBittnService_GetBookings()
        {
            var service = Substitute.For<IBittnService>();
            GetBookingsRequest arg = null;
            service.GetBookings(Arg.Do<GetBookingsRequest>(a => arg = a)).Returns(new GetBookingsResponse());

            var page = 1;
            var sortField = "sortField";
            var sortDescending = true;
            await _controller.GetBookings(page, sortField, sortDescending, service);

            await service.Received(1).GetBookings(Arg.Any<GetBookingsRequest>());
            arg.PageIndex.Should().Be(page);
        }

        [Fact]
        public async void GetBookings_Should_Return_Correctly()
        {
            var response = new GetBookingsResponse
            {
                Data = new [] { new BookingDetails() },
                PrevPageIndex = 111,
                NextPageIndex = 222
            };
            var service = Substitute.For<IBittnService>();
            service.GetBookings(Arg.Any<GetBookingsRequest>()).Returns(response);

            var actual = await _controller.GetBookings(1, "any", null, service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be($"{response.Data.Count()} booking(s) found.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response.Data);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.PrevPageIndex.Should().Be(response.PrevPageIndex);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Navigation.NextPageIndex.Should().Be(response.NextPageIndex);
        }
    }
}
