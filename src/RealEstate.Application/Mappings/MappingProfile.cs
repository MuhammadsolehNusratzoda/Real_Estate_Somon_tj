using AutoMapper;
using RealEstate.Application.DTOs.Property;
using RealEstate.Application.DTOs.Admin;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.User;

namespace RealEstate.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Havli
        CreateMap<Havli, HavliDto>();
        CreateMap<HavliCreateDto, Havli>();
        CreateMap<HavliUpdateDto, Havli>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // DomApartment
        CreateMap<DomApartment, DomApartmentDto>();
        CreateMap<DomApartmentCreateDto, DomApartment>();
        CreateMap<DomApartmentUpdateDto, DomApartment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // RentalApartment
        CreateMap<RentalApartment, RentalApartmentDto>();
        CreateMap<RentalApartmentCreateDto, RentalApartment>();
        CreateMap<RentalApartmentUpdateDto, RentalApartment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // User
        CreateMap<AppUser, UserManageDto>();
    }
}
