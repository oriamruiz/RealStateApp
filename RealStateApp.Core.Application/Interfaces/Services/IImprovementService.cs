﻿using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Improvements;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<ImprovementViewModel, SaveImprovementViewModel, Improvement>
    {
        Task<SaveImprovementViewModel> CheckDelete(int id);
    }
}
