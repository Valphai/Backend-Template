using AutoMapper;
using Project.Application.DataTransferObjects;
using Project.Domain.Entities;

namespace Project.Application.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleResponseDTO>().ReverseMap();
    }
}