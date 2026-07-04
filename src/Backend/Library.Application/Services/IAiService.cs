namespace Library.Application.Services;

public interface IAiService
{
    Task<string> AskAsync(string question);
}