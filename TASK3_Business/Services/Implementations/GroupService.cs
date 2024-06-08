using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TASK3_Business.Dtos.GroupDtos;
using TASK3_Business.Exceptions;
using TASK3_Business.Services.Interfaces;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Business.Services.Implementations {
  public class GroupService : IGroupService {
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public GroupService(AppDbContext context, IWebHostEnvironment env) {
      _context = context;
      _env = env;
    }
    public int Create(GroupCreateOneDto dto) {
      if (_context.Groups.Any(x => x.Name == dto.Name))
        throw new RestException();

      Group entity = new() {
        Name = dto.Name,
        Limit = dto.Limit
      };
      _context.Groups.Add(entity);
      _context.SaveChanges();
      return entity.Id;
    }
    public List<GroupGetAllDto> GetAll(int pageNumber = 1, int pageSize = 1) {
      if (pageNumber <= 0 || pageSize <= 0) {
        throw new RestException();
      }

      return _context.Groups.Where(x => !x.IsDeleted)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(x => new GroupGetAllDto {
          Id = x.Id,
          Name = x.Name,
          Limit = x.Limit
        }).ToList();
    }
    public GroupGetOneDto GetById(int id) {
      var group = _context.Groups.Include(g => g.Students)
          .FirstOrDefault(x => x.Id == id);

      return group == null
        ? throw new RestException()
        : new GroupGetOneDto {
          Id = group.Id,
          Name = group.Name,
          Limit = group.Limit
        };
    }
    public void Update(int id, GroupUpdateOneDto updateDto) {
      var group = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted) ?? throw new RestException();

      if (group.Name != updateDto.Name && _context.Groups.Any(x => x.Name == updateDto.Name && !x.IsDeleted))
        throw new RestException();

      group.Name = updateDto.Name;
      group.Limit = updateDto.Limit;
      group.UpdatedAt = DateTime.Now;

      _context.SaveChanges();
    }

    public void Delete(int id) {
      var group = _context.Groups.Include(g => g.Students)
        .FirstOrDefault(x => x.Id == id && !x.IsDeleted) ?? throw new RestException();

      group.IsDeleted = true;
      _context.Groups.Update(group);
      _context.SaveChanges();
    }
  }
}
