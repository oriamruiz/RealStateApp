using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.SaleTypes;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface ISaleTypeService : IGenericService<SaleTypeViewModel, SaveSaleTypeViewModel, SaleType>
    {
        Task<SaveSaleTypeViewModel> CheckDelete(int id);

        Task<List<int>> DeletePropertiesAssociatedViewModel(int id);
    }
}
