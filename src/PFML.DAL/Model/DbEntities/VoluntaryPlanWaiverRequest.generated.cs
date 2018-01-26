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

    /// <summary>[VoluntaryPlanWaiverRequest]</summary>
    [Table("VoluntaryPlanWaiverRequest", Schema="dbo")]
    public class VoluntaryPlanWaiverRequest : Form
    {



        /// <summary>[EmployerId]</summary>
        [Required]
        [Column("EmployerId")]
        public int EmployerId { get; set; }

        /// <summary>[EndDate]</summary>
        [Required]
        [Column("EndDate")]
        public DateTime EndDate { get; set; }


        /// <summary>[IsVoluntaryPlanWaiverRequestAcknowledged]</summary>
        [Required]
        [Column("IsVoluntaryPlanWaiverRequestAcknowledged")]
        public bool IsVoluntaryPlanWaiverRequestAcknowledged { get; set; }

        /// <summary>[StartDate]</summary>
        [Required]
        [Column("StartDate")]
        public DateTime StartDate { get; set; }





        public virtual Employer Employer { get; set; }

        private ICollection<VoluntaryPlanWaiverRequestType> voluntaryPlanWaiverRequestTypes { get; set; }
        public virtual ICollection<VoluntaryPlanWaiverRequestType> VoluntaryPlanWaiverRequestTypes { get { return voluntaryPlanWaiverRequestTypes ?? (voluntaryPlanWaiverRequestTypes = new Collection<VoluntaryPlanWaiverRequestType>()); } protected set { voluntaryPlanWaiverRequestTypes = value; } }

        public override void SetAuditFields(EntityState state)
        {
            base.SetAuditFields(state);
        }

        internal static void ModelCreating(DbModelBuilder builder)
        {
            builder.Entity<VoluntaryPlanWaiverRequest>().HasRequired(x => x.Employer).WithMany(x => x.VoluntaryPlanWaiverRequests).HasForeignKey(x => x.EmployerId);
        }

        /// <summary>Convert from VoluntaryPlanWaiverRequest entity to DTO</summary>
        /// <param name="dbContext">DB Context to use for setting DTO state</param>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <param name="entityDtos">Used internally to track which entities have been converted to DTO's already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant VoluntaryPlanWaiverRequest DTO</returns>
        public VoluntaryPlanWaiverRequestDto ToDtoDeep(FACTS.Framework.DAL.DbContext dbContext, VoluntaryPlanWaiverRequestDto dto = null, Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = null)
        {
            entityDtos = entityDtos ?? new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            if (entityDtos.ContainsKey(this))
            {
                return (VoluntaryPlanWaiverRequestDto)entityDtos[this];
            }

            dto = (VoluntaryPlanWaiverRequestDto)base.ToDtoDeep(dbContext, dto ?? new VoluntaryPlanWaiverRequestDto(), entityDtos);
            ToDto(dto);

            System.Data.Entity.Infrastructure.DbEntityEntry<VoluntaryPlanWaiverRequest> entry = dbContext?.Entry(this);
            dto.IsNew = (entry?.State == EntityState.Added);
            dto.IsDeleted = (entry?.State == EntityState.Deleted);

            if (entry?.Reference(x => x.Employer)?.IsLoaded == true)
            {
                dto.Employer = Employer?.ToDtoDeep(dbContext, entityDtos: entityDtos);
            }
            if (entry?.Collection(x => x.VoluntaryPlanWaiverRequestTypes)?.IsLoaded == true)
            {
                foreach (VoluntaryPlanWaiverRequestType voluntaryPlanWaiverRequestType in VoluntaryPlanWaiverRequestTypes)
                {
                    dto.VoluntaryPlanWaiverRequestTypes.Add(voluntaryPlanWaiverRequestType.ToDtoDeep(dbContext, entityDtos: entityDtos));
                }
            }

            return dto;
        }

        /// <summary>Convert from VoluntaryPlanWaiverRequest entity to DTO w/o checking entity state or entity navigation</summary>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <returns>Resultant VoluntaryPlanWaiverRequest DTO</returns>
        public VoluntaryPlanWaiverRequestDto ToDto(VoluntaryPlanWaiverRequestDto dto = null)
        {
            dto = dto ?? new VoluntaryPlanWaiverRequestDto();
            base.ToDto(dto);

            dto.EmployerId = EmployerId;
            dto.EndDate = EndDate;
            dto.IsVoluntaryPlanWaiverRequestAcknowledged = IsVoluntaryPlanWaiverRequestAcknowledged;
            dto.StartDate = StartDate;

            return dto;
        }

        /// <summary>Convert from VoluntaryPlanWaiverRequest DTO to entity</summary>
        /// <param name="dbContext">DB Context to use for attaching entity</param>
        /// <param name="dto">DTO to convert from</param>
        /// <param name="dtoEntities">Used internally to track which dtos have been converted to entites already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant VoluntaryPlanWaiverRequest entity</returns>
        public static VoluntaryPlanWaiverRequest FromDto(FACTS.Framework.DAL.DbContext dbContext, VoluntaryPlanWaiverRequestDto dto, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities = null)
        {
            dtoEntities = dtoEntities ?? new Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity>();
            if (dtoEntities.ContainsKey(dto))
            {
                return (VoluntaryPlanWaiverRequest)dtoEntities[dto];
            }

            VoluntaryPlanWaiverRequest entity = new VoluntaryPlanWaiverRequest();
            dtoEntities.Add(dto, entity);
            Form.FromDtoSet(dbContext, dto, entity, dtoEntities);
            FromDtoSet(dbContext, dto, entity, dtoEntities);

            if (dbContext != null)
            {
                dbContext.Entry(entity).State = (dto.IsNew ? EntityState.Added : (dto.IsDeleted ? EntityState.Deleted : EntityState.Modified));
            }

            return entity;
        }

        protected static void FromDtoSet(FACTS.Framework.DAL.DbContext dbContext, VoluntaryPlanWaiverRequestDto dto, VoluntaryPlanWaiverRequest entity, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities)
        {
            entity.EmployerId = dto.EmployerId;
            entity.EndDate = dto.EndDate;
            entity.IsVoluntaryPlanWaiverRequestAcknowledged = dto.IsVoluntaryPlanWaiverRequestAcknowledged;
            entity.StartDate = dto.StartDate;

            entity.Employer = (dto.Employer == null) ? null : Employer.FromDto(dbContext, dto.Employer, dtoEntities);
            if (dto.VoluntaryPlanWaiverRequestTypes != null)
            {
                foreach (VoluntaryPlanWaiverRequestTypeDto voluntaryPlanWaiverRequestType in dto.VoluntaryPlanWaiverRequestTypes)
                {
                    entity.VoluntaryPlanWaiverRequestTypes.Add(DbEntities.VoluntaryPlanWaiverRequestType.FromDto(dbContext, voluntaryPlanWaiverRequestType, dtoEntities));
                }
            }
        }

    }

    /// <summary>Extension methods related to VoluntaryPlanWaiverRequest entity</summary>
    public static class VoluntaryPlanWaiverRequestExtension
    {

        /// <summary>Convert IEnumerable VoluntaryPlanWaiverRequest to list of DTOs</summary>
        /// <param name="entities">IEnumerable VoluntaryPlanWaiverRequests</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequests</returns>
        public static List<VoluntaryPlanWaiverRequestDto> ToDtoListDeep(this IEnumerable<VoluntaryPlanWaiverRequest> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<VoluntaryPlanWaiverRequestDto> dtos = new List<VoluntaryPlanWaiverRequestDto>();
            foreach (VoluntaryPlanWaiverRequest entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequest to list of DTOs</summary>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequests</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequests</returns>
        public static List<VoluntaryPlanWaiverRequestDto> ToDtoListDeep(this IQueryable<VoluntaryPlanWaiverRequest> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<VoluntaryPlanWaiverRequestDto> dtos = new List<VoluntaryPlanWaiverRequestDto>();
            foreach (VoluntaryPlanWaiverRequest entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequest to list of customized DTOs</summary>
        /// <typeparam name="T">Custom DTO derived from VoluntaryPlanWaiverRequestDto</typeparam>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequests</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO customized VoluntaryPlanWaiverRequests</returns>
        public static List<T> ToDtoListDeep<T>(this IQueryable<VoluntaryPlanWaiverRequest> entities, FACTS.Framework.DAL.DbContext dbContext) where T : VoluntaryPlanWaiverRequestDto, new()
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<T> dtos = new List<T>();
            foreach (VoluntaryPlanWaiverRequest entity in entities)
            {
                dtos.Add((T)entity.ToDtoDeep(dbContext, new T(), entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert IEnumerable VoluntaryPlanWaiverRequest to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">IEnumerable VoluntaryPlanWaiverRequests</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequests</returns>
        public static List<VoluntaryPlanWaiverRequestDto> ToDtoList(this IEnumerable<VoluntaryPlanWaiverRequest> entities)
        {
            List<VoluntaryPlanWaiverRequestDto> dtos = new List<VoluntaryPlanWaiverRequestDto>();
            foreach (VoluntaryPlanWaiverRequest entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequest to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequests</param>
        /// <returns>List of DTO VoluntaryPlanWaiverRequests</returns>
        public static List<VoluntaryPlanWaiverRequestDto> ToDtoList(this IQueryable<VoluntaryPlanWaiverRequest> entities)
        {
            List<VoluntaryPlanWaiverRequestDto> dtos = new List<VoluntaryPlanWaiverRequestDto>();
            foreach (VoluntaryPlanWaiverRequest entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E VoluntaryPlanWaiverRequest to list of customized DTOs w/o checking entity state or entity navigation</summary>
        /// <typeparam name="T">Custom DTO derived from VoluntaryPlanWaiverRequestDto</typeparam>
        /// <param name="entities">L2E VoluntaryPlanWaiverRequests</param>
        /// <returns>List of DTO customized VoluntaryPlanWaiverRequests</returns>
        public static List<T> ToDtoList<T>(this IQueryable<VoluntaryPlanWaiverRequest> entities) where T : VoluntaryPlanWaiverRequestDto, new()
        {
            List<T> dtos = new List<T>();
            foreach (VoluntaryPlanWaiverRequest entity in entities)
            {
                dtos.Add((T)entity.ToDto(new T()));
            }
            return dtos;
        }

    }

}
