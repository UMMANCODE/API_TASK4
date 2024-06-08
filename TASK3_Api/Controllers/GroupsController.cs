using Microsoft.AspNetCore.Mvc;
using TASK3_Business.Dtos.GroupDtos;
using TASK3_Business.Services.Interfaces;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Api.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class GroupsController : Controller {
    private readonly AppDbContext _context;
    private readonly IGroupService _groupService;

    public GroupsController(AppDbContext context, IGroupService groupService) {
      _context = context;
      _groupService = groupService;
    }

    [HttpGet("")]
    public ActionResult<List<GroupGetAllDto>> GetAll(int pageNumber = 1, int pageSize = 1) {
      var data = _groupService.GetAll(pageNumber, pageSize);

      return StatusCode(200, data);
    }

    [HttpGet("{id}")]
    public ActionResult<GroupGetOneDto> GetById(int id) {
      var data = _groupService.GetById(id);
      return StatusCode(200, data);
    }

    [HttpPost("")]
    public ActionResult Create(GroupCreateOneDto groupCreateOneDto) {
      int id = _groupService.Create(groupCreateOneDto);
      return StatusCode(201, new { id });
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, GroupUpdateOneDto groupUpdateOneDto) {
      _groupService.Update(id, groupUpdateOneDto);
      return StatusCode(204);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id) {
      _groupService.Delete(id);
      return StatusCode(204);
    }
  }
}
