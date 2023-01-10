using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController:BaseApiController
    {
        public DataContext _context { get; }
        public ActivitiesController(DataContext context)
        {
           _context = context;
            
        }
        [HttpGet] //api/activites
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        [HttpGet("{id}")] //api/activites/guid
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await _context.Activities.FindAsync();
        }
    }
}