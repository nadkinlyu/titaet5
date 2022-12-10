using Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Maps;

public class CardMap
{
    public CardMap(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(x => x.PersonId);
        builder
            .HasOne(x => x.Person)
            .WithOne(x => x.Card)
            .HasForeignKey<Card>(x => x.PersonId);
        builder
            .HasOne(x => x.Discount)
            .WithMany(x => x.Cards)
            .HasForeignKey(x => x.DiscontId);
    }
}
