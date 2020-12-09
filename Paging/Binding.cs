using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Paging
{
    public interface IBinding
    {
        void GetFrst();
        void GetLast();
        void GetFront();
        void GetAfter();
        void GetPageNumber();
        void GetInputPageNumber();
        void GetGotoPageIndex();
        void GetShowChar();
    }
    public class Binding : IBinding
    {
        private StringBuilder Append = new StringBuilder();
        private Paging Paging = null;
        public Binding(Paging Paging)
        {
            this.Append = Paging.Append;
            this.Paging = Paging;
        }
        public void GetFrst()
        {
            Append.Append(Paging.SetFrst);
        }
        public void GetLast()
        {
            Append.Append(Paging.SetLast);
        }
        public void GetFront()
        {
            Append.Append(Paging.SetFront);
        }
        public void GetAfter()
        {
            Append.Append(Paging.SetAfter);
        }
        public void GetPageNumber()
        {
            Append.Append(Paging.SetPageNumber);
        }
        public void GetInputPageNumber()
        {
            Append.Append(Paging.SetInputPageNumber);
        }
        public void GetGotoPageIndex()
        {
            Append.Append(Paging.SetGotoPageIndex);
        }
        public void GetShowChar()
        {
            Append.Append(Paging.SetShowChar);
        }
    }
}
