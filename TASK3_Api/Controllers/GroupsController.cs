using Microsoft.AspNetCore.Mvc;
using TASK3_Business.Dtos.GroupDtos;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class GroupsController : Controller {
    private readonly AppDbContext _context;

    public GroupsController(AppDbContext context) {
      _context = context;
    }

    [HttpGet("")]
    public ActionResult<List<GroupGetAllDto>> GetAll(int pageNumber = 1, int pageSize = 1) {
      if (pageNumber <= 0 || pageSize <= 0) {
        return BadRequest("Page number and page size must be greater than zero!");
      }
      var data = _context.Groups
        .Where(x => !x.IsDeleted)
        .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Select(x => new GroupGetAllDto {
            Id = x.Id,
            Name = x.Name,
            Limit = x.Limit
          }).ToList();

      return StatusCode(200, data);
    }

    [HttpGet("{id}")]
    public ActionResult<GroupGetOneDto> GetById(int id) {
      var data = _context.Groups
        .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
      if (data == null) {
        return NotFound();
      }
      GroupGetOneDto groupGetOneDto = new() {
        Id = data.Id,
        Name = data.Name,
        Limit = data.Limit
      };

      return StatusCode(200, groupGetOneDto);
    }

    [HttpPost("")]
    public ActionResult Create(GroupCreateOneDto groupCreateOneDto) {
      Group group = new() {
        Name = groupCreateOneDto.Name,
        Limit = groupCreateOneDto.Limit
      };

      _context.Groups.Add(group);
      _context.SaveChanges();

      return StatusCode(201, new { group.Id });
    }

    [HttpPut("{id}")]
    public ActionResult Update(GroupUpdateOneDto groupUpdateOneDto) {
      var existingGroup = _context.Groups
        .FirstOrDefault(x => x.Id == groupUpdateOneDto.Id && !x.IsDeleted);
      if (existingGroup == null) {
        return NotFound();
      }
      existingGroup.Name = groupUpdateOneDto.Name;
      existingGroup.Limit = groupUpdateOneDto.Limit;

      _context.Groups.Update(existingGroup);
      _context.SaveChanges();

      return StatusCode(204);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id) {
      var existingGroup = _context.Groups
        .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
      if (existingGroup == null) {
        return NotFound();
      }

      _context.Groups.Remove(existingGroup);
      _context.SaveChanges();

      return StatusCode(204);
    }
  }
}
