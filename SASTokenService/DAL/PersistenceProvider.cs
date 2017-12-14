using SasTokenService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService.DAL
{
    /// <summary>
    /// Class To Create, Update ,Delete a new Device in Database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PersistenceProvider<TEntity> where TEntity : EntityBase
    {
        private PersistenceContext m_DbContext;

        /// <summary>
        /// Constructor to set m_DbContext
        /// </summary>
        public PersistenceProvider()
        {
            m_DbContext = new PersistenceContext();
        }
        /// <summary>
        /// Method To Create a new Device table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Create(TEntity entity)
        {
            entity.ChangedAt = entity.CreatedAt = DateTime.UtcNow;

            var newEntity = m_DbContext.Set<TEntity>().Add(entity);
            m_DbContext.SaveChanges();

            return newEntity;
        }
        /// <summary>
        /// Method To Delete a Device From Table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(TEntity entity)
        {
            m_DbContext.Set<TEntity>().Remove(entity);

            if (m_DbContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Method To Get all Devices from Table.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetItems()
        {
            return m_DbContext.Set<TEntity>();
        }
        /// <summary>
        /// Method To Update a existing Record 
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        public TEntity Update(TEntity updatedEntity)
        {
            updatedEntity.ChangedAt = DateTime.UtcNow;

            m_DbContext.Set<TEntity>().Attach(updatedEntity);
            var entry = m_DbContext.Entry<TEntity>(updatedEntity);
            entry.Property(p => p.CreatedAt).IsModified = false;
            entry.State = System.Data.Entity.EntityState.Modified;

            m_DbContext.SaveChanges();

            return updatedEntity;
        }
    }
}