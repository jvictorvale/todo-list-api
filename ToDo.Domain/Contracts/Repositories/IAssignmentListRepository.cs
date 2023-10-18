﻿using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IAssignmentListRepository : IBaseRepository<AssignmentList>
{
    Task<IPagedResult<AssignmentList>> Search(int? userId, string name, string description, int perPage = 10, int page = 1);
    Task<AssignmentList?> GetById(int? id, int? userId);
}