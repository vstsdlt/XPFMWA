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

    /// <summary>[Document]</summary>
    [Table("Document", Schema="dbo")]
    public class Document : BaseEntity
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

        /// <summary>[DocumentDescription]</summary>
        [MaxLength(500)]
        [Column("DocumentDescription")]
        public string DocumentDescription { get; set; }

        /// <summary>[DocumentID]</summary>
        [Key]
        [Required]
        [Column("DocumentID", Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

        /// <summary>[DocumentName]</summary>
        [Required]
        [MaxLength(255)]
        [Column("DocumentName")]
        public string DocumentName { get; set; }

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

        private ICollection<FormAttachment> formAttachments { get; set; }
        public virtual ICollection<FormAttachment> FormAttachments { get { return formAttachments ?? (formAttachments = new Collection<FormAttachment>()); } protected set { formAttachments = value; } }

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
            builder.Entity<Document>().Property(x => x.CreateUserId).IsUnicode(false);
            builder.Entity<Document>().Property(x => x.DocumentDescription).IsUnicode(false);
            builder.Entity<Document>().Property(x => x.DocumentName).IsUnicode(false);
            builder.Entity<Document>().Property(x => x.UpdateProcess).IsUnicode(false);
            builder.Entity<Document>().Property(x => x.UpdateUserId).IsUnicode(false);
        }

        /// <summary>Convert from Document entity to DTO</summary>
        /// <param name="dbContext">DB Context to use for setting DTO state</param>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <param name="entityDtos">Used internally to track which entities have been converted to DTO's already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant Document DTO</returns>
        public DocumentDto ToDtoDeep(FACTS.Framework.DAL.DbContext dbContext, DocumentDto dto = null, Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = null)
        {
            entityDtos = entityDtos ?? new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            if (entityDtos.ContainsKey(this))
            {
                return (DocumentDto)entityDtos[this];
            }

            dto = ToDto(dto);
            entityDtos.Add(this, dto);

            System.Data.Entity.Infrastructure.DbEntityEntry<Document> entry = dbContext?.Entry(this);
            dto.IsNew = (entry?.State == EntityState.Added);
            dto.IsDeleted = (entry?.State == EntityState.Deleted);

            if (entry?.Collection(x => x.FormAttachments)?.IsLoaded == true)
            {
                foreach (FormAttachment formAttachment in FormAttachments)
                {
                    dto.FormAttachments.Add(formAttachment.ToDtoDeep(dbContext, entityDtos: entityDtos));
                }
            }

            return dto;
        }

        /// <summary>Convert from Document entity to DTO w/o checking entity state or entity navigation</summary>
        /// <param name="dto">DTO to use if already created instead of creating new one (can be inherited class instead as opposed to base class)</param>
        /// <returns>Resultant Document DTO</returns>
        public DocumentDto ToDto(DocumentDto dto = null)
        {
            dto = dto ?? new DocumentDto();
            dto.IsNew = false;

            dto.CreateDateTime = CreateDateTime;
            dto.CreateUserId = CreateUserId;
            dto.DocumentDescription = DocumentDescription;
            dto.DocumentID = DocumentID;
            dto.DocumentName = DocumentName;
            dto.UpdateDateTime = UpdateDateTime;
            dto.UpdateNumber = UpdateNumber;
            dto.UpdateProcess = UpdateProcess;
            dto.UpdateUserId = UpdateUserId;

            return dto;
        }

        /// <summary>Convert from Document DTO to entity</summary>
        /// <param name="dbContext">DB Context to use for attaching entity</param>
        /// <param name="dto">DTO to convert from</param>
        /// <param name="dtoEntities">Used internally to track which dtos have been converted to entites already (to avoid re-converting when circularly referenced)</param>
        /// <returns>Resultant Document entity</returns>
        public static Document FromDto(FACTS.Framework.DAL.DbContext dbContext, DocumentDto dto, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities = null)
        {
            dtoEntities = dtoEntities ?? new Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity>();
            if (dtoEntities.ContainsKey(dto))
            {
                return (Document)dtoEntities[dto];
            }

            Document entity = new Document();
            dtoEntities.Add(dto, entity);
            FromDtoSet(dbContext, dto, entity, dtoEntities);

            if (dbContext != null)
            {
                dbContext.Entry(entity).State = (dto.IsNew ? EntityState.Added : (dto.IsDeleted ? EntityState.Deleted : EntityState.Modified));
            }

            return entity;
        }

        protected static void FromDtoSet(FACTS.Framework.DAL.DbContext dbContext, DocumentDto dto, Document entity, Dictionary<FACTS.Framework.Dto.BaseDto, BaseEntity> dtoEntities)
        {
            entity.CreateDateTime = dto.CreateDateTime;
            entity.CreateUserId = dto.CreateUserId;
            entity.DocumentDescription = dto.DocumentDescription;
            entity.DocumentID = dto.DocumentID;
            entity.DocumentName = dto.DocumentName;
            entity.UpdateDateTime = dto.UpdateDateTime;
            entity.UpdateNumber = dto.UpdateNumber;
            entity.UpdateProcess = dto.UpdateProcess;
            entity.UpdateUserId = dto.UpdateUserId;

            if (dto.FormAttachments != null)
            {
                foreach (FormAttachmentDto formAttachment in dto.FormAttachments)
                {
                    entity.FormAttachments.Add(DbEntities.FormAttachment.FromDto(dbContext, formAttachment, dtoEntities));
                }
            }
        }

    }

    /// <summary>Extension methods related to Document entity</summary>
    public static class DocumentExtension
    {

        /// <summary>Convert IEnumerable Document to list of DTOs</summary>
        /// <param name="entities">IEnumerable Documents</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO Documents</returns>
        public static List<DocumentDto> ToDtoListDeep(this IEnumerable<Document> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<DocumentDto> dtos = new List<DocumentDto>();
            foreach (Document entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E Document to list of DTOs</summary>
        /// <param name="entities">L2E Documents</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO Documents</returns>
        public static List<DocumentDto> ToDtoListDeep(this IQueryable<Document> entities, FACTS.Framework.DAL.DbContext dbContext)
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<DocumentDto> dtos = new List<DocumentDto>();
            foreach (Document entity in entities)
            {
                dtos.Add(entity.ToDtoDeep(dbContext, entityDtos: entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert L2E Document to list of customized DTOs</summary>
        /// <typeparam name="T">Custom DTO derived from DocumentDto</typeparam>
        /// <param name="entities">L2E Documents</param>
        /// <param name="dbContext">DB Context to use for setting state of DTO</param>
        /// <returns>List of DTO customized Documents</returns>
        public static List<T> ToDtoListDeep<T>(this IQueryable<Document> entities, FACTS.Framework.DAL.DbContext dbContext) where T : DocumentDto, new()
        {
            Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto> entityDtos = new Dictionary<BaseEntity, FACTS.Framework.Dto.BaseDto>();
            List<T> dtos = new List<T>();
            foreach (Document entity in entities)
            {
                dtos.Add((T)entity.ToDtoDeep(dbContext, new T(), entityDtos));
            }
            return dtos;
        }

        /// <summary>Convert IEnumerable Document to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">IEnumerable Documents</param>
        /// <returns>List of DTO Documents</returns>
        public static List<DocumentDto> ToDtoList(this IEnumerable<Document> entities)
        {
            List<DocumentDto> dtos = new List<DocumentDto>();
            foreach (Document entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E Document to list of DTOs w/o checking entity state or entity navigation</summary>
        /// <param name="entities">L2E Documents</param>
        /// <returns>List of DTO Documents</returns>
        public static List<DocumentDto> ToDtoList(this IQueryable<Document> entities)
        {
            List<DocumentDto> dtos = new List<DocumentDto>();
            foreach (Document entity in entities)
            {
                dtos.Add(entity.ToDto());
            }
            return dtos;
        }

        /// <summary>Convert L2E Document to list of customized DTOs w/o checking entity state or entity navigation</summary>
        /// <typeparam name="T">Custom DTO derived from DocumentDto</typeparam>
        /// <param name="entities">L2E Documents</param>
        /// <returns>List of DTO customized Documents</returns>
        public static List<T> ToDtoList<T>(this IQueryable<Document> entities) where T : DocumentDto, new()
        {
            List<T> dtos = new List<T>();
            foreach (Document entity in entities)
            {
                dtos.Add((T)entity.ToDto(new T()));
            }
            return dtos;
        }

    }

}
