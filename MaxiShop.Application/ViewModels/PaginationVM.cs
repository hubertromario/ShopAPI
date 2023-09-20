using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.ViewModels
{
     public class PaginationVM<T>
    {
        public int CurrentPage { get; set; }  // To say current page 

        public int Totalpages { get; set; }   //to find total no.of.pages

        public int PageSize {  get; set; }    //

        public int TotalNoOfRecords { get; set; } // no.of.records in a page

        public List<T> Items {  get; set; }  //afetr filter we will return 5 records and that 5 reords are return as list

        public bool HasPrevious => CurrentPage > 1;   // to check if it has previous page

        public bool HasNext => CurrentPage < Totalpages;   // to check if it has next page



            public PaginationVM(int currentPage,int totalpages,int pageSize,int totalNoOfRecords,List<T>items) {
               CurrentPage = currentPage;
               Totalpages = totalpages;
               PageSize = pageSize;
               TotalNoOfRecords = totalNoOfRecords;
               Items = items;
                
        
        }
    }
}
