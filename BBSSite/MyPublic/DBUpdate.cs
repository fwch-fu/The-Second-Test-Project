using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBSSite.MyPublic
{
    public interface DBSaveChanges
    {
        bool SaveChanges(string TableName);
    }

    public interface IDBUpdate : DBSaveChanges
    {
        IDBUpdate Set(string name, object value, bool IsQuotation);
        IDBUpdate Where(string name, object value, bool IsQuotation);
        IDBUpdate And(string name, object value, bool IsQuotation);
    }
    public class DBUpdate : IDBUpdate
    {
        private int SetIndex = 0;
        private string SetField = " set {0}";
        private int WhereIndex = 0;
        private string WhereField = " where {0}";
        private int AndIndex = 0;
        private string AndField = " and {0}";
        public IDBUpdate Set(string name, object value, bool IsQuotation)
        {
            if (SetIndex == 0)
            {
                SetField = string.Format(SetField, name + "=" + (IsQuotation ? "'" + value + "'" : value)) + " {0}";
            }
            else
            {
                SetField = "," + string.Format(SetField, name + "=" + (IsQuotation ? "'" + value + "'" : value)) + " {0}";
            }
            SetIndex++;
            return this;
        }
        public IDBUpdate Where(string name, object value, bool IsQuotation)
        {
            if (WhereIndex == 0)
            {
                WhereField = string.Format(WhereField, name + "=" + (IsQuotation ? "'" + value + "'" : value));
            }
            else
            {
                return And(name, value, IsQuotation);
            }
            WhereIndex++;
            return this;
        }
        public IDBUpdate And(string name, object value, bool IsQuotation)
        {
            AndField = " and " + string.Format(WhereField, name + "=" + (IsQuotation ? "'" + value + "'" : value) + " {0}");
            AndIndex++;
            return this;
        }
        public bool SaveChanges(string TableName)
        {
            SetField = string.Format(SetField, "");
            if (SetIndex == 0)
            {
                return false;
            }
            if (WhereIndex == 0)
            {
                WhereField = "";
            }
            if (AndIndex == 0)
            {
                AndField = "";
            }
            else
            {
                AndField = string.Format(AndField, "");
            }
            using (Models.DB_BBSEntities db = new Models.DB_BBSEntities())
            {
                return db.DBUpdate(TableName, SetField, WhereField, AndField) > 0;
            }
        }
    }
}