using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;
using System.Threading.Tasks;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        StudentsContext db;
        public StudentsController(StudentsContext context)
        {
            db = context;

            if (!db.Students.Any())
            {
                db.Students.Add(new Student { Name = "Tom", Surname = "Smith", Group = "A", Age = 26 });
                db.Students.Add(new Student { Name = "Alice", Surname = "Black", Group = "C", Age = 31 });
                db.SaveChanges();
            }
        }

        // Запит GET повертає колекцію об'єктів з БД
        // READ
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            return await db.Students.ToListAsync();
        }

        // Запит POST отримує з тіла запиту відправлені дані і додає в базу даних
        // CREATE
        [HttpPost]
        public async Task<ActionResult<Student>> Post(Student user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            db.Students.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // Запит PUT отримує дані з запиту і додає їх в базу даних
        // UPDATE
        [HttpPut]
        public async Task<ActionResult<Student>> Put(Student student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            if (!db.Students.Any(x => x.Id == student.Id))
            {
                return NotFound();
            }

            db.Update(student);
            await db.SaveChangesAsync();
            return Ok(student);
        }

        // Запит DELETE запит видалення інформації з бази даних
        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> Delete(int id)
        {
            Student student = db.Students.FirstOrDefault(x => x.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return Ok(student);
        }
    }
}
