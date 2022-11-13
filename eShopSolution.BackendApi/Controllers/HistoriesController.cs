using eShopSolution.Application.System.Actions;
using eShopSolution.Application.System.Forms;
using eShopSolution.Application.System.Histories;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Histories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoriesController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IFormService _formService;
        private readonly IActionService _actionService;
        private readonly IUserService _userService;

        public HistoriesController(IHistoryService historyService, IActionService actionService,
            IFormService formService, IUserService userService)
        {
            _historyService = historyService;
            _actionService = actionService;
            _formService = formService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var query = from h in _historyService.GetAll()
                        join f in _formService.GetAll() on h.FormId equals f.Id
                        join a in _actionService.GetAll() on h.ActionId equals a.Id
                        join u in _userService.GetAll() on h.UserId equals u.Id
                        select new HistoryViewModel()
                        {
                            Id = h.Id,
                            Time = h.Time,
                            ActionName = a.Name,
                            FormName = f.Name,
                            UserName = u.UserName,
                        };

            var totalRecord = await query.CountAsync();

            var data = await query.ToListAsync();

            var histories = new PageResult<HistoryViewModel>()
            {
                Data = data,
                TotalRecord = totalRecord,
            };

            return Ok(histories);
        }

        [HttpGet("{historyId}")]
        public async Task<ActionResult> GetById(int historyId)
        {
            var history = await _historyService.GetById(historyId);

            if (history == null) return BadRequest($"Can not find history by id: {historyId}");

            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult> Create(HistoryGetRequest request)
        {
            History history = new History()
            {
                Time = DateTime.Now,
                UserId = request.UserId,
                FormId = request.FormId,
                ActionId = request.ActionId,
            };

            var result = await _historyService.Create(history);

            if (result > 0) return Ok("Add history success");

            return BadRequest("Fail to add history");
        }

        [HttpDelete("{historyId}")]
        public async Task<ActionResult> Remove(int historyId)
        {
            var history = await _historyService.GetById(historyId);
            if (history == null) return BadRequest($"Can not find history by id: {historyId}");

            var result = await _historyService.Remove(history);
            if (result > 0) return Ok("Delete history succes");

            return BadRequest("Fail to delete history");
        }
    }
}
