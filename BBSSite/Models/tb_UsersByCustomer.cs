//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BBSSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_UsersByCustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_UsersByCustomer()
        {
            this.tb_ForumInfoStatus = new HashSet<tb_ForumInfoStatus>();
            this.tb_ForumReport = new HashSet<tb_ForumReport>();
            this.tb_ForumSecond = new HashSet<tb_ForumSecond>();
            this.tb_ForumClassify = new HashSet<tb_ForumClassify>();
            this.tb_ForumMain = new HashSet<tb_ForumMain>();
        }
    
        public int ID { get; set; }
        public string UserName { get; set; }
        public byte[] UserPassword { get; set; }
        public string NickName { get; set; }
        public int SexID { get; set; }
        public int Age { get; set; }
        public bool IsModerator { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public Nullable<int> Fatieshu { get; set; }
        public Nullable<int> Huitieshu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ForumInfoStatus> tb_ForumInfoStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ForumReport> tb_ForumReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ForumSecond> tb_ForumSecond { get; set; }
        public virtual tb_ZY_Sex tb_ZY_Sex { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ForumClassify> tb_ForumClassify { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ForumMain> tb_ForumMain { get; set; }
    }
}
