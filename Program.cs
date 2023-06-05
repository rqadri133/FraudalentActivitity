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


  