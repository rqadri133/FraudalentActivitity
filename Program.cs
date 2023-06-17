using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.IO;
using FraudulentActivity.Notifications;

// the assignment excercise question problem 
// owned by hacker rank i am a hacker rank 
// student and got 3 certificates
// i have only provided solution as learning process
// please dont cheat from it learning is fine
           IEnumerable<string> files = Directory.EnumerateFiles(Environment.CurrentDirectory + "//TestData//" );
           List<Line> lines = new List<Line>();
           // Currently its just one structure 
           foreach(string filepath in files)
           {
               lines = FileContextReader.ReadFile(filepath);
               string[] firstMultipleInput = lines[0].LineContent.Split(" ".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

               int n = Convert.ToInt32(firstMultipleInput[0]);
               string contextLine= lines[1].LineContent;

               int d = Convert.ToInt32(firstMultipleInput[1]);
               List<int> expenditure = contextLine.TrimEnd().Split(' ').ToList().Select(expenditureTemp => Convert.ToInt32(expenditureTemp)).ToList();
               
               int result = Result.activityNotifications(expenditure, d);

                
               List<TransactionOccurance> occurances = Result.checkSpendingAISpirals(expenditure,d);
               foreach(TransactionOccurance occurance in occurances)
               {
                 Console.WriteLine($"The Occurance of Fraud injection AI found at {occurance.StartIndex} and covered till {occurance.EndIndex}");
                  foreach(Transaction tran in occurance.TransactionsFound)
                  {

                    Console.WriteLine($"I have found  at this interval {tran.DayLimit} the Duplicate Amount is: {tran.Amount} for almost {tran.RepatedAmountTimes} number of Times");
 


                  }

               }

               Console.WriteLine($"Number of Notifications Send out for Data Test File {filepath}  is : {result} ");
               

           }
         
  /*

        int n = Convert.ToInt32(firstMultipleInput[0]);

        int d = Convert.ToInt32(firstMultipleInput[1]);

        
        int result = Result.activityNotifications(expenditure, d);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();

 */


public class FileContextReader
{
    public static List<Line> ReadFile(string fileName)
    {
        
        List<Line> lines = new List<Line>();
        using (StreamReader sr = File.OpenText(fileName))
        {
            int x = 0;
            while (!sr.EndOfStream)
            {
                //we're just testing read speeds
                var line = new Line();
                line.LineContent  = sr.ReadLine();
                line.Index = x;
                x += 1;
                lines.Add(line);
            }
        }
       return lines; 

     }  


 }  

public class Line
{
  public string LineContent 
  {
     get;
     set;
  }
  
  public int Index 
  {
      get;
      set;
  } 

}
  public class Result 
  {

     private static double GetMedian(List<int> expenditures)
     {
          expenditures.Sort();
          int middle = expenditures.Count % 2;
          int middle_num =  expenditures.Count / 2;
          double median = 0;
          if(middle == 0)
          {
              median = (double) (expenditures[middle_num] + expenditures[middle_num - 1]) / 2  ;
              
              
          }
          else 
          {
              // its an odd entry
              
              median = (double) expenditures[middle_num];
          }
          
          return median;
         
         
     }

    public static List<TransactionOccurance>  checkSpendingAISpirals(List<int> checkSameExpenditures , int days )
    { 
        Dictionary<int,int> limitExpenditure = new Dictionary<int, int>();
         int l = 0;
         int counter = 0 ;
         int indexer = 0 ;
         int repeat =0 ;
         Transaction transaction = new Transaction();
        TransactionOccurance occurance = new TransactionOccurance();
            
       //  List<int> limitExpenditure = new List<int>();
        List<Transaction> transactionsRepeat = new List<Transaction>();
        List<TransactionOccurance> occurances = new List<TransactionOccurance>();
         while( indexer < checkSameExpenditures.Count - 1  )
         {
            
            
            if(limitExpenditure.ContainsKey(checkSameExpenditures[counter]))
            {
                transaction = new Transaction();
                // A duplicate transaction entry found here 
                transaction.Amount = checkSameExpenditures[counter];
                transaction.RepatedAmountTimes = repeat;
                transaction.DayLimit = days; 
                transaction.TransactionIndexStart = indexer;
                transactionsRepeat.Add(transaction);
            

                repeat++;

            } 
            else 
            {
                repeat = 1;
                limitExpenditure.Add(checkSameExpenditures[indexer], indexer);
 


            }

            // find repeat transaction during the day limit cycle
            // this is different then the one notifications 
            // 
           if(counter == days)
           {
              // The problem here is during that sepecific days find that specific repeat 
              // Find all end during that duration from Start Index
              int currentIndexEnd  = indexer + days - 1;
              transactionsRepeat = transactionsRepeat.FindAll(p=>p.TransactionIndexStart <= currentIndexEnd); 
              // Number of repeat transactions found for counter slab 
              
              // if for any occurance
              occurance = new TransactionOccurance();
              occurance.TransactionsFound = transactionsRepeat; 
              occurance.StartIndex = indexer;
              occurance.EndIndex = currentIndexEnd;
              occurances.Add(occurance);
              l = l + 1;
              counter = l;
              
              

              
           }     
           counter = counter + 1;  
           // this is overall indexer will never updated to 0
           indexer = indexer + 1;

         }
        
         return occurances;
            



    }

    public static int activityNotifications(List<int> expenditure, int d)
    {
        int k=0 ;
        int counter = 1 ;
        double median = 0;
        List<int> newSpending = new List<int>();
        int notifications = 0;
        for(int j=k ; j < expenditure.Count - 1 ; j++ )
        {
            newSpending.Add(expenditure[j]);
            
        
            if(counter == d)
            {
             //  Console.WriteLine($"For the counter {counter} the limit day is {d} its New Spending List Item count {newSpending.Count} the first at {newSpending[0]} last at {newSpending[newSpending.Count - 1]}"); 
               
               
               median =  GetMedian(newSpending);
               median = median * 2;
              // Console.WriteLine($"The median is current {median} and expenditure is {expenditure[j+1]}");      
               
               if(median <= expenditure[j+1])
               {
                   // its identified as notification 
                 // notifications.Add(new Notification() {DayLimit = d , spending = median });
                  notifications++;
               }
               counter = 1 ;
                k = k+1;
                j = k;
               median = 0;
               newSpending.Clear();
                
            }
            counter = counter + 1;
         
            
            
        }
        return notifications;
    }

    }


  