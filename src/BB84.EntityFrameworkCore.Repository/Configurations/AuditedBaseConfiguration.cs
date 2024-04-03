using System.Diagnostics.CodeAnalysis;

using BB84.EntityFrameworkCore.Models.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB84.EntityFrameworkCore.Repository.Configurations;

/// <summary>
/// The audited base configuration class.
/// </summary>
/// <inheritdoc cref="IEntityTypeConfiguration{TEntity}"/>
/// <inheritdoc cref="IAuditedModel{TKey, TCreated, TModified}"/>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, entity type configuration.")]
public abstract class AuditedBaseConfiguration<TEntity, TKey, TCreated, TModified> : IEntityTypeConfiguration<TEntity>
  where TEntity : class, IAuditedModel<TKey, TCreated, TModified>
  where TKey : IEquatable<TKey>
{
  /// <inheritdoc/>
  public virtual void Configure(EntityTypeBuilder<TEntity> builder)
  {
    builder.HasKey(e => e.Id)
      .IsClustered(false);

    builder.Property(e => e.Id)
      .HasDefaultValueSql("NEWID()")
      .ValueGeneratedOnAdd()
      .HasColumnOrder(1);

    builder.Property(e => e.Timestamp)
      .IsRowVersion()
      .HasColumnOrder(2);

    builder.Property(e => e.CreatedBy)
      .IsRequired()
      .HasColumnOrder(3);

    builder.Property(e => e.ModifiedBy)
      .IsRequired(false)
      .HasColumnOrder(4);
  }
}

/// <inheritdoc/>
public abstract class AuditedBaseConfiguration<TEntity> : AuditedBaseConfiguration<TEntity, Guid, string, string?>,
  IEntityTypeConfiguration<TEntity> where TEntity : class, IAuditedModel
{ }
