﻿using ToDo.Application.DTOs.Base;

namespace ToDo.Application.DTOs.AssignmentList;

public class AssignmentListSearchDto : BaseSearch
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}