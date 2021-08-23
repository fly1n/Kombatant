using Kombatant.Settings.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombatant.Helpers
{
    class CustomConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            FullyObservableCollection<TargetObject> targetWhitelist = (FullyObservableCollection<TargetObject>)value;

            //LogHelper.Instance.Log($"Number of target whitelist is {targetWhitelist.Count}");
            writer.WriteStartArray();
            foreach (TargetObject obj in targetWhitelist)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("NpcId");
                writer.WriteValue(obj.NpcId);
                writer.WritePropertyName("Name");
                writer.WriteValue(obj.Name);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray jArray = JArray.Load(reader);
            FullyObservableCollection<TargetObject> targetWhitelist = new FullyObservableCollection<TargetObject>();
            foreach (JObject obj in jArray)
            {

                uint npcId = obj.GetValue("NpcId").Value<uint>();
                string name = obj.GetValue("Name").Value<string>();
                targetWhitelist.Add(new TargetObject(npcId, name));
            }

            return targetWhitelist;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FullyObservableCollection<TargetObject>);
        }
    }
}
