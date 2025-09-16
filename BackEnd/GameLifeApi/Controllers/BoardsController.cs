using Microsoft.AspNetCore.Mvc;
using GameLifeModels.Interface.Services;
using GameLifeModels.Interface.Requests;
using GameLifeModels.Interface.Responses;

namespace GameLifeApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpPost]
    public IActionResult CreateBoard([FromBody] CreateBoardRequest request)
    {
        try
        {
            var id = _boardService.CreateBoard(request.Cells);
            Serilog.Log.Information("Board created with id {BoardId} and {CellCount} cells", id, request.Cells?.Length ?? 0);
            return Ok(new CreateBoardResponse() { Success = true, BoardId = id });
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error creating board: {Error}", ex.Message);
            return BadRequest(new CreateBoardResponse() { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("{id}/next")]
    public IActionResult GetNextState(Guid id)
    {
        try
        {
            var board = _boardService.CalculateNextGeneration(id);
            var state = new BoardWithCellsResponse() { BoardId = id, Cells = board };
            Serilog.Log.Information("Next generation calculated for board {BoardId}", id);
            return Ok(state);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error getting next state for board {BoardId}: {Error}", id, ex.Message);
            return BadRequest(new BoardWithCellsResponse() { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("{id}/next/{times}")]
    public IActionResult GetNextGenerations(Guid id, int times)
    {
        try
        {
            var board = _boardService.CalculateNextGenerationTimes(id, times);
            var state = new BoardWithCellsResponse() { BoardId = id, Cells = board };
            Serilog.Log.Information("{Times} generations calculated for board {BoardId}", times, id);
            return Ok(state);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error getting next {Times} generations for board {BoardId}: {Error}", times, id, ex.Message);
            return BadRequest(new BoardWithCellsResponse() { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("{id}/final")]
    public IActionResult GetFinalState(Guid id)
    {
        try
        {
            var board = _boardService.CalculateFinal(id);
            var state = new BoardWithCellsResponse() { BoardId = id, Cells = board };
            Serilog.Log.Information("Final state calculated for board {BoardId}", id);
            return Ok(state);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error getting final state for board {BoardId}: {Error}", id, ex.Message);
            return BadRequest(new BoardWithCellsResponse() { Success = false, Error = ex.Message });
        }
    }

    [HttpGet("{id}/current")]
    public IActionResult GetCurrentState(Guid id)
    {
        try
        {
            var board = _boardService.CurrentBoard(id);
            var state = new BoardWithCellsResponse() { BoardId = id, Cells = board };
            Serilog.Log.Information("Current state retrieved for board {BoardId}", id);
            return Ok(state);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error getting current state for board {BoardId}: {Error}", id, ex.Message);
            return BadRequest(new BoardWithCellsResponse() { Success = false, Error = ex.Message });
        }
    }    
}
