// ----------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a compiler emitter: FACTS.Framework.Analysis.Generators.DAL.EntityEmitter
//
// Changes to this file may cause incorrect behavior and will be lost when the code is regenerated.
// </auto-generated>
// ----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using PFML.Shared.Model.DbDtos;
using FACTS.Framework.DAL;

namespace PFML.DAL.Model.DbEntities
{

    /// <summary>[EmployerLiability]</summary>
    [Table("EmployerLiability", Schema="dbo")]
    public class EmployerLiability : BaseEntity
    {

        /// <summary>[CreateDateTime]</summary>
        [Required]
        [Column("CreateDateTime")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>[CreateUserId]</summary>
        [Required]
        [MaxLength(50)]
        [Column("CreateUserId")]
        public string CreateUserId { get; set; }

        /// <summary>[EmployerId]</summary>
        [Key]
        [Required]
        [Column("EmployerId", Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployerId { get; set; }

        /// <summary>[GrossWagesPaid]</summary>
        [Column("GrossWagesPaid")]
        public decimal? GrossWagesPaid { get; set; }

        /// <summary>[HasEmployed10In20Weeks]</summary>
        [Column("HasEmployed10In20Weeks")]
        public bool? HasEmployed10In20Weeks { get; set; }

        /// <summary>[HasEmployed1In20Weeks]</summary>
        [Column("HasEmployed1In20Weeks")]
        public bool? HasEmployed1In20Weeks { get; set; }

        /// <summary>[HasPaid1KDomesticWages]</summary>
        [Column("HasPaid1KDomesticWages")]
        public bool? HasPaid1KDomesticWages { get; set; }

        /// <summary>[HasPaid20KAgriculturalLaborWages]</summary>
        [Column("HasPaid20KAgriculturalLaborWages")]
        public bool? HasPaid20KAgriculturalLaborWages { get; set; }

        /// <summary>[HasPaid450RegularWages]</summary>
        [Column("HasPaid450RegularWages")]
        public bool? HasPaid450RegularWages { get; set; }

        /// <summary>[LiabilityAmountMetQuarter]</summary>
        [MaxLength(20)]
        [Column("LiabilityAmountMetQuarter")]
        public string LiabilityAmountMetQuarter { get; set; }

        /// <summary>[LiabilityAmountMetYear]</summary>
        [Column("LiabilityAmountMetYear")]
        public short? LiabilityAmountMetYear { get; set; }

        /// <summary>[UpdateDateTime]</summary>
        [Required]
        [Column("UpdateDateTime")]
        [ConcurrencyCheck]
        public DateTime UpdateDateTime { get; set; }

        /// <summary>[UpdateNumber]</summary>
        [Column("UpdateNumber")]
        public int? UpdateNumber { get; set; }

        /// <summary>[UpdateProcess]</summary>
        [MaxLength(100)]
        [Column("UpdateProcess")]
        public string UpdateProcess { get; set; }

        /// <summary>[UpdateUserId]</summary>
        [Required]
        [MaxLength(50)]
        [Column("UpdateUserId")]
        public string UpdateUserId { get; set; }

        public virtual Employer Employer { get; set; }

        public override void SetAuditFields(EntityState state)
        {
            string username = FACTS.Framework.Service.Context.UserName ?? "UNKNOWN";
            DateTime timestamp = FACTS.Framework.Utility.DateTimeUtil.Now;

            if (state == EntityState.Added)
            {
                CreateUserId = username;
                CreateDateTime = new System.Data.SqlTypes.SqlDateTime(timestamp).Value;
                UpdateUserId = username;
                UpdateDateTime = new System.Data.SqlTypes.SqlDateTime(timestamp).Value;
                UpdateNumber = 0;
                UpdateProcess = FACTS.Framework.Utility.StringUtil.CapLength(FACTS.Framework.Service.Context.Process.ToString(), 100);
            }
            else if (state == EntityState.Modified)
            {
                UpdateUserId = username;
                UpdateDateTime = new System.Data.SqlTypes.SqlDateTime(timestamp).Value;
                UpdateNumber = (UpdateNumber ?? 0) + 1;
                UpdateProcess = FACTS.Framework.Utility.StringUtil.CapLength(FACTS.Framework.Service.Context.Process.ToString(), 100);
            }
        }

        internal static void ModelCreating(DbModelBuilder builder)
        {
            builder.Entity<EmployerLiability>().Property(x => x.CreateUserId).IsUnicode(false);
            builder.Entity<EmployerLiability>().Property(x => x.GrossWagesPaid).HasPrecision(14, 2);
            builder.Entity<EmployerLiability>().Property(x => x.LiabilityAmountMetQuarter).IsUnicode(false);
            builder.Entity<EmployerLiability>().Property(x => x.UpdateProcess).IsUnicode(false);
            builder.Entity<EmployerLiability>().Property(x => x.UpdateUserId).IsUnicode(false);
            builder.Entity<EmployerLiability>().HasRequired(x => x.Employer).WithRequiredDependent(x => x.EmployerLiability);
        }

        /// <summary>Convert from EmployerLiability entity to DTO</summary>
        /// <param name="dbContext">DB Context to use for setting DTO state</param>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <param name="entityDtos">Used internally to track which entities have been converted to DTO's already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant EmployerLiability DTO</returns>
        public EmployerLiabilityDto ToDtoDeep(FACTS.Framework.DAL.DbContext dbContext, EmployerLiabilityDto dto = null, Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = null)
        {
            entityDtos = entityDtos ?? new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            if (entityDtos.ContainsKey(this))
            {
                return (EmployerLiabilityDto)entityDtos[this];
            }

            dto = ToDto(dto);
            entityDtos.Add(this, dto);

            System.Data.Entity.Infrastructure.DbEntityEntry<EmployerLiability> entry = dbContext?.Entry(this);
            dto.IsNew = (entry?.State == EntityState.Added);
            dto.IsDeleted = (entry?.State == EntityState.Deleted);

            if (entry?.Reference(x => x.Employer)?.IsLoaded == true)
            {
                dto.Employer = Employer?.ToDtoDeep(dbContext, entityDtos: entityDtos);
            }

            return dto;
        }

        /// <summary>Convert from EmployerLiability entity to DTO w/o checking entity state or entity navigation</summary>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <returns>Resultant EmployerLiability DTO</returns>
        public EmployerLiabilityDto ToDto(EmployerLiabilityDto dto = null)
        {
            dto = dto ?? new EmployerLiabilityDto();
            dto.IsNew = false;

            dto.CreateDateTime = CreateDateTime;
            dto.CreateUserId = CreateUserId;
            dto.EmployerId = EmployerId;
            dto.GrossWagesPaid = GrossWagesPaid;
            dto.HasEmployed10In20Weeks = HasEmployed10In20Weeks;
            dto.HasEmployed1In20Weeks = HasEmployed1In20Weeks;
            dto.HasPaid1KDomesticWages = HasPaid1KDomesticWages;
            dto.HasPaid20KAgriculturalLaborWages = HasPaid20KAgriculturalLaborWages;
            dto.HasPaid450RegularWages = HasPaid450RegularWages;
            dto.LiabilityAmountMetQuarter = LiabilityAmountMetQuarter;
            dto.LiabilityAmountMetYear = LiabilityAmountMetYear;
            dto.UpdateDateTime = UpdateDateTime;
            dto.UpdateNumber = UpdateNumber;
            dto.UpdateProcess = UpdateProcess;
            dto.UpdateUserId = UpdateUserId;

            return dto;
        }

        /// <summary>Convert from EmployerLiability DTO to entity</summary>
        /// <param name="dbContext">DB Context to use for attaching entity</param>
        /// <param name="dto">DTO to convert from</param>
        /// <param name="dtoEntities">Used internally to track which dtos have been converted to entites already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant EmployerLiability entity</returns>
        public static EmployerLiability FromDto(FACTS.Framework.DAL.DbContext dbContext, EmployerLiabilityDto dto, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities = null)
        {
            dtoEntities = dtoEntities ?? new Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity>();
            if (dtoEntities.ContainsKey(dto))
            {
                return (EmployerLiability)dtoEntities[dto];
            }

            EmployerLiability entity = new EmployerLiability();
            dtoEntities.Add(dto, entity);
            FromDtoSet(dbContext, dto, entity, dtoEntities);

            if (dbContext != null)
            {
                dbContext.Entry(entity).State = (dto.IsNew ? EntityState.Added : (dto.IsDeleted ? EntityState.Deleted : EntityState.Modified));
            }

            return entity;
        }

        protected static void FromDtoSet(FACTS.Framework.DAL.DbContext dbContext, EmployerLiabilityDto dto, EmployerLiability entity, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities)
        {
            entity.CreateDateTime = dto.CreateDateTime;
            entity.CreateUserId = dto.CreateUserId;
            entity.EmployerId = dto.EmployerId;
            entity.GrossWagesPaid = dto.GrossWagesPaid;
            entity.HasEmployed10In20Weeks = dto.HasEmployed10In20Weeks;
            entity.HasEmployed1In20Weeks = dto.HasEmployed1In20Weeks;
            entity.HasPaid1KDomesticWages = dto.HasPaid1KDomesticWages;
            entity.HasPaid20KAgriculturalLaborWages = dto.HasPaid20KAgriculturalLaborWages;
            entity.HasPaid450RegularWages = dto.HasPaid450RegularWages;
            entity.LiabilityAmountMetQuarter = dto.LiabilityAmountMetQuarter;
            entity.LiabilityAmountMetYear = dto.LiabilityAmountMetYear;
            entity.UpdateDateTime = dto.UpdateDateTime;
            entity.UpdateNumber = dto.UpdateNumber;
            entity.UpdateProcess = dto.UpdateProcess;
            entity.UpdateUserId = dto.UpdateUserId;

            entity.Employer = (dto.Employer == null) ? null : Employer.FromDto(dbContext, dto.Employer, dtoEntities);
        }

    }

    /// <summary>Extension methods related to EmployerLiability entity</summary>
    public static class EmployerLiabilityExtension
    {

        /// <summary>Convert IEnumerable EmployerLiability to list of DTOs</summary>
        /// <param name="entities">IEnumerable EmployerLiabilities</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO EmployerLiabilities</returns>
        public static List<EmployerLiabilityDto> ToDtoListDeep(this IEnumerable<EmployerLiability> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<EmployerLiabilityDto> dtos = new List<EmployerLiabilityDto>();
            foreach (EmployerLiability entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerLiability to list of DTOs</summary>
        /// <param name="entities">L2E EmployerLiabilities</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO EmployerLiabilities</returns>
        public static List<EmployerLiabilityDto> ToDtoListDeep(this IQueryable<EmployerLiability> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<EmployerLiabilityDto> dtos = new List<EmployerLiabilityDto>();
            foreach (EmployerLiability entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerLiability to list of customized DTOs</summary>
        /// <typeparam name="T">Custom DTO derived from EmployerLiabilityDto</typeparam>
        /// <param name="entities">L2E EmployerLiabilities</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO customized EmployerLiabilities</returns>
        public static List<T> ToDtoListDeep<T>(this IQueryable<EmployerLiability> entities, FACTS.Framework.DAL.DbContext dbContext) where T : EmployerLiabilityDto, new()
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<T> dtos = new List<T>();
            foreach (EmployerLiability entity in entities)
            {
                dtos.Add((T)entity.ToDtoDeep(dbContext, new T(), entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert IEnumerable EmployerLiability to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">IEnumerable EmployerLiabilities</param>
        /// <returns>List of DTO EmployerLiabilities</returns>
        public static List<EmployerLiabilityDto> ToDtoList(this IEnumerable<EmployerLiability> entities)
        {
            List<EmployerLiabilityDto> dtos = new List<EmployerLiabilityDto>();
            foreach (EmployerLiability entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerLiability to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">L2E EmployerLiabilities</param>
        /// <returns>List of DTO EmployerLiabilities</returns>
        public static List<EmployerLiabilityDto> ToDtoList(this IQueryable<EmployerLiability> entities)
        {
            List<EmployerLiabilityDto> dtos = new List<EmployerLiabilityDto>();
            foreach (EmployerLiability entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerLiability to list of customized DTOs w/o checking entity state or entity navigation</summary>
        /// <typeparam name="T">Custom DTO derived from EmployerLiabilityDto</typeparam>
        /// <param name="entities">L2E EmployerLiabilities</param>
        /// <returns>List of DTO customized EmployerLiabilities</returns>
        public static List<T> ToDtoList<T>(this IQueryable<EmployerLiability> entities) where T : EmployerLiabilityDto, new()
        {
            List<T> dtos = new List<T>();
            foreach (EmployerLiability entity in entities)
            {
                dtos.Add((T)entity.ToDto(new T()));
            }
            return dtos;
        }

    }

}
