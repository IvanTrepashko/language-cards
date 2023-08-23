using LanguageCards.Domain.Abstractions;
using System.Linq.Expressions;

namespace MongoDb.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> AsQueryable();

        IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression);

        TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken);

        TEntity FindById(Guid id);

        Task<TEntity> FindByIdAsync(Guid id, CancellationToken cancellationToken);

        void InsertOne(TEntity document);

        Task InsertOneAsync(TEntity document, CancellationToken cancellationToken);

        void InsertMany(ICollection<TEntity> documents);

        Task InsertManyAsync(ICollection<TEntity> documents, CancellationToken cancellationToken);

        void ReplaceOne(TEntity document);

        Task ReplaceOneAsync(TEntity document, CancellationToken cancellationToken);

        void DeleteOne(Expression<Func<TEntity, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken);

        void DeleteById(Guid id);

        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

        void DeleteMany(Expression<Func<TEntity, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression, CancellationToken cancellationToken);
    }
}
