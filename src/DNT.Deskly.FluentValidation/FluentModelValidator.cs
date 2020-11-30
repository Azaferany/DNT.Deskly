using FluentValidation;

namespace DNT.Deskly.FluentValidation
{
    public abstract class FluentModelValidator<TModel> : AbstractValidator<TModel>
    {
        public virtual object ValidatorCaller { get; set; }
    }
}