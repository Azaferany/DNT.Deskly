using FluentValidation;

namespace DNT.Deskly.FluentValidation
{
    public abstract class FluentModelValidator<TModel> : AbstractValidator<TModel>
    {
        public virtual object ValidatorCaller { get; set; }
    }
    public abstract class FluentModelValidator<TModel, TValidatorCaller> : AbstractValidator<TModel>
    {
        public virtual TValidatorCaller ValidatorCaller { get; set; }
    }
}