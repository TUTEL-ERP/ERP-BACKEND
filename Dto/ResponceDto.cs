namespace server.Dto
{
    public class ResponceDto
    {
        public string Message { get; set; } = "Success";
        public bool  IsSuccessed { get; set; } = true;
        public object? Data { get; set; } = null;
    }
}
