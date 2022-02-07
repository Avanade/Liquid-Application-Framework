using PROJECTNAME.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PROJECTNAME.Repository.Configurations
{
    public class ENTITYNAMEConfiguration : IEntityTypeConfiguration<ENTITYNAMEEntity>
    {
        public void Configure(EntityTypeBuilder<ENTITYNAMEEntity> builder)
        {
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.HasKey(o => o.Id);
        }
    }
}
