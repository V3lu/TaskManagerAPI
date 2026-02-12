namespace TaskManagerAPI.DTOs
{
    public record TaskSendDto(int Id, string Title, bool IsCompleted);
    public record TaskReceiveDto(string Title, bool IsCompleted);
}
