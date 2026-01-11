using Microsoft.AspNetCore.Mvc;
using ApplicantTracking.Application.Commands.Create;
using ApplicantTracking.Application.Commands.Delete;
using ApplicantTracking.Application.Commands.Update;
using ApplicantTracking.Application.DTOs;
using ApplicantTracking.Application.Queries.GetById;
using ApplicantTracking.Application.Queries.List;
using ApplicantTracking.Application.Queries.Paged;
using MediatR;
using System.Threading.Tasks;

namespace ApplicantTracking.Api.Controllers
{
    [ApiController]
    [Route("candidates")]
    public sealed class CandidateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidateController(IMediator mediator) => _mediator = mediator;

        [HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await _mediator.Send(new GetCandidateListQuery());
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> ListPaged([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 5)
        {
            if (page <= 0) return BadRequest("'page' must be >= 1.");
            if (itemsPerPage <= 0) return BadRequest("'itemsPerPage' must be >= 1.");

            var result = await _mediator.Send(new GetCandidatePagedListQuery(page, itemsPerPage));
            return Ok(result);
        }

        [HttpGet("{idCandidate:int}")]
        public async Task<IActionResult> Get([FromRoute] int idCandidate)
        {
            var result = await _mediator.Send(new GetCandidateByIdQuery(idCandidate));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateCandidateDto candidate)
        {
            var created = await _mediator.Send(new CreateCandidateCommand(candidate));
            return CreatedAtAction(nameof(Get), new { idCandidate = created.IdCandidate }, created);
        }

        [HttpPut("{idCandidate:int}")]
        public async Task<IActionResult> Edit([FromRoute] int idCandidate, [FromBody] UpdateCandidateDto candidate)
        {
            var updated = await _mediator.Send(new UpdateCandidateCommand(idCandidate, candidate));
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{idCandidate:int}")]
        public async Task<IActionResult> Delete([FromRoute] int idCandidate)
        {
            var deleted = await _mediator.Send(new DeleteCandidateCommand(idCandidate));
            return deleted ? NoContent() : NotFound();
        }
    }
}
