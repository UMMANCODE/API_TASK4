using Microsoft.AspNetCore.Mvc;
using TASK3_Business.Dtos.StudentDtos;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class StudentController : ControllerBase {
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context) {
      _context = context;
    }

    [HttpGet]
    public IActionResult GetAll() {
      var data = _context.Students
        .Where(x => !x.IsDeleted)
        .Select(x => new StudentGetAllDto {
          Id = x.Id,
          FirstName = x.FirstName,
          LastName = x.LastName,
          Email = x.Email,
          GroupId = x.GroupId
        }).ToList();

      return StatusCode(200, data);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id) {
      var data = _context.Students
        .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
      if (data == null) {
        return NotFound();
      }
      StudentGetOneDto studentGetOneDto = new() {
        Id = data.Id,
        FirstName = data.FirstName,
        LastName = data.LastName,
        Email = data.Email,
        GroupId = data.GroupId,
        Phone = data.Phone,
        Address = data.Address,
        BirthDate = data.BirthDate
      };

      return StatusCode(200, studentGetOneDto);
    }

    [HttpPost("")]
    public IActionResult Create(StudentCreateOneDto studentCreateOneDto) {
      Student student = new() {
        FirstName = studentCreateOneDto.FirstName,
        LastName = studentCreateOneDto.LastName,
        Email = studentCreateOneDto.Email,
        GroupId = studentCreateOneDto.GroupId,
        Phone = studentCreateOneDto.Phone,
        Address = studentCreateOneDto.Address,
        BirthDate = studentCreateOneDto.BirthDate
      };

      _context.Students.Add(student);
      _context.SaveChanges();

      return StatusCode(201, new { student.Id });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, StudentUpdateOneDto studentUpdateOneDto) {
      var existingStudent = _context.Students
        .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
      if (existingStudent == null) {
        return NotFound();
      }
      existingStudent.FirstName = studentUpdateOneDto.FirstName;
      existingStudent.LastName = studentUpdateOneDto.LastName;
      existingStudent.Email = studentUpdateOneDto.Email;
      existingStudent.GroupId = studentUpdateOneDto.GroupId;
      existingStudent.Phone = studentUpdateOneDto.Phone;
      existingStudent.Address = studentUpdateOneDto.Address;
      existingStudent.BirthDate = studentUpdateOneDto.BirthDate;

      _context.Students.Update(existingStudent);
      _context.SaveChanges();

      return StatusCode(204);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
      var existingStudent = _context.Students
        .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
      if (existingStudent == null) {
        return NotFound();
      }

      _context.Students.Remove(existingStudent);
      _context.SaveChanges();

      return StatusCode(204);
    }
  }
}
