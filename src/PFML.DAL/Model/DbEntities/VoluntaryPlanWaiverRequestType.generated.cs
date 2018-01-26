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

    /// <summary>[VoluntaryPlanWaiverRequestType]</summary>
    [Table("VoluntaryPlanWaiverRequestType", Schema="dbo")]
    public class VoluntaryPlanWaiverRequestType : BaseEntity
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

        /// <summary>[DurationInWeeksCode]</summary>
        [Required]
        [MaxLength(20)]
        [Column("DurationInWeeksCode")]
        public string DurationInWeeksCode { get; set; }

        /// <summary>[FormId]</summary>
        [Key]
        [Required]
        [Column("FormId", Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FormId { get; set; }

        /// <summary>[LeaveTypeCode]</summary>
        [Key]
        [Required]
        [MaxLength(20)]
        [Column("LeaveTypeCode", Order=2)]
        public string LeaveTypeCode { get; set; }

        /// <summary>[PercentagePaid]</summary>
        [Required]
        [Column("PercentagePaid")]
        public decimal PercentagePaid { get; set; }

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

        public virtual VoluntaryPlanWaiverRequest VoluntaryPlanWaiverRequest { get; set; }

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
            builder.Entity<VoluntaryPlanWaiverRequestType>().Property(x => x.CreateUserId).IsUnicode(false);
            builder.Entity<VoluntaryPlanWaiverRequestType>().Property(x => x.DurationInWeeksCode).IsUnicode(false);
            builder.Entity<VoluntaryPlanWaiverRequestType>().Property(x => x.LeaveTypeCode).IsUnicode(false);
            builder.Entity<VoluntaryPlanWaiverRequestType>().Property(x => x.PercentagePaid).HasPrecision(5, 2);
            builder.Entity<VoluntaryPlanWaiverRequestType>().Property(x => x.UpdateProcess).IsUnicode(false);
            builder.Entity<VoluntaryPlanWaiverRequestType>().Property(x => x.UpdateUserId).IsUnicode(false);
            builder.Entity<VoluntaryPlanWaiverRequestType>().HasRequired(x => x.VoluntaryPlanWaiverRequest).WithMany(x => x.VoluntaryPlanWaiverRequestTypes).HasForeignKey(x => x.FormId);
        }

        /// <summary>Convert from VoluntaryPlanWaiverRequestType entity to DTO</summary>
        /// <param name="dbContext">DB Context to use for setting DTO state</param>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <param name="entityDtos">Used internally to track which entities have been converted to DTO's already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant VoluntaryPlanWaiverRequestType DTO</returns>
        public VoluntaryPlanWaiverRequestTypeDto ToDtoDeep(FACTS.Framework.DAL.DbContext dbContext, VoluntaryPlanWaiverRequestTypeDto dto = null, Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = null)
        {
            entityDtos = entityDtos ?? new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            if (entityDtos.ContainsKey(this))
            {
                return (VoluntaryPlanWaiverRequestTypeDto)entityDtos[this];
            }

            dto = ToDto(dto);
            entityDtos.Add(this, dto);

            System.Data.Entity.Infrastructure.DbEntityEntry<VoluntaryPlanWaiverRequestType> entry = dbContext?.Entry(this);
            dto.IsNew = (entry?.State == EntityState.Added);
            dto.IsDeleted = (entry?.State == EntityState.Deleted);

            if (entry?.Reference(x => x.VoluntaryPlanWaiverRequest)?.IsLoaded == true)
            {
                dto.VoluntaryPlanWaiverRequest = VoluntaryPlanWaiverRequest?.ToDtoDeep(dbContext, entityDtos: entityDtos);
            }

            return dto;
        }

        /// <summary>Convert from VoluntaryPlanWaiverRequestType entity to DTO w/o checking entity state or entity navigation</summary>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <returns>Resultant VoluntaryPlanWaiverRequestType DTO</returns>
        public VoluntaryPlanWaiverRequestTypeDto ToDto(VoluntaryPlanWaiverRequestTypeDto dto = null)
        {
            dto = dto ?? new VoluntaryPlanWaiverRequestTypeDto();
            dto.IsNew = false;

            dto.CreateDateTime = CreateDateTime;
            dto.CreateUserId = CreateUserId;
            dto.DurationInWeeksCode = DurationInWeeksCode;
            dto.FormId = FormId;
            dto.LeaveTypeCode = LeaveTypeCode;
            dto.PercentagePaid = PercentagePaid;
            dto.UpdateDateTime = UpdateDateTime;
            dto.UpdateNumber = UpdateNumber;
            dto.UpdateProcess = UpdateProcess;
            dto.UpdateUserId = UpdateUserId;

            return dto;
        }

        /// <summary>Convert from VoluntaryPlanWaiverRequestType DTO to entity</summary>
        /// <param name="dbContext">DB Context to use for attaching entity</param>
        /// <param name="dto">DTO to convert from</param>
        /// <param name="dtoEntities">Used internally to track which dtos have been converted to entites already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant VoluntaryPlanWaiverRequestType entity</returns>
        public static VoluntaryPlanWaiverRequestType FromDto(FACTS.Framework.DAL.DbContext dbContext, VoluntaryPlanWaiverRequestTypeDto dto, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities = null)
        {
            dtoEntities = dtoEntities ?? new Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity>();
            if (dtoEntities.ContainsKey(dto))
            {
                return (VoluntaryPlanWaiverRequestType)dtoEntities[dto];
            }

            VoluntaryPlanWaiverRequestType entity = new VoluntaryPlanWaiverRequestType();
            dtoEntities.Add(dto, entity);
            FromDtoSet(dbContext, dto, entity, dtoEntities);

            if (dbContext != null)
            {
                dbContext.Entry(entity).State = (dto.IsNew ? EntityState.Added : (dto.IsDeleted ? EntityState.Deleted : EntityState.Modified));
            }

            return entity;
        }

        protected static void FromDtoSet(FACTS.Framework.DAL.DbContext dbContext, VoluntaryPlanWaiverRequestTypeDto dto, VoluntaryPlanWaiverRequestType entity, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities)
        {
            entity.CreateDateTime = dto.CreateDateTime;
            entity.CreateUserId = dto.CreateUserId;
            entity.DurationInWeeksCode = dto.DurationInWeeksCode;
            entity.FormId = dto.FormId;
            entity.LeaveTypeCode = dto.LeaveTypeCode;
            entity.PercentagePaid = dto.PercentagePaid;
            entity.UpdateDateTime = dto.UpdateDateTime;
            entity.UpdateNumber = dto.UpdateNumber;
            entity.UpdateProcess = dto.UpdateProcess;
            entity.UpdateUserId = dto.UpdateUserId;

            entity.VoluntaryPlanWaiverRequest = (dto.VoluntaryPlanWaiverRequest == null) ? null : VoluntaryPlanWaiverRequest.FromDto(dbContext, dto.VoluntaryPlanWaiverRequest, dtoEntities);
        }

    }

    /// <summary>Extension methods related to VoluntaryPlanWaiverRequestType entity</summary>
    public static class VoluntaryPlanWaiverRequestTypeExtension
    {

        /// <summary>Convert IEnumerable VoluntaryPlanWaiverRequestType to list of DTOs</summary>
        /// <param name="entities">IEnumerable VoluntaryPlanWaiverRequestTypes</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequestTypes</returns>
        public static List<VoluntaryPlanWaiverRequestTypeDto> ToDtoListDeep(this IEnumerable<VoluntaryPlanWaiverRequestType> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<VoluntaryPlanWaiverRequestTypeDto> dtos = new List<VoluntaryPlanWaiverRequestTypeDto>();
            foreach (VoluntaryPlanWaiverRequestType entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequestType to list of DTOs</summary>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequestTypes</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequestTypes</returns>
        public static List<VoluntaryPlanWaiverRequestTypeDto> ToDtoListDeep(this IQueryable<VoluntaryPlanWaiverRequestType> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<VoluntaryPlanWaiverRequestTypeDto> dtos = new List<VoluntaryPlanWaiverRequestTypeDto>();
            foreach (VoluntaryPlanWaiverRequestType entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequestType to list of customized DTOs</summary>
        /// <typeparam name="T">Custom DTO derived from VoluntaryPlanWaiverRequestTypeDto</typeparam>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequestTypes</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO customized VoluntaryPlanWaiverRequestTypes</returns>
        public static List<T> ToDtoListDeep<T>(this IQueryable<VoluntaryPlanWaiverRequestType> entities, FACTS.Framework.DAL.DbContext dbContext) where T : VoluntaryPlanWaiverRequestTypeDto, new()
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<T> dtos = new List<T>();
            foreach (VoluntaryPlanWaiverRequestType entity in entities)
            {
                dtos.Add((T)entity.ToDtoDeep(dbContext, new T(), entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert IEnumerable VoluntaryPlanWaiverRequestType to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">IEnumerable VoluntaryPlanWaiverRequestTypes</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequestTypes</returns>
        public static List<VoluntaryPlanWaiverRequestTypeDto> ToDtoList(this IEnumerable<VoluntaryPlanWaiverRequestType> entities)
        {
            List<VoluntaryPlanWaiverRequestTypeDto> dtos = new List<VoluntaryPlanWaiverRequestTypeDto>();
            foreach (VoluntaryPlanWaiverRequestType entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequestType to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequestTypes</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequestTypes</returns>
        public static List<VoluntaryPlanWaiverRequestTypeDto> ToDtoList(this IQueryable<VoluntaryPlanWaiverRequestType> entities)
        {
            List<VoluntaryPlanWaiverRequestTypeDto> dtos = new List<VoluntaryPlanWaiverRequestTypeDto>();
            foreach (VoluntaryPlanWaiverRequestType entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequestType to list of customized DTOs w/o checking entity state or entity navigation</summary>
        /// <typeparam name="T">Custom DTO derived from VoluntaryPlanWaiverRequestTypeDto</typeparam>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequestTypes</param>
        /// <returns>List of DTO customized VoluntaryPlanWaiverRequestTypes</returns>
        public static List<T> ToDtoList<T>(this IQueryable<VoluntaryPlanWaiverRequestType> entities) where T : VoluntaryPlanWaiverRequestTypeDto, new()
        {
            List<T> dtos = new List<T>();
            foreach (VoluntaryPlanWaiverRequestType entity in entities)
            {
                dtos.Add((T)entity.ToDto(new T()));
            }
            return dtos;
        }

    }

}
