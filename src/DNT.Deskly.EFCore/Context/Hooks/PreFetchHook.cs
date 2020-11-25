using Microsoft.EntityFrameworkCore;

namespace DNT.Deskly.EFCore.Context.Hooks
{
    public abstract class PreFetchHook<TEntity> : PreActionHook<TEntity>
    {
        public override EntityState HookState => EntityState.Unchanged;
    }
}