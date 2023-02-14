using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;

namespace EvergreenAPI.Helper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DiseaseCategory, DiseaseCategoryDTO>().ReverseMap();
            CreateMap<Disease, DiseaseDTO>().ReverseMap();
            CreateMap<MedicineCategory, MedicineCategoryDTO>().ReverseMap();
            CreateMap<Medicine, MedicineDTO>().ReverseMap();
            CreateMap<Treatment, TreatmentDTO>().ReverseMap();
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<Account, UserDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();

        }
    }
}
