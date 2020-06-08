namespace AdminService.Dto
{
    public class ErrorDto
    {
        public string Message { get; set; }
    }

    public class BaseResponseDto
    {
        public ErrorDto Error { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
