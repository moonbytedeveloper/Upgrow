using System.ComponentModel.DataAnnotations;
using Upgrow.Application.Commands.WL;

namespace UpgrowAdminPanel.Models.WL
{
    public class MasterBannerVM /*: IValidatableObject*/
    {
        public WLMasterBannerCommand AddModel { get; set; } = new();
        public bool IsEdit { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (IsEdit)
        //    {
        //        // Validate UpdateModel
        //        var context = new ValidationContext(UpdateModel);
        //        Validator.TryValidateObject(UpdateModel, context, new List<ValidationResult>(), true);
        //    }
        //    else
        //    {
        //        // Validate AddModel
        //        var context = new ValidationContext(AddModel);
        //        Validator.TryValidateObject(AddModel, context, new List<ValidationResult>(), true);
        //    }
        //    yield break;
        //}

    }
}
