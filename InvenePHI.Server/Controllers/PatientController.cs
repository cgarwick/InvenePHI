using InvenePHI.Server.Models;
using InvenePHI.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvenePHI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IFileManager fileManager) : ControllerBase
    {
        // POST api/<PatientController>
        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files)
        {
            // Call the FileUpload method from Injected FileManager
            var uploadResult = fileManager.FileUpload(files);

            // Return the result based on the upload status
            if (uploadResult.isSuccess)
            {
                return Ok(uploadResult.message);
            } 
            else
            {
                return BadRequest(uploadResult.message);
            }
        }
    }
}

