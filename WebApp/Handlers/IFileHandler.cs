namespace WebApp.Handlers;

public interface IFileHandler
{
    Task<string> UploadAsync(IFormFile file);
}
