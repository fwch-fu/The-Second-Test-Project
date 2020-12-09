using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Paging
{
    public abstract class APaging
    {
        public abstract IBinding Set(ISettingPaging SettingPaging, int CurrentPageIndex, int DataCount, int ShowDataCount, int ShowPageCount);
        public abstract int PageCount { get; }
        public abstract string Bind();
    }
    public class Paging : APaging, IDisposable
    {
        internal StringBuilder Append = null;
        private string DivBox = "<div class=\"WebPaging\">{0}</div>";
        private string StartTarget = "<{0}>";
        private string EndTarget = "</{0}>";
        private ISettingPaging SettingPaging = null;
        public Paging()
        {
            this.Append = new StringBuilder();
        }

        #region +实现抽象类或接口
        /// <summary>
        /// 构造分页类
        /// </summary>
        /// <param name="DataCount">数据总数</param>
        /// <param name="ShowDataCount">每页要显示的数据总数</param>
        /// <param name="ShowPageCount">最多显示的页码数</param>
        public override IBinding Set(ISettingPaging SettingPaging, int CurrentPageIndex, int DataCount, int ShowDataCount, int ShowPageCount)
        {
            if (!PagingStorage.IsNull)
            {
                PagingStorage.SettingPaging = SettingPaging;
                PagingStorage.IsNull = true;
            }
            PagingStorage.ShowDataCount = ShowDataCount;
            PagingStorage.ShowPageCount = ShowPageCount;
            PagingStorage.DataCount = DataCount;
            PagingStorage.CurrentPageIndex = CurrentPageIndex < 1 ? 1 : (CurrentPageIndex > PageCount ? PageCount : CurrentPageIndex);
            this.SettingPaging = PagingStorage.SettingPaging;
            return new Binding(this);
        }
        public override string Bind()
        {
            return string.Format(DivBox, Append.ToString());
        }
        private int _PageCount = -1;
        public override int PageCount
        {
            get
            {
                if (_PageCount == -1)
                {
                    this._PageCount = (PagingStorage.DataCount / PagingStorage.ShowDataCount) + ((PagingStorage.DataCount % PagingStorage.ShowDataCount) > 0 ? 1 : 0);
                }
                return this._PageCount;
            }
        }
        public void Dispose()
        {
            this.Append = null;
        }
        #endregion



        #region #内部算法
        internal string SetFrst
        {
            get
            {
                if (PagingStorage.CurrentPageIndex == 1)
                {
                    return string.Format(StartTarget, SettingPaging.FrstOrLast[0] +
                        " class=\"" + SettingPaging.FrstOrLast[4] + "\" ") +
                        SettingPaging.FrstOrLast[1] +
                        string.Format(EndTarget, SettingPaging.FrstOrLast[0]);
                }
                else
                {
                    return string.Format(StartTarget, SettingPaging.FrstOrLast[0] +
                        " class=\"" + SettingPaging.FrstOrLast[3] + "\" " +
                        string.Format(SettingPaging.FrstOrLast[5], 1)) +
                        SettingPaging.FrstOrLast[1] +
                        string.Format(EndTarget, SettingPaging.FrstOrLast[0]);
                }
            }
        }
        internal string SetLast
        {
            get
            {
                if (PagingStorage.CurrentPageIndex == PageCount)
                {
                    return string.Format(StartTarget, SettingPaging.FrstOrLast[0] +
                    " class=\"" + SettingPaging.FrstOrLast[4] + "\" ") +
                    SettingPaging.FrstOrLast[2] +
                    string.Format(EndTarget, SettingPaging.FrstOrLast[0]);
                }
                else
                {
                    return string.Format(StartTarget, SettingPaging.FrstOrLast[0] +
                    " class=\"" + SettingPaging.FrstOrLast[3] + "\" " +
                    string.Format(SettingPaging.FrstOrLast[5], PageCount)) +
                    SettingPaging.FrstOrLast[2] +
                    string.Format(EndTarget, SettingPaging.FrstOrLast[0]);
                }
            }
        }
        internal string SetFront
        {
            get
            {
                if (PagingStorage.CurrentPageIndex == 1)
                {
                    return string.Format(StartTarget, SettingPaging.FrontOrAfter[0] +
                    " class=\"" + SettingPaging.FrontOrAfter[4] + "\" ") +
                    SettingPaging.FrontOrAfter[1] +
                    string.Format(EndTarget, SettingPaging.FrontOrAfter[0]);
                }
                else
                {
                    return string.Format(StartTarget, SettingPaging.FrontOrAfter[0] +
                    " class=\"" + SettingPaging.FrontOrAfter[3] + "\" " +
                    string.Format(SettingPaging.FrontOrAfter[5], PagingStorage.CurrentPageIndex - 1)) +
                    SettingPaging.FrontOrAfter[1] +
                    string.Format(EndTarget, SettingPaging.FrontOrAfter[0]);
                }
            }
        }
        internal string SetAfter
        {
            get
            {
                if (PagingStorage.CurrentPageIndex == PageCount)
                {
                    return string.Format(StartTarget, SettingPaging.FrontOrAfter[0] +
                   " class=\"" + SettingPaging.FrontOrAfter[4] + "\" ") +
                   SettingPaging.FrontOrAfter[2] +
                   string.Format(EndTarget, SettingPaging.FrontOrAfter[0]);
                }
                else
                {
                    return string.Format(StartTarget, SettingPaging.FrontOrAfter[0] +
                    " class=\"" + SettingPaging.FrontOrAfter[3] + "\" " +
                    string.Format(SettingPaging.FrontOrAfter[5], PagingStorage.CurrentPageIndex + 1)) +
                    SettingPaging.FrontOrAfter[2] +
                    string.Format(EndTarget, SettingPaging.FrontOrAfter[0]);
                }
            }
        }
        internal StringBuilder SetPageNumber
        {
            get
            {
                int CurrentPageIndex = PagingStorage.CurrentPageIndex;
                int SeedStartIndex = PagingStorage.ShowPageCount / 2;
                if (CurrentPageIndex - SeedStartIndex > 0)
                {
                    if (CurrentPageIndex + SeedStartIndex <= PageCount)
                    {
                        PagingStorage.StartIndex = CurrentPageIndex - SeedStartIndex;
                        PagingStorage.EndIndex = PagingStorage.StartIndex + PagingStorage.ShowPageCount - 1;
                    }
                    else
                    {
                        PagingStorage.StartIndex = PageCount - PagingStorage.ShowPageCount + 1;
                        PagingStorage.EndIndex = PageCount;

                    }
                }
                else
                {
                    PagingStorage.StartIndex = 1;
                    PagingStorage.EndIndex = (PageCount < PagingStorage.ShowPageCount ? PageCount : PagingStorage.ShowPageCount);
                }
                StringBuilder Builder = new StringBuilder();
                for (int i = PagingStorage.StartIndex; i <= PagingStorage.EndIndex; i++)
                {
                    if (i == CurrentPageIndex)
                    {
                        Builder.Append(
                            string.Format(StartTarget, SettingPaging.PageNumber[0] +
                                " class=\"" + SettingPaging.PageNumber[2] + "\" ") + i +
                                string.Format(EndTarget, SettingPaging.PageNumber[0])
                            );
                    }
                    else
                    {
                        Builder.Append(
                            string.Format(StartTarget, SettingPaging.PageNumber[0] +
                                " class=\"" + SettingPaging.PageNumber[1] + "\" " +
                                string.Format(SettingPaging.PageNumber[3], i)) + i +
                                string.Format(EndTarget, SettingPaging.PageNumber[0])
                            );
                    }
                }
                return Builder;
            }
        }
        internal string SetInputPageNumber
        {
            get
            {
                return string.Format(StartTarget, SettingPaging.InputPageNumber[0] +
                  " class=\"" + SettingPaging.InputPageNumber[1] + "\" " + SettingPaging.InputPageNumber[2] + "/");
            }
        }
        internal string SetGotoPageIndex
        {
            get
            {
                return string.Format(StartTarget, SettingPaging.GotoPageIndex[0] +
                 " class=\"" + SettingPaging.GotoPageIndex[2] + "\" " + SettingPaging.GotoPageIndex[3] + " value=\"" + SettingPaging.GotoPageIndex[1] + "\"/");
            }
        }
        internal string SetShowChar
        {
            get
            {
                return string.Format(StartTarget, SettingPaging.ShowChar[0] +
                  " class=\"" + SettingPaging.ShowChar[1] + "\" " + SettingPaging.ShowChar[2]) +
                  "共" + PagingStorage.DataCount + "条记录,每页显示" + PagingStorage.ShowDataCount + "条,共" + PageCount + "页,当前第" + PagingStorage.CurrentPageIndex + "页," +
                  string.Format(EndTarget, SettingPaging.ShowChar[0]);
            }
        }
        #endregion
    }
}
