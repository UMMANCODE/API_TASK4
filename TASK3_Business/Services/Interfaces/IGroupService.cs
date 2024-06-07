using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASK3_Business.Dtos.GroupDtos;

namespace TASK3_Business.Services.Interfaces {
  public interface IGroupService {
    int Create(GroupCreateOneDto createDto);
    List<GroupGetAllDto> GetAll();
    GroupGetOneDto GetById(int id);
    void Update(int id, GroupUpdateOneDto updateDto);
    void Delete(int id);
  }
}
