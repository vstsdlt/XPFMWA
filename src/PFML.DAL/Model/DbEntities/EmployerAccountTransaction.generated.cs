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

    /// <summary>[EmployerAccountTransaction]</summary>
    [Table("EmployerAccountTransaction", Schema="dbo")]
    public class EmployerAccountTransaction : BaseEntity
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

        /// <summary>[DueDate]</summary>
        [Required]
        [Column("DueDate")]
        public DateTime DueDate { get; set; }

        /// <summary>[EmployerId]</summary>
        [Key]
        [Required]
        [Column("EmployerId", Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployerId { get; set; }

        /// <summary>[OwedAmount]</summary>
        [Required]
        [Column("OwedAmount")]
        public decimal OwedAmount { get; set; }

        /// <summary>[ReportingQuarter]</summary>
        [Key]
        [Required]
        [MaxLength(20)]
        [Column("ReportingQuarter", Order=3)]
        public string ReportingQuarter { get; set; }

        /// <summary>[ReportingYear]</summary>
        [Key]
        [Required]
        [Column("ReportingYear", Order=2)]
        public short ReportingYear { get; set; }

        /// <summary>[StatusCode]</summary>
        [Required]
        [MaxLength(20)]
        [Column("StatusCode")]
        public string StatusCode { get; set; }

        /// <summary>[ThresholdAmount]</summary>
        [Required]
        [Column("ThresholdAmount")]
        public decimal ThresholdAmount { get; set; }

        /// <summary>[TransactionSeqNo]</summary>
        [Key]
        [Required]
        [Column("TransactionSeqNo", Order=5)]
        public int TransactionSeqNo { get; set; }

        /// <summary>[TypeCode]</summary>
        [Key]
        [Required]
        [MaxLength(20)]
        [Column("TypeCode", Order=4)]
        public string TypeCode { get; set; }

        /// <summary>[UnpaidAmount]</summary>
        [Required]
        [Column("UnpaidAmount")]
        public decimal UnpaidAmount { get; set; }

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
            builder.Entity<EmployerAccountTransaction>().Property(x => x.CreateUserId).IsUnicode(false);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.OwedAmount).HasPrecision(12, 2);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.ReportingQuarter).IsUnicode(false);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.StatusCode).IsUnicode(false);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.ThresholdAmount).HasPrecision(12, 2);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.TypeCode).IsUnicode(false);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.UnpaidAmount).HasPrecision(12, 2);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.UpdateProcess).IsUnicode(false);
            builder.Entity<EmployerAccountTransaction>().Property(x => x.UpdateUserId).IsUnicode(false);
            builder.Entity<EmployerAccountTransaction>().HasRequired(x => x.Employer).WithMany(x => x.EmployerAccountTransactions).HasForeignKey(x => x.EmployerId);
        }

        /// <summary>Convert from EmployerAccountTransaction entity to DTO</summary>
        /// <param name="dbContext">DB Context to use for setting DTO state</param>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <param name="entityDtos">Used internally to track which entities have been converted to DTO's already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant EmployerAccountTransaction DTO</returns>
        public EmployerAccountTransactionDto ToDtoDeep(FACTS.Framework.DAL.DbContext dbContext, EmployerAccountTransactionDto dto = null, Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = null)
        {
            entityDtos = entityDtos ?? new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            if (entityDtos.ContainsKey(this))
            {
                return (EmployerAccountTransactionDto)entityDtos[this];
            }

            dto = ToDto(dto);
            entityDtos.Add(this, dto);

            System.Data.Entity.Infrastructure.DbEntityEntry<EmployerAccountTransaction> entry = dbContext?.Entry(this);
            dto.IsNew = (entry?.State == EntityState.Added);
            dto.IsDeleted = (entry?.State == EntityState.Deleted);

            if (entry?.Reference(x => x.Employer)?.IsLoaded == true)
            {
                dto.Employer = Employer?.ToDtoDeep(dbContext, entityDtos: entityDtos);
            }

            return dto;
        }

        /// <summary>Convert from EmployerAccountTransaction entity to DTO w/o checking entity state or entity navigation</summary>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <returns>Resultant EmployerAccountTransaction DTO</returns>
        public EmployerAccountTransactionDto ToDto(EmployerAccountTransactionDto dto = null)
        {
            dto = dto ?? new EmployerAccountTransactionDto();
            dto.IsNew = false;

            dto.CreateDateTime = CreateDateTime;
            dto.CreateUserId = CreateUserId;
            dto.DueDate = DueDate;
            dto.EmployerId = EmployerId;
            dto.OwedAmount = OwedAmount;
            dto.ReportingQuarter = ReportingQuarter;
            dto.ReportingYear = ReportingYear;
            dto.StatusCode = StatusCode;
            dto.ThresholdAmount = ThresholdAmount;
            dto.TransactionSeqNo = TransactionSeqNo;
            dto.TypeCode = TypeCode;
            dto.UnpaidAmount = UnpaidAmount;
            dto.UpdateDateTime = UpdateDateTime;
            dto.UpdateNumber = UpdateNumber;
            dto.UpdateProcess = UpdateProcess;
            dto.UpdateUserId = UpdateUserId;

            return dto;
        }

        /// <summary>Convert from EmployerAccountTransaction DTO to entity</summary>
        /// <param name="dbContext">DB Context to use for attaching entity</param>
        /// <param name="dto">DTO to convert from</param>
        /// <param name="dtoEntities">Used internally to track which dtos have been converted to entites already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant EmployerAccountTransaction entity</returns>
        public static EmployerAccountTransaction FromDto(FACTS.Framework.DAL.DbContext dbContext, EmployerAccountTransactionDto dto, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities = null)
        {
            dtoEntities = dtoEntities ?? new Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity>();
            if (dtoEntities.ContainsKey(dto))
            {
                return (EmployerAccountTransaction)dtoEntities[dto];
            }

            EmployerAccountTransaction entity = new EmployerAccountTransaction();
            dtoEntities.Add(dto, entity);
            FromDtoSet(dbContext, dto, entity, dtoEntities);

            if (dbContext != null)
            {
                dbContext.Entry(entity).State = (dto.IsNew ? EntityState.Added : (dto.IsDeleted ? EntityState.Deleted : EntityState.Modified));
            }

            return entity;
        }

        protected static void FromDtoSet(FACTS.Framework.DAL.DbContext dbContext, EmployerAccountTransactionDto dto, EmployerAccountTransaction entity, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities)
        {
            entity.CreateDateTime = dto.CreateDateTime;
            entity.CreateUserId = dto.CreateUserId;
            entity.DueDate = dto.DueDate;
            entity.EmployerId = dto.EmployerId;
            entity.OwedAmount = dto.OwedAmount;
            entity.ReportingQuarter = dto.ReportingQuarter;
            entity.ReportingYear = dto.ReportingYear;
            entity.StatusCode = dto.StatusCode;
            entity.ThresholdAmount = dto.ThresholdAmount;
            entity.TransactionSeqNo = dto.TransactionSeqNo;
            entity.TypeCode = dto.TypeCode;
            entity.UnpaidAmount = dto.UnpaidAmount;
            entity.UpdateDateTime = dto.UpdateDateTime;
            entity.UpdateNumber = dto.UpdateNumber;
            entity.UpdateProcess = dto.UpdateProcess;
            entity.UpdateUserId = dto.UpdateUserId;

            entity.Employer = (dto.Employer == null) ? null : Employer.FromDto(dbContext, dto.Employer, dtoEntities);
        }

    }

    /// <summary>Extension methods related to EmployerAccountTransaction entity</summary>
    public static class EmployerAccountTransactionExtension
    {

        /// <summary>Convert IEnumerable EmployerAccountTransaction to list of DTOs</summary>
        /// <param name="entities">IEnumerable EmployerAccountTransactions</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO EmployerAccountTransactions</returns>
        public static List<EmployerAccountTransactionDto> ToDtoListDeep(this IEnumerable<EmployerAccountTransaction> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<EmployerAccountTransactionDto> dtos = new List<EmployerAccountTransactionDto>();
            foreach (EmployerAccountTransaction entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerAccountTransaction to list of DTOs</summary>
        /// <param name="entities">L2E EmployerAccountTransactions</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO EmployerAccountTransactions</returns>
        public static List<EmployerAccountTransactionDto> ToDtoListDeep(this IQueryable<EmployerAccountTransaction> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<EmployerAccountTransactionDto> dtos = new List<EmployerAccountTransactionDto>();
            foreach (EmployerAccountTransaction entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerAccountTransaction to list of customized DTOs</summary>
        /// <typeparam name="T">Custom DTO derived from EmployerAccountTransactionDto</typeparam>
        /// <param name="entities">L2E EmployerAccountTransactions</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO customized EmployerAccountTransactions</returns>
        public static List<T> ToDtoListDeep<T>(this IQueryable<EmployerAccountTransaction> entities, FACTS.Framework.DAL.DbContext dbContext) where T : EmployerAccountTransactionDto, new()
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<T> dtos = new List<T>();
            foreach (EmployerAccountTransaction entity in entities)
            {
                dtos.Add((T)entity.ToDtoDeep(dbContext, new T(), entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert IEnumerable EmployerAccountTransaction to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">IEnumerable EmployerAccountTransactions</param>
        /// <returns>List of DTO EmployerAccountTransactions</returns>
        public static List<EmployerAccountTransactionDto> ToDtoList(this IEnumerable<EmployerAccountTransaction> entities)
        {
            List<EmployerAccountTransactionDto> dtos = new List<EmployerAccountTransactionDto>();
            foreach (EmployerAccountTransaction entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerAccountTransaction to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">L2E EmployerAccountTransactions</param>
        /// <returns>List of DTO EmployerAccountTransactions</returns>
        public static List<EmployerAccountTransactionDto> ToDtoList(this IQueryable<EmployerAccountTransaction> entities)
        {
            List<EmployerAccountTransactionDto> dtos = new List<EmployerAccountTransactionDto>();
            foreach (EmployerAccountTransaction entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E EmployerAccountTransaction to list of customized DTOs w/o checking entity state or entity navigation</summary>
        /// <typeparam name="T">Custom DTO derived from EmployerAccountTransactionDto</typeparam>
        /// <param name="entities">L2E EmployerAccountTransactions</param>
        /// <returns>List of DTO customized EmployerAccountTransactions</returns>
        public static List<T> ToDtoList<T>(this IQueryable<EmployerAccountTransaction> entities) where T : EmployerAccountTransactionDto, new()
        {
            List<T> dtos = new List<T>();
            foreach (EmployerAccountTransaction entity in entities)
            {
                dtos.Add((T)entity.ToDto(new T()));
            }
            return dtos;
        }

    }

}
