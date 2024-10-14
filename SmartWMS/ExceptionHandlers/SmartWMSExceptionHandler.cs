namespace SmartWMS;

public class SmartWMSExceptionHandler : Exception
{
    public SmartWMSExceptionHandler() {}
    
    public SmartWMSExceptionHandler(string message) : base(message) {}
}