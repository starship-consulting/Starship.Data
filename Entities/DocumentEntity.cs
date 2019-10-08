using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Starship.Core.Interfaces;
using Starship.Core.Security;

namespace Starship.Data.Entities {
    public class DocumentEntity : Dictionary<string, object>, HasId {

        public string Get(string key) {
            return Get<string>(key);
        }

        public T Get<T>(string key) {

            if(!ContainsKey(key)) {
                return default;
            }
            
            if(this[key] is JObject jObject) {
                return jObject.ToObject<T>();
            }

            if(this[key] is JArray jArray) {
                return jArray.ToObject<T>();
            }

            var value = this[key];

            try {
                if(value != null && typeof(T) != value.GetType() && !typeof(IConvertible).IsAssignableFrom(typeof(T))) {
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
                }
            }
            catch {
            }
            
            return (T)Convert.ChangeType(this[key], typeof(T));
        }
        
        public void Set(string key, object value) {

            if(!ContainsKey(key)) {
                Add(key, value);
            }
            else {
                this[key] = value;
            }
        }

        public string GetId() {
            return Id;
        }

        public void SetId(object value) {
            Id = value.ToString();
        }

        public bool HasParticipant(string id) {
            return GetParticipants().Any(participant => participant.Id == id);
        }

        public List<EntityParticipant> GetParticipants() {
            if(Participants == null) {
                return new List<EntityParticipant>();
            }

            return Participants.ToList();
        }

        public void RemoveParticipant(string key) {
            Participants = GetParticipants().Where(each => each.Id != key).ToList();
        }

        public void AddParticipant(string key, string value = "") {
            AddParticipant(new EntityParticipant(key, value));
        }

        public void AddParticipant(EntityParticipant participant) {
            var accountClaims = GetParticipants();
            accountClaims.Add(participant);
            Participants = accountClaims.ToList();
        }

        public object ConvertTo(Type type) {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(this), type);
        }

        [JsonProperty(PropertyName="id")]
        public string Id {
            get => Get<string>("id");
            set => Set("id", value);
        }

        [JsonProperty(PropertyName="$type")]
        public string Type {
            get => Get<string>("$type");
            set => Set("$type", value);
        }

        [JsonProperty(PropertyName="participants")]
        public List<EntityParticipant> Participants {
            get => Get<List<EntityParticipant>>("participants");
            set => Set("participants", value);
        }
    }
}