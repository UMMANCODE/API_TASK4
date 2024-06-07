using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASK3_Business.Dtos.StudentDtos;

namespace TASK3_Business.Services.Interfaces {
  public interface IStudentService {
    void Update(int id, StudentUpdateOneDto studentUpdate);
    void Delete(int id);
    List<StudentGetAllDto> GetAll();
    StudentGetOneDto GetById(int id);
    int Create(StudentCreateOneDto createDto);
  }
}
