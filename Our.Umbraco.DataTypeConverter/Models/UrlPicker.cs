using System;
using Newtonsoft.Json;

namespace Our.Umbraco.DataTypeConverter.Models
{
    //{
    //  "type": "content",
    //  "meta": {
    //    "title": "",
    //    "newWindow": false
    //  },
    //  "typeData": {
    //    "url": "",
    //    "contentId": 1196,
    //    "mediaId": null
    //  }
    //}
    [Serializable]
    public class UrlPicker
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }

        public class MetaInfo
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("newWindow")]
            public bool NewWindow { get; set; }
        }

        [JsonProperty("typeData")]
        public TypeInfo TypeData { get; set; }

        public class TypeInfo
        {
            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("contentId")]
            public int? ContentId { get; set; }

            [JsonProperty("mediaId")]
            public int? MediaId { get; set; }
        }
    }    
}
