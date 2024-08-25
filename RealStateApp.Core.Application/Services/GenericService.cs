using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class GenericService<ViewModel, SaveViewModel, Entity> : IGenericService<ViewModel, SaveViewModel, Entity>
        where ViewModel : class
        where SaveViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _genericrepository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _genericrepository = repository;
            _mapper = mapper;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            var entitylist = await _genericrepository.GetAllAsync();

            return _mapper.Map<List<ViewModel>>(entitylist);

        }
        public virtual async Task<SaveViewModel> CreateViewModel(SaveViewModel vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);


            entity = await _genericrepository.AddAsync(entity);

            SaveViewModel savevm = _mapper.Map<SaveViewModel>(entity);

            return savevm;

        }
        public virtual async Task<SaveViewModel> GetByIdViewModel(int id)
        {
            Entity entity = await _genericrepository.GetByIdAsync(id);

            SaveViewModel entityViewModel = _mapper.Map<SaveViewModel>(entity);

            return entityViewModel;

        }

        public virtual async Task<SaveViewModel> UpdateViewModel(SaveViewModel vm, int id)
        {
            Entity entity = _mapper.Map<Entity>(vm);

            entity = await _genericrepository.UpdateAsync(entity,id);

            SaveViewModel savevm = _mapper.Map<SaveViewModel>(entity);

            return savevm;
        }

        public virtual async Task DeleteViewModel(int id)
        {
            Entity entity = await _genericrepository.GetByIdAsync(id);

            await _genericrepository.DeleteAsync(entity);
        }
    }
}
