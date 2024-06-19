
using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommonTestUtilities.Mapper;
public static class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(c =>
        {
            c.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}