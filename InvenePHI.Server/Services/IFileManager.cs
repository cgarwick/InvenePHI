namespace InvenePHI.Server.Services
{
    public interface IFileManager
    {
        (bool isSuccess, string message) FileUpload(List<IFormFile> files);
        string FileRedact(IFormFile file);
        string IdentifyPHI(string line);
    }
}
