using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Helpper
{
    public class UploadFile
    {
        [Required(ErrorMessage = "Please choose a file!")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions ="zip, pdf, png, jpg, jpeg, gif")]
        [Display(Name = "Upload file support idea")]
        public IFormFile FileUpload { get; set; }
    }
}
