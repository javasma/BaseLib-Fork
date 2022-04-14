namespace BaseLib.Core.Services
{
   public interface ICoreServiceResponse
   {
      bool Succeeded { get; set; }
      string Reason { get; }
      System.Enum ReasonCode { get; set; }
      string[] Messages { get; set; }
   }
  

}
