using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Laserfiche.LicenseManager.LMO;

namespace PowerLFLicensing.Data
{
    public class LFProductLicense
    {
        internal ApplicationLimit App;

        public Guid GUID => App.Uuid;

        public int ProductId => App.ProductID;

        public string Name => App.Name;

        public string MinVersion => App.MinimumVersion;

        public string MaxVersion => App.MaximumVersion;

        public string[] AddOns => App.AddOns;

        public ReadOnlyDictionary<string, int?> Features => new ReadOnlyDictionary<string, int?>(App.Features);

        internal LFProductLicense(ApplicationLimit app)
        {
            App = app;
        }
    }
}