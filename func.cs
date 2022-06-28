using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;

namespace Function {
    public class AliceRequestBase
    {
        

        [JsonPropertyName("meta")]
        public AliceMetaModel Meta { get; set; }

        [JsonPropertyName("session")]
        public AliceSessionModel Session { get; set; }
        
        [JsonPropertyName("request")]
        public AliceRequestModel Request { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
    public class AliceMetaModel
    {
        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("interfaces")]
        public AliceInterfacesModel Interfaces { get; set; }
    }

    public class AliceInterfacesModel
    {
        [JsonPropertyName("screen")]
        public object Screen { get; set; }

        [JsonPropertyName("payments")]
        public object Payments { get; set; }

        [JsonPropertyName("account_linking")]
        public object AccountLinking { get; set; }

        
    }
    
    public class AliceSessionModel
    {
        [JsonPropertyName("new")]
        public bool New { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        [JsonPropertyName("skill_id")]
        public string SkillId { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("user")]
        public AliceSessionUserModel User { get; set; }

        [JsonPropertyName("application")]
        public AliceSessionApplicationModel Application { get; set; }

        
    }
    public class AliceSessionUserModel
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
    public class AliceSessionApplicationModel
    {
        [JsonPropertyName("application_id")]
        public string ApplicationId { get; set; }
    }

    public class AliceRequestModel
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("original_utterance")]
        public string OriginalUtterance { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }

        [JsonPropertyName("markup")]
        public AliceMarkupModel Markup { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        //nlu
        [JsonPropertyName("nlu")]
        public AliceNluModel Nlu { get; set; }
    }
    public class AliceMarkupModel
    {
        [JsonPropertyName("dangerous_context")]
        public bool DangerousContext { get; set; }
    }
    //nlu
    public class AliceNluModel
    {
        [JsonPropertyName("tokens")]
        public IEnumerable<string> Tokens { get; set; }

        [JsonPropertyName("entities")]
        public IEnumerable<NluEntity> Entities { get; set; }

        [JsonPropertyName("intents")]
        public object Intents { get; set; }
    }
	public class NluEntity
	{
		[JsonPropertyName("type")]
		public string type { get; set; }
		
		[JsonPropertyName("tokens")]
		public NluTokens tokens { get; set; }
		
		[JsonPropertyName("value")]
		[JsonConverter(typeof(NluValueConverter))]
		public NluValue value { get; set; }
	}
	public class NluTokens
	{
		[JsonPropertyName("start")]
		public int start { get; set; }
		[JsonPropertyName("end")]
		public int end { get; set; }
		
	}
	#region NLU
	public class NluValueConverter : EnumerableConverter<NluValue>
    {
        protected override NluValue ToItem(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            return AliceEntityModelConverterHelper.ToItem(ref reader, options);
        }

        protected override void WriteItem(Utf8JsonWriter writer, NluValue item, JsonSerializerOptions options)
        {
            ConverterHelper.WriteItem(writer, item, options);
        }
    }
	public static class ConverterHelper
    {
        public static void WriteItem<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            object newValue = null;
            if (value != null)
            {
                newValue = Convert.ChangeType(value, value.GetType(), CultureInfo.InvariantCulture);
            }

            JsonSerializer.Serialize(writer, newValue, options);
        }
    }
	public static class AliceEntityModelConverterHelper
    {
        private static readonly Dictionary<string, Type> _typeMap = new Dictionary<string, Type>
        {
            { AliceConstants.AliceEntityTypeValues.Geo, typeof(AliceEntityGeoModel) },
            { AliceConstants.AliceEntityTypeValues.Fio, typeof(AliceEntityFioModel) },
            { AliceConstants.AliceEntityTypeValues.Number, typeof(AliceEntityNumberModel) },
            { AliceConstants.AliceEntityTypeValues.DateTime, typeof(AliceEntityDateTimeModel) },
            { AliceConstants.AliceEntityTypeValues.String, typeof(AliceEntityStringModel) },
        };

        public static NluValue ToItem(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var readerAtStart = reader;

            string type = null;
            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                var jsonObject = jsonDocument.RootElement;

                type = jsonObject
                    .EnumerateObject()
                    .FirstOrDefault(x => x.Name == "type")
                    .Value.GetString();
            }
			var targetType = typeof(NluValue);
			if (type=="YANDEX.NUMBER") targetType = typeof(int);
			
            if (!string.IsNullOrEmpty(type)  )
            {
                if (type!="YANDEX.NUMBER")
					return JsonSerializer.Deserialize(ref readerAtStart, targetType, options) as NluValue;
				else
				{ 
					var intval = JsonSerializer.Deserialize(ref readerAtStart, targetType, options) as int;
					NluValue valres = new NluValue();
					valres.intvalue = intval;
					return valres;
				}
            }
			
            
        }
    }
	
	
	public class NluValue
	{
		[JsonPropertyName("value")]
		public string intvalue { get; set; }
		
		[JsonPropertyName("first_name")]
		public string first_name { get; set; }
		
		[JsonPropertyName("last_name")]
		public string last_name { get; set; }
		
		[JsonPropertyName("patronymic_name")]
		public string patronymic_name { get; set; }
		
		[JsonPropertyName("street")]
		public string street { get; set; }
		
		[JsonPropertyName("house_number")]
		public string house_number { get; set; }
		
		[JsonPropertyName("city")]
		public string city { get; set; }
		
		[JsonPropertyName("country")]
		public string country { get; set; }
		
		[JsonPropertyName("airport ")]
		public string airport  { get; set; }
		
		[JsonPropertyName("day")]
		public string day { get; set; }
		
		[JsonPropertyName("day_is_relative")]
		public string day_is_relative { get; set; }
		
	}
	#endregion
    public class BaseRequest {
        public string httpMethod { get; set; }
        public string body { get; set; }
    }

    public class Response {
        public int StatusCode { get; set; }
        public string Body { get; set; }

        public Response(int statusCode, string body) {
            StatusCode = statusCode;
            Body = body;
        }
    }
	public class AliceResponse
	{
		[JsonPropertyName("version")]
        public string Version { get; set; }
		
		[JsonPropertyName("response")]
		public AliceResponseModel Response { get; set; }
		
		public AliceResponse(AliceRequestBase request)
		{
			Version = request.Version;
			Response = new AliceResponseModel();
			Response.EndSession = false;
			var sh = new ShowMeta();
			//some random guid
			sh.content_id = "88f83f54-1135-4238-85c4-5e45959a64d0";
			sh.id = "88f83f54-1135-4238-85c4-5e45959a64d0";
			sh.publication_date = DateTime.UtcNow.ToString("o");
			
            if (request.Request.Command == "привет") 
                request.Request.Command = request.Request.Command + " медвед ";

			Response.ShowItemMeta = sh;
			Response.Text = "ПРИВЕТ ПОВЕЛИТЕЛЬ! "+request.Request.Command;
			Response.Tts = Response.Text;
			
		}
	}
	public class AliceResponseModel
	{
		private string _tts;
		private const int _textMaxLength = 1024;
        private const int _ttsMaxLength = 1024;

        private string _text;
		
		[JsonPropertyName("text")]
        public string Text
        {
            get => _text;

            set
            {
                _text = value;
			}
		}
		[JsonPropertyName("tts")]
        public string Tts
        {
            get => _tts;

            set
            {
                _tts = value;
			}
		}
		
		[JsonPropertyName("end_session")]
        public bool EndSession { get; set; }
		
		[JsonPropertyName("show_item_meta")]
        public ShowMeta ShowItemMeta { get; set; }
		
	}
	public class ShowMeta
	{
		[JsonPropertyName("publication_date")]
		public string publication_date { get; set; }
		[JsonPropertyName("id")]
		public string id { get; set; }
		[JsonPropertyName("content_id")]
		public string content_id { get; set; }
	}
	
    public class Handler {
        public AliceResponse FunctionHandler(AliceRequestBase request) {
			var response = new AliceResponse(request);
            return response;
        }
    }
}