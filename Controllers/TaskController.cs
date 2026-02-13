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

        abstract class Creator
        {
            public abstract IProduct FactoryMethod();

            public string SomeOperation()
            {
                var product = FactoryMethod();
                var result = "Creator: The same creator's code has just worked with "
                    + product.Operation();

                return result;
            }
        }
        class ConcreteCreator1 : Creator
        {
            public override IProduct FactoryMethod()
            {
                return new ConcreteProduct1();
            }
        }

        class ConcreteCreator2 : Creator
        {
            public override IProduct FactoryMethod()
            {
                return new ConcreteProduct2();
            }
        }
        public interface IProduct
        {
            string Operation();
        }
        class ConcreteProduct1 : IProduct
        {
            public string Operation()
            {
                return "{Result of ConcreteProduct1}";
            }
        }

        class ConcreteProduct2 : IProduct
        {
            public string Operation()
            {
                return "{Result of ConcreteProduct2}";
            }
        }

        class Client
        {
            public void Main()
            {
                Console.WriteLine("App: Launched with the ConcreteCreator1.");
                ClientCode(new ConcreteCreator1());

                Console.WriteLine("");

                Console.WriteLine("App: Launched with the ConcreteCreator2.");
                ClientCode(new ConcreteCreator2());
            }
            public void ClientCode(Creator creator)
            {
                Console.WriteLine("Client: I'm not aware of the creator's class," +
                    "but it still works.\n" + creator.SomeOperation());
            }
        }
    }
}
