namespace UniSozluk.Api.Domain.Models
{
    public class EmailConfirmation : BaseEntity
    {
        public string OldEmailAdress { get; set; }
        public string NewEmailAdress { get; set; }
    }
}
