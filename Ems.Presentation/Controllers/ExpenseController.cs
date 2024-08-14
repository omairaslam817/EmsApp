using Ems.Application.Expenses.Commands.CreateExpense;
using Ems.Application.Expenses.Commands.DeleteExpense;
using Ems.Application.Expenses.Commands.UpdateExpense;
using Ems.Application.Expenses.Queries.GetAllExpensesQuery;
using Ems.Application.Members.Queries.GetExpenseByIdQuery;
using Ems.Application.ViewModels.Factories;
using Ems.Domain.Helpers;
using Ems.Domain.Shared;
using Gatherly.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ems.Domain.Errors.DomainErrors;

namespace Ems.WebApi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExpenseController(ISender sender, IWebHostEnvironment webHostEnvironment)
         : base(sender)
        {
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(CancellationToken cancellationToken)
        {
            var file = Request.Form.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var command = new CreateExpenseCommand() { File = file };
            var result = await Sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }


        [HttpGet("AllExpenses")]
        [ProducesResponseType(200, Type = typeof(List<GetExpenseRequestViewModel>))]
        public async Task<IActionResult> GetExpenses(int pageNumber, int pageSize, [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null)
        {

            var result = await Sender.Send(new GetAllExpensesQuery
            {
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize
            });


            return Ok(result);

        }
        private string SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var folderPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}.{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return filePath;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetExpenseByIdQuery(id);

            Result<ExpenseResponse> response = await Sender.Send(query, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateExpenseCommand command)
        {
            var result = await Sender.Send(command);

            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await Sender.Send(new DeleteExpenseCommand { ExpenseId = id });


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
