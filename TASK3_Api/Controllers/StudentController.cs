using Microsoft.AspNetCore.Mvc;
using TASK3_Business.Dtos.StudentDtos;
using TASK3_Business.Services.Interfaces;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class StudentController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly IStudentService _studentService;

    public StudentController(AppDbContext context, IStudentService studentService) {
      _context = context;
      _studentService = studentService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 1) {
      var data = _studentService.GetAll(pageNumber, pageSize);

      return StatusCode(200, data);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id) {
      var data = _studentService.GetById(id);
      return StatusCode(200, data);
    }

    [HttpPost("")]
    public IActionResult Create(StudentCreateOneDto studentCreateOneDto) {
      int id = _studentService.Create(studentCreateOneDto);
      return StatusCode(201, new { id });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, StudentUpdateOneDto studentUpdateOneDto) {
      _studentService.Update(id, studentUpdateOneDto);
      return StatusCode(204);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
      _studentService.Delete(id);
      return StatusCode(204);
    }
  }
}
