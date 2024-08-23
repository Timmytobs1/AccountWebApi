namespace AccountWebApi.Dtos.Transaction
{
    /*public class TransferDto
    {
        public string AccountNo { get; set; }
        public decimal Amount { get; set; }
    }*/
    public class TransferDto
    {
        public string SourceAccountNo { get; set; }
        public string DestinationAccountNo { get; set; }
        public decimal Amount { get; set; }
    }

}
