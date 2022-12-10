using Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Maps;

public class DiscountMap
{
    public DiscountMap(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(x => x.Id);
    }
}