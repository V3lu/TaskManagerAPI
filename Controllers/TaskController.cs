using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskManagerDBContext _dbContext;
        public TaskController(TaskManagerDBContext taskManagerDBContext)
        {
            _dbContext = taskManagerDBContext;
        }

        [HttpGet]
        [Route("returnalltasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _dbContext.Tasks.Select(t => new TaskSendDto(Id: t.Id, Title: t.Title, IsCompleted: t.IsCompleted)).ToListAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("recievetask")]
        public async Task<IActionResult> RecieveTask([FromBody] TaskReceiveDto task)
        {
            try
            {
                Entities.Task taskToAdd = new Entities.Task
                {
                    Title = task.Title,
                    IsCompleted = task.IsCompleted,
                };
                
                await _dbContext.Tasks.AddAsync(taskToAdd);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut]
        [Route("finishtask/{id}")]
        public async Task<IActionResult> MarkTaskAsFinished(int id)
        {
            try
            {
                await _dbContext.Tasks.Where(x => x.Id == id).ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsCompleted, true));
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
