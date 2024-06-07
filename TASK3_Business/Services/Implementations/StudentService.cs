using Microsoft.EntityFrameworkCore;
using TASK3_Business.Dtos.StudentDtos;
using TASK3_Business.Exceptions;
using TASK3_Business.Services.Interfaces;
using TASK3_Core.Entities;
using TASK3_DataAccess;

namespace TASK3_Business.Services.Implementations {
  public class StudentService : IStudentService {
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context) {
      _context = context;

    }
    public int Create(StudentCreateOneDto createDto) {
      Group group = _context.Groups.Include(x => x.Students)
        .FirstOrDefault(x => x.Id == createDto.GroupId && !x.IsDeleted);
      if (group == null) {
        throw new RestException();
      }

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

      _context.Students.Add(student);
      _context.SaveChanges();

      return student.Id;
    }

    public void Delete(int id) {
      var studentToDelete = _context.Students.Find(id);
      if (studentToDelete == null) {
        throw new RestException();
      }

      var group = _context.Groups.FirstOrDefault(c => c.Id == studentToDelete.GroupId);
      if (group != null) {
        group.Students.Remove(studentToDelete);
        _context.Groups.Update(group);
      }
      _context.Students.Remove(studentToDelete);
      _context.SaveChanges();

    }

    public List<StudentGetAllDto> GetAll() {

      var data = _context.Students

          .Select(x => new StudentGetAllDto {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            GroupId = x.GroupId
          })
          .ToList();

      return data;

    }

    public StudentGetOneDto GetById(int id) {
      var data = _context.Students.Include(s => s.Group)
                  .FirstOrDefault(s => s.Id == id);
      if (data == null) {
        throw new RestException();
      }

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
      var existingStudent = _context.Students.Find(id);
      if (existingStudent == null) {
        throw new RestException();
      }

      Group group = _context.Groups.Include(x => x.Students)
        .FirstOrDefault(x => x.Id == studentUpdate.GroupId && !x.IsDeleted);
      if (group == null) {
        throw new RestException();
      }


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

      _context.Students.Update(existingStudent);
      _context.SaveChanges();
    }
  }
}