
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace snglrtycrvtureofspce.Hotels.Desktop.Model
{

using System;
    using System.Collections.Generic;
    
public partial class tblRooms
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tblRooms()
    {

        this.tblReservations = new HashSet<tblReservations>();

    }


    public string RoomID { get; set; }

    public string RoomTypeID { get; set; }

    public int StatusID { get; set; }

    public decimal Cost { get; set; }

    public int RoomFloor { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblReservations> tblReservations { get; set; }

}

}
