using BookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.DAL
{
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>All records</returns>
        Task<List<T>> List();

        /// <summary>
        /// Returns a record by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The record with given id.</returns>
        Task<T> GetById(Guid id);

        /// <summary>
        /// Removes a record by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Task Remove(Guid id);

        /// <summary>
        /// Removes a record.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(T entity);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns></returns>
        Task Insert(T entity);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Saves the changes to the database.
        /// </summary>
        /// <returns></returns>
        Task Save();
    }
}
