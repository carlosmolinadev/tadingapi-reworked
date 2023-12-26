namespace TradingApp.Application.Models.Responses.Base
{
    public class Response
    {
        public Response()
        {
        }

        public Response(string message)
        {
            Message = message;
        }

        public Response(string message, bool success)
        {
            Success = success;
            Message = message;
        }
        public void MarkSuccess(){
            Success = true;
        }

        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;

        public void SetMessage(string message){
            this.Message = message;
        }
    }
}