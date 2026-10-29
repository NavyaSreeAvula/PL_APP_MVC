using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PL_APP_MVC.Dtos
{
    public class RootDto
    {
        [ValidateNever]
        public int AppId { set; get; }

        [ValidateNever]
        public string Name { set; get; }

        [ValidateNever]
        public string AppStage { get; set; }

        [ValidateNever]
        public string AppStatus {  get; set; }

        [ValidateNever]
        public string Message {  get; set; }

    }
}
