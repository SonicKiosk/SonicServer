using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicServer.JsonClasses
{
    public struct ProductCatalog
    {
        public struct HeaderProperty
        {
            public string Version;
            public string DeploymentID;
            public string Name;
            public string ActivationDate;
            public string EndDate;
            public struct CMSDetailsProperty
            {
                public string GenerationDate;
            }
        }

        public struct DistributionProperty
        {
            public string Version;
            public List<MediaEntry> MediaList;
            public struct MediaEntry
            {
                public string RelativeFileName;
                public string MD5;
                public string Size;
            }
        }

        public struct Item
        {
            public struct DescriptionProperty
            {
                public string Language;
                public string Text;
            }

            public struct ImageProperty
            {
                public string Path;
                public string Type;
            }

            public struct MainStructProperty
            {
                public List<DescriptionProperty> Long;
                public List<Image> Images;
                public string PLU;
                public string SonicID;
            }

            public struct DescriptionsProperty
            {
                public List<DescriptionProperty> Long;
            }

            public DescriptionsProperty Descriptions;
            public List<ImageProperty> Images;

            public string PLU;
            public string SonicID;
        }

        public HeaderProperty Header;
        public DistributionProperty Distribution;
        public List<Item> Items;
    }
}
