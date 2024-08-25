using AutoMapper;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Admin;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Developer;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RealStateApp.Core.Application.ViewModels.AdminHome;
using RealStateApp.Core.Application.ViewModels.Admins;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Developers;
using RealStateApp.Core.Application.ViewModels.Improvements;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;
using RealStateApp.Core.Application.ViewModels.SaleTypes;
using RealStateApp.Core.Application.ViewModels.Users;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationRequest, LoginViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<RegisterRequest, CreateAgentOrClientViewModel>()
                 .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.Dni, opt => opt.Ignore());

            CreateMap<RegisterRequest, CreateAdminOrDeveloperViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.Phone, opt => opt.Ignore())
                 .ForMember(dest => dest.AccountImgUrl, opt => opt.Ignore());

            CreateMap<UpdateImageRequest, CreateAgentOrClientViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Error, opt => opt.Ignore())
                .ForMember(dest => dest.HasError, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Property, PropertyViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ForMember(dest => dest.IsFavorite, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 

            CreateMap<Property, SavePropertyViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ForMember(dest => dest.MainImageFile, opt => opt.Ignore())
                 .ForMember(dest => dest.optionalImagesFile, opt => opt.Ignore())
                 .ForMember(dest => dest.optionalimages, opt => opt.Ignore())
                 .ForMember(dest => dest.PropertyTypes, opt => opt.Ignore())
                 .ForMember(dest => dest.SaleTypes, opt => opt.Ignore())
                 .ForMember(dest => dest.Improvements, opt => opt.Ignore())
                 .ForMember(dest => dest.ImprovementsIds, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.PropertyType, opt => opt.Ignore())
                 .ForMember(dest => dest.SaleType, opt => opt.Ignore())
                 .ForMember(dest => dest.Improvements, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 


            CreateMap<Property, DetailsPropertyViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ForMember(dest => dest.OptionalImages, opt => opt.Ignore())
                 .ForMember(dest => dest.Agent, opt => opt.Ignore())
                 .ForMember(dest => dest.RedirectTo, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.ImageUrl2, opt => opt.Ignore())
                 .ForMember(dest => dest.ImageUrl3, opt => opt.Ignore())
                 .ForMember(dest => dest.ImageUrl4, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 

            CreateMap<GetActiveAgentViewModel, GetActiveAgentResponse>()
                  .ReverseMap();
            CreateMap<UpdateAgentResponse, UpdateAgentViewModel>()
                  .ReverseMap();

            CreateMap<UpdateAgentRequest, UpdateAgentViewModel>()
                  .ForMember(dest => dest.Error, opt => opt.Ignore())
                  .ForMember(dest => dest.HasError, opt => opt.Ignore())
                  .ReverseMap();

            CreateMap<UpdateAdminResponse, UpdateAdminViewModel>()
                  .ReverseMap();

            CreateMap<UpdateAdminRequest, UpdateAdminViewModel>()
                  .ForMember(dest => dest.Error, opt => opt.Ignore())
                  .ForMember(dest => dest.HasError, opt => opt.Ignore())
                  .ReverseMap();

            CreateMap<UpdateDeveloperResponse, UpdateDeveloperViewModel>()
                  .ReverseMap();

            CreateMap<UpdateDeveloperRequest, UpdateDeveloperViewModel>()
                  .ForMember(dest => dest.Error, opt => opt.Ignore())
                  .ForMember(dest => dest.HasError, opt => opt.Ignore())
                  .ReverseMap();

            CreateMap<UpdateImageRequest, RealStateApp.Core.Application.ViewModels.Users.UpdateImgViewModel>()
                  .ReverseMap();

            CreateMap<GetUserResponse, UserViewModel>()
                  .ReverseMap();

            CreateMap<ChangeStatusResponse, UserViewModel>()
                  .ReverseMap();

        

        CreateMap<ChangeStatusRequest, GetUserResponse>()
                 .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                 .ForMember(dest => dest.LastName, opt => opt.Ignore())
                 .ForMember(dest => dest.UserName, opt => opt.Ignore())
                 .ForMember(dest => dest.Dni, opt => opt.Ignore())
                 .ForMember(dest => dest.Email, opt => opt.Ignore())
                 .ForMember(dest => dest.Roles, opt => opt.Ignore())
                 .ForMember(dest => dest.Phone, opt => opt.Ignore())
                 .ForMember(dest => dest.AccountImgUrl, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ReverseMap();


            CreateMap<PropertyType, PropertyTypeViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<SaleType, SaleTypeViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<Improvement, ImprovementViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();


            CreateMap<GetUserResponse, AdminHomeAgentViewModel>()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.UserName, opt => opt.Ignore())
                 .ForMember(dest => dest.Dni, opt => opt.Ignore())
                 .ForMember(dest => dest.Roles, opt => opt.Ignore())
                 .ForMember(dest => dest.Phone, opt => opt.Ignore())
                 .ForMember(dest => dest.AccountImgUrl, opt => opt.Ignore());

            CreateMap<GetUserResponse, AdminHomeAdminViewModel>()
                 .ReverseMap()
                 .ForMember(dest => dest.Phone, opt => opt.Ignore())
                 .ForMember(dest => dest.AccountImgUrl, opt => opt.Ignore());

            CreateMap<GetUserResponse, AdminHomeDeveloperViewModel>()
                 .ReverseMap()
                 .ForMember(dest => dest.Phone, opt => opt.Ignore())
                 .ForMember(dest => dest.AccountImgUrl, opt => opt.Ignore());

            
                CreateMap<PropertyType, AdminHomePropertyTypesViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ForMember(dest => dest.Propertiesquantity, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<PropertyType, SavePropertyTypeViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<SaleType, AdminHomeSaleTypesViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ForMember(dest => dest.Propertiesquantity, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<SaleType, SaveSaleTypeViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<Improvement, AdminHomeImprovementsViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<Improvement, SaveImprovementViewModel>()
                 .ForMember(dest => dest.Error, opt => opt.Ignore())
                 .ForMember(dest => dest.HasError, opt => opt.Ignore())
                 .ReverseMap();

            CreateMap<Property, PropertyDto>()
                 .ForMember(dest => dest.PropertyImprovements, opt => opt.Ignore())
                 .ReverseMap()
                 .ForMember(dest => dest.MainImageUrl, opt => opt.Ignore())
                 .ForMember(dest => dest.ImageUrl2, opt => opt.Ignore())
                 .ForMember(dest => dest.ImageUrl3, opt => opt.Ignore())
                 .ForMember(dest => dest.ImageUrl4, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());


            CreateMap<GetPropertyParameter, GetPropertyQuery>()
                 .ReverseMap();



            CreateMap<PropertyType, PropertyTypeDto>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore());
            
            
            CreateMap<PropertyType, CreatePropertyTypeCommand>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore()); 

            CreateMap<PropertyType, UpdatePropertyTypeCommand>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore()); 

                CreateMap<PropertyType, PropertyTypeUpdateResponse>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore());



            CreateMap<SaleType, SaleTypeDto>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore());


            CreateMap<SaleType, CreateSaleTypeCommand>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore());

            CreateMap<SaleType, UpdateSaleTypeCommand>()
                 .ReverseMap()
                 .ForMember(dest => dest.Properties, opt => opt.Ignore());

            CreateMap<SaleType, SaleTypeUpdateResponse>()
             .ReverseMap()
             .ForMember(dest => dest.Properties, opt => opt.Ignore());




            CreateMap<Improvement, ImprovementDto>()
                 .ReverseMap();


            CreateMap<Improvement, CreateImprovementCommand>()
                 .ReverseMap();

            CreateMap<Improvement, UpdateImprovementCommand>()
                 .ReverseMap();

            CreateMap<Improvement, ImprovementUpdateResponse>()
             .ReverseMap();




            CreateMap<GetUserResponse, AgentDto>()
             .ForMember(dest => dest.QuantityProperties, opt => opt.Ignore())
             .ReverseMap()
             .ForMember(dest => dest.UserName, opt => opt.Ignore())
             .ForMember(dest => dest.Dni, opt => opt.Ignore())
             .ForMember(dest => dest.Roles, opt => opt.Ignore())
             .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
             .ForMember(dest => dest.AccountImgUrl, opt => opt.Ignore())
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.HasError, opt => opt.Ignore())
             .ForMember(dest => dest.Error, opt => opt.Ignore());

        }


    }
}
