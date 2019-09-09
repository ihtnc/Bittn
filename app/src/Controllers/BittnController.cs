using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bittn.Api.Models;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Services;
using Bittn.Api.Services.Models;

namespace Bittn.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BittnController : ControllerBase
    {

        [HttpGet("conditions")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetConditions([FromQuery] string name, [FromQuery] int page, [FromServices] IBittnService bittnService)
        {
            var response = await bittnService.GetConditions(new GetConditionsRequest
            {
                Name = name,
                PageIndex = page
            });

            return ApiResponseHelper.Ok<IllnessDetails>($"{response?.Data?.Count() ?? 0} condition(s) found.", response);
        }

        [HttpPost("findHelp")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> FindHelp([FromBody] FindHelpApiRequest request, [FromServices] IBittnService bittnService)
        {
            var response = await bittnService.FindHelp(new FindHelpRequest
            {
                ConditionId = request.ConditionId,
                SeverityLevel = request.Severity,
                PageIndex = request.Page,
                SortField = request.SortField,
                SortDescending = request.SortDescending
            });

            return ApiResponseHelper.Ok<HelpDetails>($"{response?.Data?.Count() ?? 0} help found.", response);
        }

        [HttpPost("bookings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> BookPatient([FromBody] BookPatientApiRequest request, [FromServices] IBittnService bittnService)
        {
            var response = await bittnService.BookPatient(new BookPatientRequest
            {
                ConditionId = request.ConditionId,
                ConditionName = request.ConditionName,
                HelpId = request.HelpId,
                HelpName = request.HelpName,
                PatientName = request.PatientName,
                SeverityLevel = request.Severity
            });

            return ApiResponseHelper.Ok($"Booking created.", response);
        }

        [HttpGet("bookings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetBookings([FromQuery] int page, [FromQuery] string sortField, [FromQuery] bool? sortDescending, [FromServices] IBittnService bittnService)
        {
            var response = await bittnService.GetBookings(new GetBookingsRequest
            {
                PageIndex = page,
                SortField = sortField,
                SortDescending = sortDescending
            });

            return ApiResponseHelper.Ok<BookingDetails>($"{response?.Data?.Count() ?? 0} booking(s) found.", response);
        }

        [HttpDelete("bookings/{bookingId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CancelBooking(int bookingId, [FromServices] IBittnService bittnService)
        {
            var response = await bittnService.CancelBooking(new CancelBookingRequest
            {
                BookingId = bookingId
            });

            return response.Deleted
                ? ApiResponseHelper.Ok("Booking deleted.", response)
                : ApiResponseHelper.Fail("No booking deleted.", response);
        }
    }
}
