using PROJECTNAME.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PROJECTNAME.Repository.Configurations
{
    public class ENTITYNAMEConfiguration : IEntityTypeConfiguration<ENTITYNAME>
    {
        public void Configure(EntityTypeBuilder<ENTITYNAME> builder)
        {
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.HasKey(o => o.Id);
        }
    }
}
