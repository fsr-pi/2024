﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EFModel;

public partial class Product
{
    public int ProductNumber { get; set; }

    public string ProductName { get; set; }

    public string UnitName { get; set; }

    public decimal Price { get; set; }

    public bool IsService { get; set; }

    public string Description { get; set; }

    public byte[] Photo { get; set; }

    public int? PhotoChecksum { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}