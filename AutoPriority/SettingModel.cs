using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AutoPriority
{
    class SettingModel
    {
        [JsonProperty(PropertyName = "GolbalEnable")]
        private bool _GolbalEnable;

        [JsonIgnore]
        public bool GolbalEnable { get => GolbalEnable; set => OnSetVar(ref _GolbalEnable,value); }

        private T OnSetVar<T>(ref T var,T data)
        {
            var = data;
            return data;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
