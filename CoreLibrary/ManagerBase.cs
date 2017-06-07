using System;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class ManagerBase<T, TKey>
        where T : class, IIdentifiable<TKey>
    {
        protected IAsyncStore<T, TKey> Store { get; set; }

        public ManagerBase(IAsyncStore<T, TKey> store)
        {
            Store = store;
        }

        public virtual async Task<T> FindByIdAsync(TKey id)
        {
            return await Store.FindByIdAsync(id);
        }

        public virtual Task<T> FindUniqueAsync(T matchAgainst)
        {
            throw new NotImplementedException();
        }

        public virtual ManagerResult OnCreateLogicCheck(T entity)
        {
            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> CreateAsync(T entity)
        {
            try
            {
                T duplicate = await FindUniqueAsync(entity);

                if (duplicate != null)
                    return new ManagerResult(ManagerErrors.DuplicateOnCreate);

                ManagerResult logicCheckResult = OnCreateLogicCheck(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                await Store.CreateAsync(entity);
            }
            catch (NotImplementedException)
            {
                await Store.CreateAsync(entity);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public virtual ManagerResult OnUpdateLogicCheck(T entity)
        {
            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> UpdateAsync(T entity)
        {
            try
            {
                T found = await FindUniqueAsync(entity);

                if ((found != null) && (!found.Id.Equals(entity.Id)))
                {
                    return new ManagerResult(ManagerErrors.DuplicateOnUpdate);
                }

                ManagerResult logicCheckResult = OnUpdateLogicCheck(entity);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                await Store.UpdateAsync(entity);
            }
            catch (NotImplementedException)
            {
                await Store.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public virtual ManagerResult OnDeleteLogicCheck(T entity)
        {
            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> DeleteAsync(TKey id)
        {
            try
            {
                T found = await Store.FindByIdAsync(id);

                if (found == null)
                    return new ManagerResult(ManagerErrors.RecordNotFound);

                ManagerResult logicCheckResult = OnDeleteLogicCheck(found);

                if (!logicCheckResult.Success)
                    return logicCheckResult;

                await Store.DeleteAsync(found);
            }
            catch (Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }

        public virtual async Task<ManagerResult> DeleteAsync(T entity)
        {
            return await DeleteAsync(entity.Id);
        }
    }
}
