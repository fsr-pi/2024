﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EF_EN.Model;

public partial class City
{
    public int CityId { get; set; }

    public string CountryCode { get; set; }

    public string CityName { get; set; }

    public int PostalCode { get; set; }

    public string PostalName { get; set; }

    public virtual Country CountryCodeNavigation { get; set; }

    public virtual ICollection<Partner> PartnerResidenceCities { get; set; } = new List<Partner>();

    public virtual ICollection<Partner> PartnerShipmentCities { get; set; } = new List<Partner>();
}