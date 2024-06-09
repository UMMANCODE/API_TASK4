using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Project.Helpers;
using TASK3_Business.Dtos.StudentDtos;
using TASK3_Business.Exceptions;
using TASK3_Business.Services.Interfaces;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Business.Services.Implementations {
  public class StudentService : IStudentService {
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public StudentService(AppDbContext context, IWebHostEnvironment env) {
      _context = context;
      _env = env;
    }
    public int Create(StudentCreateOneDto createDto) {
      Group? group = _context.Groups.Include(x => x.Students)
        .FirstOrDefault(x => x.Id == createDto.GroupId && !x.IsDeleted) ?? throw new RestException();
      if (group.Limit <= group.Students.Count) {
        throw new RestException();
      }

      Student student = new() {
        FirstName = createDto.FirstName,
        LastName = createDto.LastName,
        Email = createDto.Email,
        Phone = createDto.Phone,
        Address = createDto.Address,
        BirthDate = createDto.BirthDate,
        GroupId = createDto.GroupId
      };

      if (createDto.Photo != null) {
        student.Image = FileManager.Save(createDto.Photo, _env.WebRootPath, "images");
      }

      _context.Students.Add(student);
      _context.SaveChanges();

      return student.Id;
    }

    public void Delete(int id) {
      var studentToDelete = _context.Students.Find(id) ?? throw new RestException();
      var group = _context.Groups.FirstOrDefault(c => c.Id == studentToDelete.GroupId && !c.IsDeleted);

      if (group != null) {
        group.Students.Remove(studentToDelete);
        _context.Groups.Update(group);
      }

      if (studentToDelete.Image != null) {
        FileManager.Delete(_env.WebRootPath, "images", studentToDelete.Image);
      }

      studentToDelete.IsDeleted = true;
      _context.Students.Update(studentToDelete);
    }

    public List<StudentGetAllDto> GetAll(int pageNumber = 1, int pageSize = 1) {

      var data = _context.Students
        .Where(x => !x.IsDeleted)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .OrderBy(x => x.Id)
        .Select(x => new StudentGetAllDto {
          Id = x.Id,
          FirstName = x.FirstName,
          LastName = x.LastName,
          Email = x.Email,
          GroupId = x.GroupId
        }).ToList();

      return data;

    }

    public StudentGetOneDto GetById(int id) {
      var data = _context.Students.Include(s => s.Group)
                  .FirstOrDefault(s => s.Id == id) ?? throw new RestException();

      StudentGetOneDto studentDetailsDto = new() {
        FirstName = data.FirstName,
        LastName = data.LastName,
        Phone = data.Phone,
        Email = data.Email,
        Address = data.Address,
        BirthDate = data.BirthDate,
        GroupId = data.GroupId,
        Id = data.Id
      };

      return studentDetailsDto;
    }

    public void Update(int id, StudentUpdateOneDto studentUpdate) {
      var existingStudent = _context.Students.Find(id) ?? throw new RestException();

      Group? group = _context.Groups.Include(x => x.Students)
        .FirstOrDefault(x => x.Id == studentUpdate.GroupId && !x.IsDeleted) ?? throw new RestException();

      if (group.Limit <= group.Students.Count) {
        throw new RestException();
      }

      existingStudent.FirstName = studentUpdate.FirstName;
      existingStudent.LastName = studentUpdate.LastName;
      existingStudent.Email = studentUpdate.Email;
      existingStudent.Phone = studentUpdate.Phone;
      existingStudent.Address = studentUpdate.Address;
      existingStudent.BirthDate = studentUpdate.BirthDate;
      existingStudent.GroupId = studentUpdate.GroupId;

      if (studentUpdate.Photo != null) {
        if (existingStudent.Image != null) {
          FileManager.Delete(_env.WebRootPath, "images", existingStudent.Image);
        }
        existingStudent.Image = FileManager.Save(studentUpdate.Photo, _env.WebRootPath, "images");
      }

      _context.Students.Update(existingStudent);
      _context.SaveChanges();
    }
  }
}