
namespace FraudulentActivity.Notifications
{
public class Notification
{
    public int IndexAt {get;set;}
    public double Amount {get;set;}


}


public class Transaction
{
     public double Amount { get;set;}

     public int RepatedAmountTimes {get;set;}

     public int DayLimit {get;set;}

     public int TransactionIndexStart {get;set;}



}


public class TransactionOccurance 
{
   public int StartIndex 
   {
      get;
      set;
   }

   public int EndIndex 
   {
      get;
      set;
 

   }


   public List<Transaction> TransactionsFound {get;set;}

}

}